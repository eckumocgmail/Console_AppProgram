using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class MySqlExecutor : MySqlDbConnector, ISqlExecutor
    {
        public MySqlExecutor()
        {
        }

        public JArray GetJsonResult(string sql)
        {
            throw new NotImplementedException();
        }

        public JObject GetSingleJObject(string command)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteQuery(string command)
        {
            throw new NotImplementedException();
        }

        public int PrepareQuery(string command)
        {
            throw new NotImplementedException();
        }

        public int ExecuteProcedure(string command, IDictionary<string, string> input, IDictionary<string, string> output)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string command) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public int TryPrepareQuery(string command)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> ExecuteQuery(string command, Type entity)
        {
            throw new NotImplementedException();
        }
    }
}
