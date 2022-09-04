
using DataADO;




using DataCommon.DatabaseMetadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

internal class EntityFasade: IEntityFasade
{ 
    private ISqlExecutor _api;
    private Type _entity;
    private TableMetaData _metadata;

    public EntityFasade(ISqlExecutor api, TableMetaData metadata, Type entity )
    {
        _api = api;
        _entity = entity;
        _metadata = metadata;
    }
    public int Count( ) 
    {
        return _api.GetSingleJObject($"SELECT COUNT(*) AS CNT FROM [{_metadata.TableSchema}].[{_metadata.TableName}]")["CNT"].ToString().ToInt();
    }

    public Task<TResultSet> Find<TResultSet>(int id) where TResultSet : class
    {
        return Task.Run(()=> {
            return _api.ExecuteQuery<TResultSet>($"SELECT * FROM [{_metadata.TableSchema}].[{_metadata.TableName}] WHERE {_metadata.PrimaryKey}={id}").FirstOrDefault();
        });
    }

    public Task<int> Delete(int id)
    {
        return Task.Run(() => {
            return _api.PrepareQuery($"DELETE FROM [{_metadata.TableSchema}].[{_metadata.TableName}] WHERE {_metadata.PrimaryKey}={id}");
        });
    }

    public Task<int> Update(object model)
    {
        return Task.Run(() => {
            string Settings = $"";
            
            foreach (string key in _metadata.columns.Keys)
            {
      
                string Constant = GetValueContant(GetValue(model,key), _metadata.columns[key]);
                Settings += $"{key}=Constant \n";

            }                        
            return _api.PrepareQuery($"UPDATE [{_metadata.TableSchema}].[{_metadata.TableName}]\n SET \n{Settings} WHERE {_metadata.PrimaryKey}={GetValue(model,_metadata.PrimaryKey)}");
        });
    }

    private object GetValue(object model, string key)
    {
        
        return ReflectionService.GetValueFor(model, key);
    }

    public Task<int> Create(object model)
    {
        return Task.Run(() => {
            string Values = $"(";
            string Columns = $"(";
            foreach(string key in _metadata.columns.Keys)
            {
                if (IsUserInput(_metadata.columns[key]))
                {
                    Columns += $"[{key.ToTSQLStyle()}],";
                    string Constant = GetValueContant(GetValue(model, key), _metadata.columns[key]);
                    Values += $"{Constant},";

                }

            }
            if (Columns.EndsWith(","))
            {
                Columns = Columns.Substring(0, Columns.Length - 1);
                Values = Values.Substring(0, Values.Length - 1);
            }
            Values += ")";
            Columns += ")";

            return _api.PrepareQuery($"INSERT INTO [{_metadata.TableSchema}].[{_metadata.TableName}] {Columns} VALUES {Values}");
        });
    }

    private bool IsUserInput(ColumnMetaData columnMetadata)
    {
        return columnMetadata.ColumnName != "ID";
    }

    /// <summary>
    /// Используется для передачи данных в SQL
    /// </summary>
    private string GetValueContant(object value, ColumnMetaData metadata)
    {
        switch(metadata.DataType.ToLower())
        {
            case "date":
            case "datetime":
                
                return ToDateString(value);
            

            //case "datetime2":
          //      break;
            case "ntext":
            case "text":

            case "nvarchar":
            case "nvarchar(max)":
            case "varchar":
                return value!=null?"'" + value   + "'": "";
                
            case "smallint":
            case "int":
            case "float":
                return "" + value.ToString().Replace(",", ".") + "";
            default: throw new NotImplementedException("Тип данных "+ metadata.DataType + " не поддерживается ");  
        }
    }

    private string ToDateString(object value)
    {
        string text = "'"+((DateTime)value).ToString("yyyy-MM-dd")+"'";
        return text;
    }

    public Task<object[]> List()
    {
        return Task.Run(() => {
            return _api.ExecuteQuery($"SELECT * FROM [{_metadata.TableSchema}].[{_metadata.TableName}]",_entity).ToArray();
        });
    }

    public Task<object[]> Page(int page, int size)
    {
        throw new NotImplementedException();
    }

    public Task<object[]> Page(int page, int size, params string[] sorting)
    {
        throw new NotImplementedException();
    }

    public string GetEntityName()
    {
        return _metadata.TableName;
    }

    public Task<object> CreateNew()
    {
        return Task.Run(()=> {
            var record = _entity.New();
            Create(record).Wait();
            return record;
        });
    }
}