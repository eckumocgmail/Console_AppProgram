using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public interface IDbConnector<TDBConnection> where TDBConnection : DbConnection
    {
        public TDBConnection CreateAndOpenConnection();
        public TDBConnection GetConnection();
        public bool CanConnect();
    }
}


