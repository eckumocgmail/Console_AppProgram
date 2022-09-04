using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public interface IWebApi
    {
        public HashSet<IEntityFasade > Services { get; set; }
    }
}
