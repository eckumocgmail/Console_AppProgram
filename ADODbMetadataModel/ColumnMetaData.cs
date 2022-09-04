

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace DataCommon.DatabaseMetadata
{
    public class ColumnMetaData: MyValidatableObject 
    {

        

        public int ID { get; set; }

        //[NotNullNotEmpty]
        public string TableCatalog { get; set; }

        //[NotNullNotEmpty]
        public string TableSchema { get; set; }
        
        //[NotNullNotEmpty]
        public string TableName { get; set; }

        //[NotNullNotEmpty]
        public string ColumnName { get; set; }

        //[InputNumber]
        //[IsPositiveNumber]
        //[NotNullNotEmpty]
        public int OrdinalPosition { get; set; }
        public bool IsComputed { get; set; } = false;
        public string Formula { get; set; }
        public bool nullable { get; set; }
        public bool IsUnique { get; set; }
        public bool IsPrimary { get; set; }
        public string InputType { get; set; }
        public string DataType { get; set; }

        //[Label("Параметр сортировки")]
        public string name { get; set; }        
        public string CharacterSetName { get; set; }
        public string ColumnCaption { get; set; }
        public string CollationDescription { get; set; }
        public string CSharpType { get; set; }
        public bool IsIncremental { get; set; }

        public ColumnMetaData() { }
        public ColumnMetaData(ColumnMetaData columnMetaData)
        {
            DataType = columnMetaData.DataType;
            ColumnName = columnMetaData.ColumnName;
        }

    }
}
