using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public interface ISqlExecutor
    {
        public JArray GetJsonResult(string sql);
        public 
            JObject GetSingleJObject(string command);
        public DataTable ExecuteQuery(string command);
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string command) where TEntity : class;
        public IEnumerable<dynamic> ExecuteQuery(string command, Type entity);
        public int TryPrepareQuery(string command);
        public int PrepareQuery(string command);
        public int ExecuteProcedure(string command, IDictionary<string,string> input, IDictionary<string, string> output);
    }
}
