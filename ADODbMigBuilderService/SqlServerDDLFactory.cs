 
using DataCommon.DatabaseMetadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class SqlServerDDLFactory : IDDLFactory
    {

        public TableMetaData CreateTableMetaData(Type typeofEntity)
        {
            TableMetaData tmd = new TableMetaData();
            tmd.TableName = typeofEntity.Name;
            tmd.columns = new Dictionary<string, ColumnMetaData>();
            foreach (var key in typeofEntity.GetProperties().Select(p=>p.Name))
            {


                try
                {
                    var p = typeofEntity.GetProperty(key);
                    var propertyType = typeofEntity.GetProperty(p.Name).PropertyType;
                    var meta = new ColumnMetaData()
                    {
                        IsPrimary = p.Name == "ID",
                        IsIncremental = p.Name == "ID",
                        nullable = false,
                        ColumnCaption = typeofEntity.GetPropertyLabel( p.Name),
                        CollationDescription = typeofEntity.GetPropertyDescription(  p.Name),
                        ColumnName = p.Name.ToTSQLStyle(),
                        IsUnique = typeofEntity.IsPropertyUniq(  p.Name),
                        DataType = TypeExtensions2.GetTypeName(p.PropertyType),
                        InputType = typeofEntity.GetPropertyInputType(  p.Name)
                    };


                    meta.EnsureIsValide();
                    meta.CSharpType =
                        p.Name == "ID" ? "int" :
                            GetDataTypeFor(propertyType);
                    tmd.columns[p.Name] = meta;
                }
                catch( Exception )
                {
                    continue;
                }
            }
            tmd.EnsureIsValide();
            return tmd;
        }

        private bool IsNullable(Type typeofEntity, string name)
        {
            throw new NotImplementedException();
        }

        private string GetDataTypeFor(Type propertyType)
        {
            switch(TypeExtensions2.GetTypeName(propertyType).Trim())
            {
                
                case "Int32": 
                case "Int64": 
                case "Int": 
                    return "int";
                case "DateTime": return "date";
                case "String": return "nvarchar(max)";
                default: throw new Exception("Не поддерживаемый тип данных " + propertyType.GetTypeName());
            }
        }




        /// <summary>
        /// Создать внешний ключ
        /// </summary>
        /// <returns></returns>
        public string CreateForeignkey(string relativeTable, string table, string column, bool? onDeleteCascade = false, bool? onUpdateCascade = null)
        {
            return $"ALTER TABLE {table} ADD CONSTRAINT {table + relativeTable} FOREIGN KEY {column} REFERENCES ({relativeTable})" + " ON DELETE " + (onDeleteCascade == true ? "CASCADE" : "NO ACTION ") + " ON UPDATE " + (onUpdateCascade == true ? "CASCADE" : "NO ACTION ");
        }


        /// <summary>
        /// Создать таблицу
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public string CreateTable(TableMetaData metadata)
        {
            string sql = $"CREATE TABLE [{metadata.TableSchema}].[{metadata.TableName}]\n";
            sql += "(";
            Action<KeyValuePair<string, ColumnMetaData>> AddColumnDefinition = (p) => {
                sql += $"\n  {p.Key.ToTSQLStyle()} {p.Value.DataType} ";
                if (p.Key.ToLower() == metadata.PrimaryKey.ToLower())
                {
                    if (p.Value.IsIncremental)
                    {
                        sql += " IDENTITY(1,1) ";
                    }
                    sql += ((p.Value.nullable) ? " NULL " : "NOT NULL ");
                    sql += " PRIMARY KEY,";
                }
                else
                {
                    sql += ((p.Value.nullable) ? " NULL " : "NOT NULL ");
                    sql += ",";
                }
            };
            foreach (var p in metadata.columns)
            {
                if (p.Key == "ID")
                    AddColumnDefinition(p);
            }
            foreach (var p in metadata.columns)
            {
                if (p.Key != "ID")
                    AddColumnDefinition(p);
            }
            if (metadata.columns.Count > 0)
            {
                sql = sql.Substring(0, sql.Length - 1);
            }
            sql += "\n)\n";
            return sql;
        }

        public string CreateTable(Type metadata)
        {
            var tableMetaData = CreateTableMetaData(metadata);
            return CreateTable(tableMetaData);
        }

        
    }
}
