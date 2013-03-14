using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.DBSizer.UI
{
    public class ColumnOwnerBase : SizingItem
    {
        private const int staticPerRow = 4;
        private const int pageSize = 8096;

        public ColumnOwnerBase(string name, IEnumerable<SqlColumn> columns) : base(name)
        {
            this.Columns = columns;
        }

        public IEnumerable<SqlColumn> Columns 
        { 
            get; 
            private set; 
        }
                    
        public int RowSize
        {
            get 
            { 
                return FixedColStorageSize + VarColStorageSize + NullBitmap + staticPerRow; 
            }           
        }
    
        public int NullBitmap
        {
            get 
            { 
                return Convert.ToInt32(2 + ((Columns.Count() + 7) / 8)); 
            }
        }
    
        public int FixedColStorageSize
        {
            get 
            { 
                return Columns.Where(a => a.DataTypeInfo.IsFixedStorageSize).Sum(a => a.StorageSize); 
            }
        }

        public int VarColStorageSize
        {
            get
            {
                var varCols = Columns.Where(a => !(a.DataTypeInfo.IsFixedStorageSize));
                var storageSum = varCols.Sum(a => a.StorageSize);
                return storageSum + 2 + (varCols.Count() * 2);
            }
        }

        public int RowsPerPage
        {
            get 
            {
                return (pageSize / (RowSize + 2));
            }
        }

        public int FreeRowsPerPage
        {
            get 
            { 
                return pageSize * ((100 - IndexFillFactor) / 100) / (RowSize + 2); 
            }
        }

        public int IndexFillFactor
        {
            get 
            {
                // Todo - calculate if we have a clustered index on this table
                return 100; 
            }
        }            
    
        public int AvgBinaryPerRow
        {
            get 
            {
                return Columns.Sum(a => a.AvgBinarySize); 
            }
        }
    }
}
