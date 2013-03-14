using System.Linq;
using System.Reflection;
using DatabaseSizer.Helpers;

namespace DatabaseSizer.SqlDataTypes
{
    public static class SqlDataTypeFactory
    {
        private static readonly SqlDataTypeDefinitions DataTypes;

        static SqlDataTypeFactory()
        {
            var xml = ResourceUtils.GetFileResourceAsString(Assembly.GetExecutingAssembly(),
                                                            "DatabaseSizer.SqlDataTypes.SQLServerDataTypes.xml");
            DataTypes = SqlDataTypeDefinitions.Deserialize(xml);
        }

        public static SqlDataType GetDataTypeByName(string name)
        {
            return DataTypes.SqlDataTypes.SingleOrDefault(a => (a.Name == name.ToLower()));
        }
    }
}