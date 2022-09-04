 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
public interface IEntityFasade  
{
    int Count( );
    public string GetEntityName();
    public Task<TResultSet> Find<TResultSet>(int id) where TResultSet : class;
    public Task<int> Delete(int id);
    public Task<int> Update(object model);
    public Task<int> Create(object model);
    public Task<object[]> List();

    public Task<object[]> Page(int page, int size);
    public Task<object[]> Page(int page, int size, params string[] sorting);
    public Task<object> CreateNew();
}
