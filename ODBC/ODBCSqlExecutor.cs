
using Newtonsoft.Json.Linq;
using System;

using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using DataADO;
using DataCommon;
using DataCommon.DatabaseMetadata;

public class ODBCSqlExecutor : ODBCDbConnector, ISqlExecutor
{
    private DataTableService DataTableService = new DataTableService();
    public ODBCSqlExecutor(string name, string username, string password) : base(name, username, password)
    {
    }
    public virtual ODBCDbMetaData GetDatabaseMetadata()
    {





        ODBCDbMetaData metadata = new ODBCDbMetaData();
        using (System.Data.Odbc.OdbcConnection connection = GetConnection())
        {
            connection.Open();

            metadata.Driver = connection.Driver;
            metadata.Database = connection.Database;
            object site = connection.Site;

            metadata.ServerVersion = connection.ServerVersion;
            metadata.ConnectionString = connection.ConnectionString;


            DataTable columns = connection.GetSchema("Columns");
            foreach (DataRow row in columns.Rows)
            {
                string table = row["TABLE_NAME"].ToString();
                string column = row["COLUMN_NAME"].ToString();
                string type = row["TYPE_NAME"].ToString();
                string catalog = row["TABLE_CAT"].ToString();
                string schema = row["TABLE_SCHEM"].ToString();
                string description = row["COLUMN_DEF"].ToString();
                string nullable = row["NULLABLE"].ToString();

                //исколючаем системные таблицы и служебные
                if (schema == "sys" || schema == "INFORMATION_SCHEMA" || table.ToLower().IndexOf("migration") != -1)
                {
                    continue;
                }



                if (!metadata.Tables.ContainsKey(table))
                {
                    metadata.Tables[table] = new TableMetaData();
                    metadata.Tables[table].TableName = table;
                    metadata.Tables[table].TableName = "";

                    //определение наименования в множественном числе и единственном                        
                    string tableName = table.ToLower();
                    if (tableName.EndsWith("s"))
                    {
                        if (tableName.EndsWith("ies"))
                        {
                            metadata.Tables[table].multicount_name = tableName.ToLower();
                            metadata.Tables[table].singlecount_name = tableName.Substring(0, tableName.Length - 3).ToLower() + "y";
                        }
                        else
                        {
                            metadata.Tables[table].multicount_name = tableName.ToLower();
                            metadata.Tables[table].singlecount_name = tableName.Substring(0, tableName.Length - 1).ToLower();
                        }
                    }
                    else
                    {
                        if (tableName.EndsWith("y"))
                        {
                            metadata.Tables[table].multicount_name = tableName.Substring(0, tableName.Length - 1) + "ies";
                            metadata.Tables[table].singlecount_name = tableName.ToLower();

                        }
                        else
                        {
                            metadata.Tables[table].multicount_name = tableName.ToLower() + "s";
                            metadata.Tables[table].singlecount_name = tableName.ToLower();
                        }
                    }
                }
                metadata.Tables[table].columns[column] = new ColumnMetaData();
                metadata.Tables[table].columns[column].name = column;
                metadata.Tables[table].columns[column].DataType = type;
                metadata.Tables[table].columns[column].nullable = (nullable == "1") ? true : false;
                metadata.Tables[table].columns[column].name = description;
            }


            //определение внешних ключей по правилам наименования
            List<TableMetaData> tables = (from table in metadata.Tables.Values select table).ToList<TableMetaData>();
            foreach (var ptable in metadata.Tables)
            {

                HashSet<string> associations = new HashSet<string>() { ptable.Value.multicount_name, ptable.Value.singlecount_name };
                foreach (var pcolumn in ptable.Value.columns)
                {
                    //дополнительный анализ наименований колоной
                    string[] ids = pcolumn.Key.ToLower().Split("_");
                    HashSet<string> idsSet = new HashSet<string>(ids);
                    List<string> lids = (from id in idsSet select id.ToLower()).ToList<string>();
                    if (idsSet.Contains("id"))
                    {
                        int count = (from s in idsSet where associations.Contains(s) select s).Count();
                        if (count == 0)
                        {
                            /*TableMetaData foreignKeyTable = ( from table in tables where lids.Contains( table.singlecount_name ) || lids.Contains( table.multicount_name ) select table ).SingleOrDefault<TableMetaData>();
                            if( foreignKeyTable == null )
                            {
                                //throw new Exception("внешний ключ не найден для поля "+ ptable.Key+"."+pcolumn.Key );
                            }
                            else
                            {
                                //ptable.Value.fk[pcolumn.Key] = foreignKeyTable;
                            }*/
                        }
                        else
                        {
                            pcolumn.Value.IsPrimary = true;
                            ptable.Value.PrimaryKey = metadata.Tables[ptable.Key].PrimaryKey = pcolumn.Key;

                        }
                    }
                }
            }


            return metadata;
        }
    }
    public JArray GetJsonResult(string sql)
    {
        this.Info($"GetJsonResult({sql})");
        DataTable ResultDataTable = this.ExecuteQuery(sql);
        JArray JResult = DataTableService.GetJArray(ResultDataTable);
        return JResult;
    }
    /// <summary>
    /// Вспомогательный метод преобразования данных в JSON
    /// </summary>
    public JArray convert(DataTable dataTable)
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

