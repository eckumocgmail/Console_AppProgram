using System;
using System.Collections.Generic;
using System.Linq;

namespace DataADO
{
    public class PostgresDbModel : PostgresDbMetadata, IDbModel
    {
        public ISet<Type> EntityTypes { get; set; } = new HashSet<Type>();
        public Type[] GetEntityClasses() => EntityTypes.ToArray();
        public void AddEntityType(Type entity) => EntityTypes.Add(entity);

    }
}
