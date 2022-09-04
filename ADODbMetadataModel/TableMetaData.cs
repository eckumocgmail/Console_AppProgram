

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace DataCommon.DatabaseMetadata
{
    public class TableMetaData: MyValidatableObject 
    {
        internal string multicount_name;
        internal string singlecount_name;

        public TableMetaData()
        {
        }

        /*public TableMetaData(TableMetaData metadata)
        {
            TableName = metadata.name;
            TableSchema = metadata.TableSchema;
            ColumnsMetadata = new Dictionary<string, ColumnMetadata>();
            foreach(var kv in metadata.columns)
            {
                ColumnsMetadata[kv.Key] = new ColumnMetadata(metadata.columns[kv.Key]);
            }
        }*/

        public int ID { get; set; } = 1;
        
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }

        [NotNullNotEmpty]
        public string TableName { get; set; }
        public string TableType { get; set; }
   

        [NotNullNotEmpty]
        [NotMapped]
        public IDictionary<string, ColumnMetaData> columns { get; set; }
            =new ConcurrentDictionary<string,ColumnMetaData>();

        [NotNullNotEmpty]
        public string PrimaryKey { get; set; } = "ID";

        /// <summary>
        /// Внешние ключи
        /// Ключ = [ColumnName] 
        /// Значение = [TableName]
        /// </summary>
        public IDictionary<string, string> ForeignKeys { get; set; } = new Dictionary<string, string>();

        internal bool ContainsBlob()
        {
            throw new NotImplementedException();
        }
    }
}
