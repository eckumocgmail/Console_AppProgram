using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataADO;

namespace DataADO
{
    /// <summary>
    /// Сервис выполнения sql-запросов 
    /// </summary>
    public class MySqlDbConnector : MySqlConnectionString,
        IDbConnector<MySqlConnection>,
        IDisposable
    {
        
        /// <summary>
        /// 
        /// </summary>
        private MySqlConnection _Connection;


        public MySqlDbConnector() : base()
        {
            this.Info("Create");
        }

        public MySqlConnection GetConnection()
        {
            if( _Connection == null )
            {
                _Connection = this.CreateAndOpenConnection();
            }
            return _Connection;
        }
            

  
        public override void Dispose()
        {
            base.Dispose();
            if( _Connection != null )
            {
                _Connection.Close();
            }
        }

      

        public MySqlConnection CreateAndOpenConnection()
        {
            var connection = new MySqlConnection(base.ToString());

            connection.StateChange += OnStateChanged;
            connection.Open();
            return connection;
        }

        private void OnStateChanged(object sender, StateChangeEventArgs evt)
        {
            this.Info($"{evt.OriginalState}=>{evt.CurrentState} {this}");            
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
    }
}

