using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class SqlServerDbModel: SqlServerDbMetadata, IDbModel
    {
        public SqlServerDbModel(string server, string database) : base(server, database)
        {
        }

        public SqlServerDbModel()
        {
        }

        public SqlServerDbModel(string server, string database, bool trustedConnection, string userID, string password) : base(server, database, trustedConnection, userID, password)
        {
        }

        public ISet<Type> EntityTypes { get; set; } = new HashSet<Type>();
        public Type[] GetEntityClasses() => EntityTypes.ToArray();
        public void AddEntityType(Type entity) => EntityTypes.Add(entity);     
        

    }
}
