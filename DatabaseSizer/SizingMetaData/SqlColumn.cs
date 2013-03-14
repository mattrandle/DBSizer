using DatabaseSizer.SqlDataTypes;

namespace DatabaseSizer.SizingMetaData
{
    public class SqlColumn : SizingItem
    {
        public SqlColumn(string name, SqlDataType dataTypeInfo, int maxLength, long avgSize, long avgBinarySize)
            : base(name)
        {
            this.DataTypeInfo = dataTypeInfo;
            this.MaxLength = maxLength;
            this.AvgSize = avgSize;
            this.AvgBinarySize = avgBinarySize;
        }

        public SqlDataType DataTypeInfo { get; private set; }

        public int MaxLength { get; private set; }

        public long AvgSize { get; private set; }

        public long AvgBinarySize { get; private set; }

        public long StorageSize
        {
            get
            {
                var result = 0L;
                switch (this.DataTypeInfo.StorageCharacteristics)
                {
                    case StorageCharacteristicsEnum.Fixed:
                        result = this.DataTypeInfo.UnitStorageSize + this.DataTypeInfo.StorageOverhead;
                        break;
                    case StorageCharacteristicsEnum.FixedByLength:
                        result = (this.DataTypeInfo.UnitStorageSize*this.MaxLength) + this.DataTypeInfo.StorageOverhead;
                        break;
                    case StorageCharacteristicsEnum.Variable:
                        result = (this.DataTypeInfo.UnitStorageSize*this.AvgSize) + this.DataTypeInfo.StorageOverhead;
                        break;
                }
                return result;
            }
        }
    }
}