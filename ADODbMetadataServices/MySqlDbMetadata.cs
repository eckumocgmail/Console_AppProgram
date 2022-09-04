using DataCommon.DatabaseMetadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class MySqlDbMetadata: MySqlExecutor,IDbMetadata
    {
        public MySqlDbMetadata()
        {
        }

        public IEnumerable<string> GetTableNames()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, TableMetaData> GetTablesMetadata()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, ColumnMetaData> GetColumnsMetadata(string TableSchema, string TableName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyMetadata> GetKeysMetadata()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, ProcedureMetadata> GetProceduresMetadata(string Schema)
        {
            throw new NotImplementedException();
        }

        public ProcedureMetadata GetProcedureMetadata(string SchemaName, string ProcedureName)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, ParameterMetadata> GetParametersMetadata(string SchemaName, string ProcedureName)
        {
            throw new NotImplementedException();
        }
    }
}
