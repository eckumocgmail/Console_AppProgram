using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

using DataADO;

using DataCommon.DatabaseMetadata;

public class ODBCDbMetaData:  IDbMetadata
{

    public IDictionary<string, TableMetaData> Tables { get; set; } =
        new ConcurrentDictionary<string, TableMetaData>();

    /// <summary>
    /// Внешние ключи
    /// Ключ = [TableName].[ColumnName] 
    /// Значение = [TableName].[ColumnName] 
    /// </summary>
    private IDictionary<string, string> ForeignKeys { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Параметры хранимых процедур с доступом по схемам
    /// </summary>
    private IDictionary<string, IDictionary<string, ProcedureMetadata>> ProceduresMetadata { get; set; } =
        new Dictionary<string, IDictionary<string, ProcedureMetadata>>();
    public string Driver { get; internal set; }
    public string Database { get; internal set; }
    public string ServerVersion { get; internal set; }
    public string ConnectionString { get; internal set; }

    public IEnumerable<string> GetTableNames() => Tables.Keys;

    public IDictionary<string, TableMetaData> GetTablesMetadata()
        => Tables;

    public IDictionary<string, ColumnMetaData> GetColumnsMetadata(string TableSchema, string TableName)
        => Tables.ContainsKey(TableName) ? Tables[TableName].columns: null;

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
