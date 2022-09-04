using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataCommon
{
    public interface IDataTableService
    {
        public IEnumerable<string> GetColumnsNames(DataTable dataTable);
        public IDictionary<string, string> GetColumnsCaptions(DataTable dataTable);
        public IDictionary<string, Type> GetDataTypes(DataTable dataTable);
        public IEnumerable<IDictionary<string, string>> GetTextData(DataTable dataTable);
        public IEnumerable<string> GetTextColumn(DataTable dataTable, string columnName);
        public IEnumerable<IDictionary<string, object>> GetRowsData(DataTable dataTable);
        public IEnumerable<TRecord> GetResultSet<TRecord>(DataTable dataTable) where TRecord: class;
        public JArray GetJArray(DataTable dataTable);
        public IEnumerable<dynamic> GetResultSet(DataTable dataTable, Type entity);
    }
}
