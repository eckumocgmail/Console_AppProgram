using DataADO;

using Npgsql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class PostgresDbConnector : PostgresConnectionString,
        IDbConnector<NpgsqlConnection>,
        IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private NpgsqlConnection _Connection;

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


        public PostgresDbConnector()
        {
        }

        public PostgresDbConnector(string dataSource, int port, string database, string userID, string password) : base(dataSource, port, database, userID, password)
        {
        }


        public NpgsqlConnection GetConnection()
        {
            if (_Connection == null)
            {
                _Connection = this.CreateAndOpenConnection();
            }
            return _Connection;
        }



        public override void Dispose()
        {
            if (_Connection != null)
            {
                _Connection.Close();
            }
        }



        public NpgsqlConnection CreateAndOpenConnection()
        {
            var connection = new NpgsqlConnection(base.ToString());

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
