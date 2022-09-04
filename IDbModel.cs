using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public interface IDbModel
    {
        public ISet<Type> EntityTypes { get; set; }
        public Type[] GetEntityClasses();
        public void AddEntityType(Type entity);
    }
}