    /// <summary>
    /// Получение списка таблиц
    /// </summary>
    public IEnumerable<string> GetTables()
    {
        List<string> tableNames = new List<string>();
        using (System.Data.Odbc.OdbcConnection connection = GetConnection())
        {
            connection.Open();
            DataTable tables = connection.GetSchema("Tables");
            foreach (JObject next in this.convert(tables))
            {
                string tableName = next["TABLE_NAME"].Value<string>();
                if (tableName.StartsWith("__") == false)
                {
                    tableNames.Add(tableName);
                }
            }
        }
        return tableNames.ToArray();
    }


    public JObject GetSingleJObject(string sql)
    {
        this.Info($"GetSingleJObject({sql})");
        DataTable ResultDataTable = this.ExecuteQuery(sql);
        JArray JResult = DataTableService.GetJArray(ResultDataTable);
        JToken token = JResult.FirstOrDefault();
        return token != null ? (JObject)token : null;
    }



    /// <summary>
    /// Выполнение простой команды (без результирующего набора)
    /// </summary>
    public int PrepareQuery(string tsql)
    {
        this.Info($"PrepareQuery({tsql})");
        try
        {
            OdbcCommand command = new OdbcCommand(tsql, GetConnection());
            int result = command.ExecuteNonQuery();
            return result;

        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось выполнить команду " + tsql + " \n" + ex.Message);
        }
    }

    public IEnumerable<TEntity> ExecuteQuery<TEntity>(string command) where TEntity : class
    {
        return DataTableService.GetResultSet<TEntity>(ExecuteQuery(command));
    }

    public int TryPrepareQuery(string command)
    {
        try
        {
            return PrepareQuery(command);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return 0;
        }
    }

    public IEnumerable<dynamic> ExecuteQuery(string command, Type entity)
    {
        return DataTableService.GetResultSet(ExecuteQuery(command), entity);
    }
    /// <summary>
    /// Выполнение запроса с 1 результирующим набором
    /// </summary>
    public DataTable ExecuteQuery(string tsql)
    {
        this.Info($"ExecuteQuery({tsql})");
        DataTable dataTable = new DataTable();
        this.Info($"{tsql}=>{dataTable.Rows.Count}");
        OdbcCommand command = new OdbcCommand(tsql, GetConnection());
        using (OdbcDataAdapter adapter = new OdbcDataAdapter(tsql, GetConnection()))
        {
            adapter.Fill(dataTable);
        }
        return dataTable;
    }

    public int ExecuteProcedure(string command, IDictionary<string, string> input, IDictionary<string, string> output)
    {
        throw new NotImplementedException();
    }
}