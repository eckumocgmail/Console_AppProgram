
using Newtonsoft.Json.Linq;
using System;

using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using DataADO;
using System.Data.SqlClient;
using DataCommon.DatabaseMetadata;

public class ODBCDbConnector : ODBCConnectionString, IDbConnector<OdbcConnection>
{

    public ODBCDbConnector(string name, string username, string password) : base(name, username, password) { }
    /// <summary>
    /// Установка соединения 
    /// </summary>
    public virtual System.Data.Odbc.OdbcConnection CreateAndOpenConnection()
    {

        System.Data.Odbc.OdbcConnection connection = null;
        try
        {
            this.Info(this);
            connection = new System.Data.Odbc.OdbcConnection(this.ToString());
            connection.Open();
            connection.InfoMessage += (sender, args) => {
                System.Console.WriteLine("From ODBC Driver: " + sender + " " + args);
            };
            connection.StateChange += (sender, args) => {
                System.Console.WriteLine("ODBC state changed: " + sender + " " + args);
            };
        }
        catch (Exception ex)
        {
            this.Error("При попытки установить соединение ODBC: " + this.ToString() + " возникла неожиданныя ситуация", ex);
        }
        return connection;
    }

    public OdbcConnection GetConnection()
    {
        System.Data.Odbc.OdbcConnection connection = null;
        try
        {
            this.Info(this);
            connection = new System.Data.Odbc.OdbcConnection(this.ToString());
            
            connection.InfoMessage += (sender, args) => {
                System.Console.WriteLine("From ODBC Driver: " + sender + " " + args);
            };
            connection.StateChange += (sender, args) => {
                System.Console.WriteLine("ODBC state changed: " + sender + " " + args);
            };
        }
        catch (Exception ex)
        {
            this.Error("При попытки установить соединение ODBC: " + this.ToString() + " возникла неожиданныя ситуация", ex);
        }
        return connection;
    }

