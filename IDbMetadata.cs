using DataCommon.DatabaseMetadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public interface IDbMetadata
    {
        public IEnumerable<string> GetTableNames();
        public IDictionary<string, TableMetaData> GetTablesMetadata();
        public IDictionary<string, ColumnMetaData> GetColumnsMetadata(string TableSchema, string TableName);
        public IEnumerable<KeyMetadata> GetKeysMetadata();
        public IDictionary<string, ProcedureMetadata> GetProceduresMetadata(string Schema);
        public ProcedureMetadata GetProcedureMetadata(string SchemaName, string ProcedureName);        
        public IDictionary<string, ParameterMetadata> GetParametersMetadata(string SchemaName, string ProcedureName);
    }
}
