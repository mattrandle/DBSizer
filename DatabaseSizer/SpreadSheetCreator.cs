using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DatabaseSizer.SizingMetaData;
using Microsoft.Office.Interop.Excel;
using Microsoft.Vbe.Interop;
using Action = System.Action;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace DatabaseSizer
{
    internal class SpreadSheetCreator
    {
        private const int StartRow = 5;
        private const int StartCol = 1;

        private const int TotalDataSizeIdx = 2;

        private const int TableNameIdx = 0;
        private const int NumRowsIdx = 1;
        private const int NumColsIdx = 2;
        private const int FixedDataSizeIdx = 3;
        private const int NumVariableColsIdx = 4;
        private const int MaxVarSizeIdx = 5;
        private const int NullBitmapIdx = 6;
        private const int VariableDataSizeIdx = 7;
        private const int RowSizeIdx = 8;
        private const int RowSppIdx = 9;
        private const int BinaryPrIdx = 10;
        private const int NumPagesIdx = 11;
        private const int TableSizeIdx = 12;
        private const int TotalIndexSizeIdx = 13;
        private const int TotalSizeIdx = 14;

        private const int IndexNameIdx = 15;
        private const int IndexTableNameIdx = 16;
        private const int IndexNumColsIdx = 17;
        private const int IndexFixedDataSizeIdx = 18;
        private const int IndexNumVariableColsIdx = 19;
        private const int IndexMaxVarSizeIdx = 20;
        private const int IndexNullBitmapIdx = 21;
        private const int IndexVariableDataSizeIdx = 22;
        private const int IndexRowSizeIdx = 23;
        private const int IndexRowsPpIdx = 24;
        private const int IndexSizeIdx = 25;

        private const string NonClusteredIndexSizeFunction = "CalculateNonClusteredIndexSize";
        private const string NonClusteredIndexSizeMacro = @"
            Public Function CalculateNonClusteredIndexSize(NumRows As Long, IndexRowsPerPage As Long) As Long    
                Dim PagesAtPreviousLevel As Double 
                Dim NumOfPages As Double 
                Dim TotalPages As Double 
                PagesAtPreviousLevel = NumRows 
                Do 
                    NumOfPages = (PagesAtPreviousLevel / IndexRowsPerPage)
                    TotalPages = TotalPages + NumOfPages
                    PagesAtPreviousLevel = NumOfPages
                Loop Until (NumOfPages <= 1)
                CalculateNonClusteredIndexSize = TotalPages * 8192
            End Function";

        private const string ClusteredIndexSizeFunction = "CalculateClusteredIndexSize";
        private const string ClusteredIndexSizeMacro = @"
            Public Function CalculateClusteredIndexSize(DataSize As Long, IndexRowsPerPage As Long) As Long    
                Dim PagesAtPreviousLevel As Double 
                Dim NumOfPages As Double 
                Dim TotalPages As Double   
                PagesAtPreviousLevel = DataSize 
                Do
                    NumOfPages = (PagesAtPreviousLevel / IndexRowsPerPage)
                    TotalPages = TotalPages + NumOfPages
                    PagesAtPreviousLevel = NumOfPages
                Loop Until (NumOfPages <= 1)
                CalculateClusteredIndexSize = TotalPages * 8192
            End Function";

        public void CreateSpreadSheet(SqlDatabase database, Action progressCallback, CancellationToken cancellationToken)
        {
            var excelApp = new Application();

            var workSheet = SetupWorksheet(excelApp);

            WriteHeaders(workSheet);

            var firstIndexCell = "";
            var lastIndexCell = "";
            var lastIndexRow = 1;
            var tables = database.Tables;
            for (var tableIdx = 0; tableIdx < tables.Count(); tableIdx++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (progressCallback != null)
                    progressCallback();

                var table = tables[tableIdx];
                var tableRowIdx = StartRow + tableIdx + 1;

                WriteTable(workSheet, table, tableRowIdx);

                var indexes = table.Indexes;
                for (var indexIdx = 0; indexIdx < indexes.Count(); indexIdx++)
                {
                    var sourceIndex = indexes[indexIdx];

                    WriteIndex(workSheet, lastIndexRow + StartRow, table, tableRowIdx, sourceIndex);

                    // Keep track of indexes for table
                    if (indexIdx == 0)
                        firstIndexCell = workSheet.Cells[StartRow + lastIndexRow, StartCol + IndexSizeIdx].Address;

                    if ((indexIdx == indexes.Count() - 1))
                        lastIndexCell = workSheet.Cells[StartRow + lastIndexRow, StartCol + IndexSizeIdx].Address;

                    lastIndexRow++;
                }

                WriteTotalTableIndexSize(workSheet, firstIndexCell, lastIndexCell, tableRowIdx, indexes);

                WriteTotalTableSize(workSheet, tableRowIdx);
            }

            if (tables.Count > 0)
                WriteTotalDatabaseSize(tables, workSheet);

            excelApp.Columns.AutoFit();
            excelApp.Visible = true;
        }

        private static Worksheet SetupWorksheet(Application excelApp)
        {
            var wb = excelApp.Workbooks.Add();
            var workSheet = (Worksheet) excelApp.ActiveSheet;
            var cm = wb.VBProject.VBComponents.Add(vbext_ComponentType.vbext_ct_StdModule).CodeModule;
            cm.AddFromString(ClusteredIndexSizeMacro);
            cm.AddFromString(NonClusteredIndexSizeMacro);
            return workSheet;
        }

        private static void WriteTotalDatabaseSize(IList<SqlTable> tables, Worksheet workSheet)
        {
            workSheet.Cells[TotalDataSizeIdx, 2].Formula =
                string.Format("=CEILING(SUM({0}:{1}) / 1048576, 1)",
                              workSheet.Cells[StartRow + 1, StartCol + TotalSizeIdx].Address,
                              workSheet.Cells[StartRow + tables.Count, StartCol + TotalSizeIdx].Address);
        }

        private static void WriteTotalTableSize(Worksheet workSheet, int tableRowIdx)
        {
            workSheet.Cells[tableRowIdx, StartCol + TotalSizeIdx].Formula =
                string.Format("={0}+{1}", workSheet.Cells[tableRowIdx, StartCol + TotalIndexSizeIdx].Address,
                              workSheet.Cells[tableRowIdx, StartCol + TableSizeIdx].Address);
        }

        private static void WriteTotalTableIndexSize(Worksheet workSheet, string firstIndexCell, string lastIndexCell,
                                                     int tableRowIdx, IList<SqlIndex> indexes)
        {
            var totalIndexSizeEquation = string.Format("=SUM({0}:{1})", firstIndexCell, lastIndexCell);
            if (indexes.Count > 0)
                workSheet.Cells[tableRowIdx, StartCol + TotalIndexSizeIdx].Formula = totalIndexSizeEquation;
            else
                workSheet.Cells[tableRowIdx, StartCol + TotalIndexSizeIdx].Value = 0;
        }

        private static void WriteIndex(Worksheet workSheet, int indexRow, SqlTable table, int tableRowIdx,
                                       SqlIndex sourceIndex)
        {
            workSheet.Cells[indexRow, StartCol + IndexTableNameIdx] = table.Name;
            workSheet.Cells[indexRow, StartCol + IndexNameIdx] = sourceIndex.Name;

            workSheet.Cells[indexRow, StartCol + IndexNumColsIdx] = sourceIndex.Columns.Count;
            workSheet.Cells[indexRow, StartCol + IndexFixedDataSizeIdx] = sourceIndex.FixedColStorageSize;
            workSheet.Cells[indexRow, StartCol + IndexNumVariableColsIdx] = sourceIndex.VarColCount;
            workSheet.Cells[indexRow, StartCol + IndexMaxVarSizeIdx] = sourceIndex.VarColStorageSize;

            workSheet.Cells[indexRow, StartCol + IndexNullBitmapIdx].Formula =
                string.Format("=2+(({0}+7)/8)", workSheet.Cells[tableRowIdx, StartCol + IndexNumColsIdx].Address);

            workSheet.Cells[indexRow, StartCol + IndexVariableDataSizeIdx].Formula =
                string.Format("=2+{0}+({1}*2)",
                              workSheet.Cells[indexRow, StartCol + IndexMaxVarSizeIdx].Address,
                              workSheet.Cells[indexRow, StartCol + IndexNumVariableColsIdx].Address);

            workSheet.Cells[indexRow, StartCol + IndexRowSizeIdx].Formula =
                string.Format("={0}+{1}+{2}+4",
                              workSheet.Cells[indexRow, StartCol + IndexFixedDataSizeIdx].Address,
                              workSheet.Cells[indexRow, StartCol + IndexVariableDataSizeIdx].Address,
                              workSheet.Cells[indexRow, StartCol + IndexNullBitmapIdx].Address);

            workSheet.Cells[indexRow, StartCol + IndexRowsPpIdx].Formula =
                string.Format("=8096/({0} + 2)", workSheet.Cells[indexRow, StartCol + RowSizeIdx].Address);

            var calcSizeFunction = sourceIndex.IsClustered
                                       ? ClusteredIndexSizeFunction
                                       : NonClusteredIndexSizeFunction;

            workSheet.Cells[indexRow, StartCol + IndexSizeIdx].Formula =
                string.Format("={0}({1}, {2})", calcSizeFunction,
                              workSheet.Cells[tableRowIdx, StartCol + NumRowsIdx].Address,
                              workSheet.Cells[indexRow, StartCol + IndexRowsPpIdx].Address);
        }

        private static void WriteTable(Worksheet workSheet, SqlTable table, int tableRowIdx)
        {
            workSheet.Cells[tableRowIdx, StartCol + TableNameIdx] = table.Name;
            workSheet.Cells[tableRowIdx, StartCol + NumRowsIdx] = 0;
            workSheet.Cells[tableRowIdx, StartCol + NumColsIdx] = table.Columns.Count;
            workSheet.Cells[tableRowIdx, StartCol + FixedDataSizeIdx] = table.FixedColStorageSize;
            workSheet.Cells[tableRowIdx, StartCol + NumVariableColsIdx] = table.VarColCount;
            workSheet.Cells[tableRowIdx, StartCol + MaxVarSizeIdx] = table.VarColStorageSize;

            workSheet.Cells[tableRowIdx, StartCol + NullBitmapIdx].Formula =
                string.Format("=2+(({0}+7)/8)", workSheet.Cells[tableRowIdx, StartCol + NumColsIdx].Address);

            workSheet.Cells[tableRowIdx, StartCol + VariableDataSizeIdx].Formula =
                string.Format("=2+{0}+({1}*2)",
                              workSheet.Cells[tableRowIdx, StartCol + MaxVarSizeIdx].Address,
                              workSheet.Cells[tableRowIdx, StartCol + NumVariableColsIdx].Address);

            workSheet.Cells[tableRowIdx, StartCol + RowSizeIdx].Formula =
                string.Format("={0}+{1}+{2}+4",
                              workSheet.Cells[tableRowIdx, StartCol + FixedDataSizeIdx].Address,
                              workSheet.Cells[tableRowIdx, StartCol + VariableDataSizeIdx].Address,
                              workSheet.Cells[tableRowIdx, StartCol + NullBitmapIdx].Address);

            workSheet.Cells[tableRowIdx, StartCol + RowSppIdx].Formula =
                string.Format("=8096/({0} + 2)", workSheet.Cells[tableRowIdx, StartCol + RowSizeIdx].Address);

            workSheet.Cells[tableRowIdx, StartCol + NumPagesIdx].Formula =
                string.Format("=CEILING({0}/{1},1)",
                              workSheet.Cells[tableRowIdx, StartCol + NumRowsIdx].Address,
                              workSheet.Cells[tableRowIdx, StartCol + RowSppIdx].Address);

            workSheet.Cells[tableRowIdx, StartCol + BinaryPrIdx] = table.AvgBinaryPerRow;

            workSheet.Cells[tableRowIdx, StartCol + TableSizeIdx].Formula =
                string.Format("=(8192*{0})+({1}*{2})",
                              workSheet.Cells[tableRowIdx, StartCol + NumPagesIdx].Address,
                              workSheet.Cells[tableRowIdx, StartCol + BinaryPrIdx].Address,
                              workSheet.Cells[tableRowIdx, StartCol + NumRowsIdx].Address);

            workSheet.Cells[tableRowIdx, StartCol + TotalIndexSizeIdx].Value = 0;
        }

        private static void WriteHeaders(Worksheet workSheet)
        {
            workSheet.Cells[TotalDataSizeIdx, 1] = "Total Size Of Database (MB)";

            workSheet.Cells[StartRow, StartCol + TableNameIdx] = "Table Name";
            workSheet.Cells[StartRow, StartCol + NumRowsIdx] = "Number of Rows";
            workSheet.Cells[StartRow, StartCol + NumColsIdx] = "Number of Cols";
            workSheet.Cells[StartRow, StartCol + FixedDataSizeIdx] = "Fixed Data Size";
            workSheet.Cells[StartRow, StartCol + NumVariableColsIdx] = "Var Col Count";
            workSheet.Cells[StartRow, StartCol + MaxVarSizeIdx] = "Var Size";
            workSheet.Cells[StartRow, StartCol + NullBitmapIdx] = "Null Bitmap";
            workSheet.Cells[StartRow, StartCol + VariableDataSizeIdx] = "Var Data Size";
            workSheet.Cells[StartRow, StartCol + RowSizeIdx] = "Row Size";
            workSheet.Cells[StartRow, StartCol + RowSppIdx] = "Rows Per Page";
            workSheet.Cells[StartRow, StartCol + BinaryPrIdx] = "Average Binary Size Per Row";
            workSheet.Cells[StartRow, StartCol + NumPagesIdx] = "Number of Pages";
            workSheet.Cells[StartRow, StartCol + TableSizeIdx] = "Table Size (bytes)";
            workSheet.Cells[StartRow, StartCol + TotalIndexSizeIdx] = "Total Index Size (bytes)";
            workSheet.Cells[StartRow, StartCol + TotalSizeIdx] = "Total Size (bytes)";

            workSheet.Cells[StartRow, StartCol + IndexNameIdx] = "Index Name";
            workSheet.Cells[StartRow, StartCol + IndexTableNameIdx] = "Table Name";
            workSheet.Cells[StartRow, StartCol + IndexNumColsIdx] = "Number of Cols";
            workSheet.Cells[StartRow, StartCol + IndexFixedDataSizeIdx] = "Fixed Data Size";
            workSheet.Cells[StartRow, StartCol + IndexNumVariableColsIdx] = "Var Col Count";
            workSheet.Cells[StartRow, StartCol + IndexMaxVarSizeIdx] = "Var Size";
            workSheet.Cells[StartRow, StartCol + IndexNullBitmapIdx] = "Null Bitmap";
            workSheet.Cells[StartRow, StartCol + IndexVariableDataSizeIdx] = "Var Data Size";
            workSheet.Cells[StartRow, StartCol + IndexRowSizeIdx] = "Row Size";
            workSheet.Cells[StartRow, StartCol + IndexRowsPpIdx] = "Rows Per Page";
            workSheet.Cells[StartRow, StartCol + IndexSizeIdx] = "Total Index Size";
        }
    }
}