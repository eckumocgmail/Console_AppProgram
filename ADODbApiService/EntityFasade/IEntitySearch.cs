using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IEntitySearch<TEntity>
{

    public Task<Dictionary<string, string>> Lastest(int size);
    public Task<Dictionary<string, string>> GetMostPopulare(int size);
    public Task<Dictionary<string, string>> GetKeywords(string query);


    public List<string> GetIndexes();

    public IQueryable<TEntity> Search(string query);
    public Task<TEntity[]> Search(string query, int page, int size);
    public Task<TEntity[]> Search(string query, int page, int size, params string[] sorting);

}