
using DataCommon.DatabaseMetadata;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;



 





public class ResultSet
{
    public DateTime Updated { get; set; }
    public string Expression { get; set; }
    public TableMetaData MetaData { get; set; }
    public DataTable DataTable { get; set; }
    public JArray DataSet { get; set; }
}