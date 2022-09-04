using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
 
    /// <summary>
    /// Сервис выполнения sql-запросов 
    /// </summary>
    public class SqlServerDbConnector : SqlServerConnectionString,
        IDbConnector<SqlConnection>, IDisposable
    {
        private SqlConnection _Connection;

        public SqlServerDbConnector() : this($"AGENT\\KILLER", "model")
        {
        }

        public SqlServerDbConnector(string server, string database) : this(server, database, true)
        {
        }

        public SqlServerDbConnector(string server, string database, bool intergratedSecurity, string userID="", string password="") : base(server, database, intergratedSecurity, false, userID, password)
        {
        }

        public SqlConnection GetConnection()
        {
            this.Info($"GetConnection({ToString()})");
            if (_Connection == null)
            {
                _Connection = this.CreateAndOpenConnection();
            }
            return _Connection;
        }

        public bool CanConnect()
        {
            try
            {
                using (var connection = CreateAndOpenConnection())
                {
                    return connection.State == ConnectionState.Open;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void Dispose()
        {
           
            this.Info("Dispose()");
            if (_Connection != null)
            {
                _Connection.Close();
            }
        }


        public SqlConnection CreateAndOpenConnection()
        {

         
            this.Info($"CreateAndOpenConnection({base.ToString()})");
            var connection = new SqlConnection(base.ToString());
            connection.StateChange += OnStateChanged;
            connection.Open();
            return connection;
        }


        private void OnStateChanged(object sender, StateChangeEventArgs evt)
        {
            this.Info($"{evt.OriginalState}=>{evt.CurrentState} {this}");
        }
 
    }
}
