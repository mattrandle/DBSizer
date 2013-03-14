using System.Collections.Generic;

namespace DatabaseSizer.SqlDataTypes
{
    public partial class SqlDataType
    {
        private readonly List<string> _synonyms;

        public SqlDataType()
        {
            this._synonyms = new List<string>();
        }

        public bool IsFixedStorageSize
        {
            get { return (this.StorageCharacteristics == StorageCharacteristicsEnum.Fixed); }
        }

        public void AddSynonym(string name)
        {
            this._synonyms.Add(name);
        }

        //private bool IsSynonym(string name)
        //{
        //    return this._synonyms.Any(a => a == name);
        //}
    }
}