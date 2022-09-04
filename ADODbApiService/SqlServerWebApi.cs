using DataADO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class SqlServerWebApi: SqlServerMigBuilder, IWebApi
    {
        public HashSet<IEntityFasade > Services { get; set; }


        public SqlServerWebApi() : base(@"(localdb)\MSSQLLocalDB", "www",true, "sa", "Gye*34FRtw")
        {
            Services = new HashSet<IEntityFasade >();
            
        }

        public SqlServerWebApi(string server, string database) : base(server, database)
        {
            Services = new HashSet<IEntityFasade >();
        }

        public SqlServerWebApi(string server, string database, bool trustedConnection, string userID, string password) : base(server, database, trustedConnection, userID, password)
        {
            Services = new HashSet<IEntityFasade >();
        }


    }
}
