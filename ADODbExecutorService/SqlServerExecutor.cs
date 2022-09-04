using DataCommon;

using System.Data.SqlClient;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class SqlServerExecutor : SqlServerDbConnector, ISqlExecutor

    {
        protected readonly IDataTableService DataTableService = new DataTableService();

        public SqlServerExecutor()
        {
        }

        public SqlServerExecutor(string server, string database) : base(server, database)
        {
        }

        public SqlServerExecutor(string server, string database, bool trustedConnection, string userID, string password) : base(server, database, trustedConnection, userID, password)
        {
        }

        public int ExecuteProcedure(string name, IDictionary<string, string> input, IDictionary<string, string> output)
        {
            this.Info($"ExecuteProcedure({name})");
            SqlCommand command = new SqlCommand(name, GetConnection());

            int result = command.ExecuteNonQuery();
            return result;
        }

        public JArray GetJsonResult(string sql)
        {
            this.Info($"GetJsonResult({sql})");
            DataTable ResultDataTable = this.ExecuteQuery(sql);
            JArray JResult = DataTableService.GetJArray(ResultDataTable);
            return JResult;
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
        /// Выполнение запроса с 1 результирующим набором
        /// </summary>
        public DataTable ExecuteQuery(string tsql)
        {
            this.Info($"ExecuteQuery({tsql})");
            DataTable dataTable = new DataTable();
            this.Info($"{tsql}=>{dataTable.Rows.Count}");
              SqlCommand command = new SqlCommand(tsql, GetConnection());
            using (SqlDataAdapter adapter = new SqlDataAdapter(tsql, GetConnection()))
            {
                adapter.Fill(dataTable);
            }                
            return dataTable;            
        }

        /// <summary>
        /// Выполнение простой команды (без результирующего набора)
        /// </summary>
        public int PrepareQuery(string tsql)
        {
            this.Info($"PrepareQuery({tsql})");
            try
            {
                SqlCommand command = new SqlCommand(tsql, GetConnection());
                int result = command.ExecuteNonQuery();
                return result;

            }
            catch(Exception ex)
            {
                throw new Exception($"Не удалось выполнить команду "+tsql+" \n"+ex.Message);
            }
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string command) where TEntity: class
        {
            return DataTableService.GetResultSet<TEntity>(ExecuteQuery(command));
        }

        public int TryPrepareQuery(string command)
        {
            try
            {
                return PrepareQuery(command);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public IEnumerable<dynamic> ExecuteQuery(string command, Type entity)
        {
            return DataTableService.GetResultSet(ExecuteQuery(command), entity);
        }
    }
}
