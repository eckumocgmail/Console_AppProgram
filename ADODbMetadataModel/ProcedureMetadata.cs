using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCommon.DatabaseMetadata
{
    public class ProcedureMetadata
    {
        /// <summary>
        /// Квалификатор процедуры идентичен имени базы данных
        /// </summary>
        public string ProcedureQualifier { get; set; }

        /// <summary>
        /// Схема данных
        /// </summary>
        public string ProcedureOwner { get; set; }

        /// <summary>
        /// Имя процедуры
        /// </summary>        
        public string ProcedureName { get; set; }

        /// <summary>
        /// Полное наименование процедуры
        /// </summary>
        public string FullName { get => $"[{ProcedureQualifier}].[{ProcedureOwner}].[{ProcedureName}]"; }


        /// <summary>
        /// Сведения о параметрах
        /// </summary>
        public IDictionary<string, ParameterMetadata> ParametersMetadata { get; set; } = new Dictionary<string, ParameterMetadata>();



               
    }
}
