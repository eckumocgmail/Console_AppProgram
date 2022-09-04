
using DataADO;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace DataCommon
{
    public class DataTableService: IDataTableService
    {

        public IEnumerable<string> GetColumnsNames(DataTable dataTable)
        {
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                columnNames.Add(column.ColumnName);
            }
            return columnNames.ToArray();
        }

        
        public IDictionary<string, string> GetColumnsCaptions(DataTable dataTable)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                result[column.ColumnName] = column.Caption;
            }
            return result;
        }

        public IDictionary<string, Type> GetDataTypes(DataTable dataTable)
        {
            IDictionary<string, Type> result = new Dictionary<string, Type>();
            foreach (DataColumn column in dataTable.Columns)
            {
                result[column.ColumnName] = column.DataType;
            }
            return result;
        }


        public IEnumerable<IDictionary<string, string>> GetTextData(DataTable dataTable)
        {
            var result = new List<IDictionary<string, string>>();
            foreach (DataRow row in dataTable.Rows)
            {
                IDictionary<string, string> data = new Dictionary<string, string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    object value = row[column.ColumnName];
                    data[column.ColumnName] = value==null? "NULL": value.ToString();
                }
                result.Add(data);
            }
            return result.ToArray();
        }

        public IEnumerable<IDictionary<string, object>> GetRowsData(DataTable dataTable)
        {
            var result = new List<IDictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    object value = row[column.ColumnName];
                    data[column.ColumnName] = value;
                }
                result.Add(data);
            }
            return result.ToArray();
        }

        public IEnumerable<string> GetTextColumn(DataTable dataTable, string columnName)
        {
            List<string> result = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                object value = row[columnName];
                string text = value == null ? "NULL" : value.ToString();
                result.Add(text);
            }
            return result.ToArray();
        }

        public IEnumerable<TRecord> GetResultSet<TRecord>(DataTable dataTable) where TRecord: class
        {
            Type type = typeof(TRecord);
            var properties = type.GetOwnPropertyNames();
            var result = new List<TRecord>();
            var columns = GetColumnsNames(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                object next = type.New();
                foreach(var name in properties)
                {
                    string columnName = name.ToTSQLStyle();
                    string key = columns.Contains(columnName) ? columnName : columnName.ToCapitalStyle();
                    if (columns.Contains(key) == false)
                        continue;
                    try
                    {
                        object value = row[columnName];
                        if (value != null)
                        {
                            Setter.SetValue(next, name, value.ToString());
                        }
                    }
                    catch(Exception ex)
                    {
                        this.Error("Ошибка при разборе свойства "+ name);
                        this.Error(ex);
                    }
                                     
                }
                
                
                
                if (next is MyValidatableObject )
                    ((MyValidatableObject )next).EnsureIsValide();
                result.Add((TRecord)next);
                
                    
                
                
            }
            return result.ToArray();
        }

        public JArray GetJArray(DataTable dataTable)
        {
            Dictionary<string, object> resultSet = new Dictionary<string, object>();
            List<Dictionary<string, object>> listRow = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> rowSet = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    rowSet[column.Caption] = row[column.Caption];
                }
                listRow.Add(rowSet);
            }
            resultSet["rows"] = listRow;

            JObject jrs = JObject.FromObject(resultSet);
            return (JArray)jrs["rows"];
        }

        public IEnumerable<dynamic> GetResultSet(DataTable dataTable, Type entity)
        {
            Type type = entity;
            var properties = type.GetOwnPropertyNames();
            var result = new List<dynamic>();
            var columns = GetColumnsNames(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                object next = type.New();
                foreach (var name in properties)
                {
                    string columnName = name.ToTSQLStyle();
                    string key = columns.Contains(columnName) ? columnName : columnName.ToCapitalStyle();
                    if (columns.Contains(key) == false)
                        continue;
                    try
                    {
                        object value = row[columnName];
                        if (value != null)
                        {
                            
                            Setter.SetValue(next, name, value.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Error("Ошибка при разборе свойства " + name);
                        this.Error(ex);
                    }

                }



                if (next is MyValidatableObject )
                    ((MyValidatableObject )next).EnsureIsValide();
                result.Add((dynamic)next);




            }
            return result.ToArray();
        }
    }
}
