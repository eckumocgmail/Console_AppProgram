using DataCommon.DatabaseMetadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatReports.BusinessResources.DataCommon.Metadata
{
    public class ProcedureMetadataService
    {

        /// <summary>
        /// IN parameters
        /// </summary>
        public IDictionary<string, ParameterMetadata> GetInParameters(ProcedureMetadata Procedure)
        {
            IDictionary<string, ParameterMetadata> InputParameters = new Dictionary<string, ParameterMetadata>();
            foreach (var ParameterMetadata in Procedure.ParametersMetadata.Values)
            {
                if (ParameterMetadata.ParameterMode == "IN")
                {
                    InputParameters[ParameterMetadata.ParameterName] = ParameterMetadata;
                }
            }
            return InputParameters;
        }


        /// <summary>
        /// OUT parameters
        /// </summary>
        /// <param name="Procedure"></param>
        /// <returns></returns>
        public IDictionary<string, ParameterMetadata> GetOutParameters(ProcedureMetadata Procedure)
        {
            IDictionary<string, ParameterMetadata> InputParameters = new Dictionary<string, ParameterMetadata>();
            foreach (var ParameterMetadata in Procedure.ParametersMetadata.Values)
            {
                if (ParameterMetadata.ParameterMode == "OUT")
                {
                    InputParameters[ParameterMetadata.ParameterName] = ParameterMetadata;
                }
            }
            return InputParameters;
        }

        /// <summary>
        /// INOUT parameters
        /// </summary>
        /// <param name="Procedure"></param>
        /// <returns></returns>
        public IDictionary<string, ParameterMetadata> GetInOutParameters(ProcedureMetadata Procedure)
        {
            IDictionary<string, ParameterMetadata> InputParameters = new Dictionary<string, ParameterMetadata>();
            foreach (var ParameterMetadata in Procedure.ParametersMetadata.Values)
            {
                if (ParameterMetadata.ParameterMode == "INOUT")
                {
                    InputParameters[ParameterMetadata.ParameterName] = ParameterMetadata;
                }
            }
            return InputParameters;
        }
    }
}