    public bool CanConnect()
    {
        throw new NotImplementedException();
    }
}
/**
 * 
    //System.Data.Odbc   @"Driver={MySQL ODBC 5.3 ANSI Driver};DATA SOURCE=mysql_app;Uid=root;Pwd=root;"
    //System.Data.Odbc   @"Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;Uid=Admin;Pwd=;"
    //System.Data.Odbc   @"DRIVER={SQL SERVER};SERVER=(LocalDB)\\v11.0;AttachDbFileName=G:\projects\eckumoc\AppData\persistance.mdf;"   "

public IDbMetadata metadata { get; set; }             

        public ResultSet CleverExecute(string expression)
        {
         
            using (System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection())
            {

                connection.Open();
                DataTable dataTable = new DataTable();
                OdbcDataAdapter adapter = new OdbcDataAdapter(expression, connection);
                adapter.Fill(dataTable);

                TableMetaData tmd = new TableMetaData();
                foreach (DataColumn column in dataTable.Columns)
                {
                    ColumnMetaData cmd = new ColumnMetaData()
                    {
                        IsNullable = column.AllowDBNull,
                        IsUnique = column.Unique,
                        ColumnCaption = column.Caption,
                        DataType = column.DataType.Name,
                    };
                    tmd.TableColumns.Add(column.ColumnName, cmd);
                    
                }

                var rs = this.convert(dataTable);
                return new ResultSet()
                {

                    MetaData = tmd,
                    DataTable = dataTable,
                    DataSet = rs
                };
            }
        }


         
        

        /// <summary>
        /// Считывание бинарных данных, получаемых запросом
        /// </summary>
        public byte[] ReadBlob( string sqlCommand )
        {
            using ( System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection() )
            {
                connection.ChangeDatabase("FRMO");
                connection.Open();
                OdbcCommand command = new OdbcCommand( sqlCommand, connection );
                OdbcDataReader reader = command.ExecuteReader();
                if ( reader.Read() )
                {
                    // matching record found, read first column as string instance
                    byte[] value = ( byte[] ) reader.GetValue( 0 );
                    reader.Close();
                    command.ExecuteNonQuery();
                    return value;
                }
                return null;
            }
        }


        /// <summary>
        /// Запись бинарных данных в базу
        /// </summary>
        public int InsertBlob( string sqlCommand, string blobColumn, byte[] data )
        {
            using ( System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection() )
            {
                connection.Open();
                OdbcCommand command = new OdbcCommand( sqlCommand, connection );
                command.Parameters.Add( blobColumn, OdbcType.Binary );
                command.Parameters[blobColumn].Value = data;
                return command.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Получение расширенной справочной информации
        /// </summary>
        public Dictionary<string, object> GetSchemaDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            using (System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection())
            {
                connection.Open();
                DataTable catalogs = connection.GetSchema();
                JArray jcatalogs = this.convert(catalogs);
                foreach (JObject catalogInfo in jcatalogs)
                {
                    string collectionName = catalogInfo["CollectionName"].Value<string>();
                    if(collectionName == "Indexes")
                    {
                        Dictionary<string, object> indexes = new Dictionary<string, object>();
                        foreach ( string table in GetTables())
                        {
                            JArray catalog = this.convert(connection.GetSchema(collectionName,new string[]{ null,null,table }));
                            indexes[table] = catalog;
                        }
                        result[collectionName] = indexes;
                    }
                    else
                    {                  
                        if(collectionName != "DataTypes")
                        {
                            JArray catalog = this.convert(connection.GetSchema(collectionName));
                            result[collectionName] = catalog;
                        }                        
                    }                                              
                }
                result["catalogs"] = jcatalogs;
            }
            return result;
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
            using (System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection())
            {
                connection.Open();
                DataTable tables = connection.GetSchema("Tables");
                foreach(JObject next in this.convert(tables))
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


        /// <summary>
        /// Запрос параметров хранимых процедур
        /// </summary>
        /// <returns></returns>
        private Dictionary< string, ProcedureMetadata > GetStoredProceduresMetadata()
        {
            Dictionary<string, ProcedureMetadata> metadata = new Dictionary<string, ProcedureMetadata>();
            // TODO:
            return metadata;
        }



        




        /// <summary>
        /// Выполнение запроса, возвращающего одну запись.
        /// </summary>
        public JObject GetSingleJObject( string sql )
        {
            try
            {
                using (System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection())
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, connection);
                    adapter.Fill(dataTable);
                    JArray rs = this.convert(dataTable);
                    foreach (JObject next in rs)
                    {
                        return next;
                    }
                    throw new Exception("Запрос не вернул данные " + sql);
                }
            }catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.{nameof(GetSingleJObject)}({sql}) => {ex.Message}");
                return null;
            }
        }



        JArray APIDataSource.GetJsonResult(string sql)
        {
            return ((OdbcDataSource)this).Execute(sql);
        }

        public DataTable CreateDataTable(string sql)
        {
            System.Console.WriteLine(sql);
            using (System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection())
            {
                try
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, connection);
                    adapter.Fill(dataTable);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    ex.ToString().WriteToConsole();
                    throw;
                }
            }
        }

        /// <summary>
        /// Выполнение запроса 
        /// </summary>
        public DataTable ExecuteDT( string sql )
        
        {
            System.Console.WriteLine(sql);
            using ( System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection() )
            {
                try
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, connection);
                    adapter.Fill(dataTable);

                    return dataTable;
                }
                catch(Exception ex)
                {
                    this.Error(ex);
                    throw;
                }
                
            }
        }
        /// <summary>
        /// Выполнение запроса 
        /// </summary>
        public JArray Execute(string sql)

        {
            System.Console.WriteLine(sql);
            using (System.Data.Odbc.OdbcConnection connection = CreateAndOpenConnection())
            {
                try
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, connection);
                    adapter.Fill(dataTable);

                    TableMetaData tmd = new TableMetaData();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        ColumnMetaData cmd = new ColumnMetaData()
                        {
                            IsNullable = column.AllowDBNull,
                            IsUnique = column.Unique,
                            ColumnCaption = column.Caption,
                            DataType = column.DataType.Name,
                        };
                        tmd.TableColumns.Add(column.ColumnName, cmd);
                    }
                    var array = this.convert(dataTable);
                    System.Console.WriteLine(array);
                    return array;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ошибка при выполнении запроса: "+sql + " " + ex.Message);
                    throw;                    
                }

            }
        }


        public string GetConenctionString()
        {
            return this.connectionString+ "Driver={SQL Server};";
        }

        public bool canConnect()
        {
            return GetTables() != null;
        }

        public bool canReadAndWrite()
        {
            //TODO:
            return true;
        }

        public bool canCreateAlterTables()
        {
            //TODO:
            return true;
        }

        public object SingleSelect(string sql)
        {
            throw new NotImplementedException();
        }

        public object MultiSelect(string sql)
        {
            throw new NotImplementedException();
        }

        public object Exec(string sql)
        {
            throw new NotImplementedException();
        }

        public object Prepare(string sql)
        {
            Console.WriteLine(sql);
            try
            {
                using (var connection = CreateAndOpenConnection())
                {
                    connection.Open();
                    System.Data.Odbc.OdbcCommand command = new System.Data.Odbc.OdbcCommand(sql, connection);
                    int result = command.ExecuteNonQuery();
                    return result;
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Исключение при выполнении операции Prepare({sql})", ex);
            }
        }

    }    //System.Data.OleDb  @"Provider=Microsoft.Jet.OLEDB.12.0;Data Source=a:\\master.mdb;";
 */