using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCommon.DatabaseMetadata
{
    public class ParameterMetadata
    {
        public string SpecificCatalog { get; set; }
        public string SpecificSchema { get; set; }

        /// <summary>
        /// Имя процедуры
        /// </summary>
        public string SpecificName { get; set; }
        public string CharacterSetName { get; set; }
        public string ParameterName { get; set; }
        public string DataType { get; set; }
        public string ParameterMode { get; set; }
        public int OrdinalPosition { get; set; }
    }
}
