using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class EntityLocalBook<TEntity> : IEntityBook<TEntity> where TEntity : class
{

    public IEnumerable<TEntity> dataset { get; set; }  

    public EntityLocalBook()
    {
        dataset = new HashSet<TEntity>();
    }

    public Task<IEnumerable<TEntity>> GetPage(int page, int size)
    {
        return Task.Run<IEnumerable<TEntity>>(() =>
        {
            return dataset.ToList().Skip((page - 1) * size).Take(size).ToArray(); 
        });
    }

    public Task<int> GetPagesCount(int size)
    {
        return Task.Run<int>(()=> {
            if (dataset.Count() % size > 0)
            {
                return (int)(Math.Floor((decimal)(dataset.Count() / size)) + 1);
            }
            else
            {
                return (int)(Math.Floor((decimal)(dataset.Count() / size)));
            }
        });
    }

    public Task<IEnumerable<TEntity>> SetPage(int page, int size)
    {
        return Task.Run<IEnumerable<TEntity>>(()=> {
            return dataset.ToList().Skip((page - 1) * size).Take(size).ToArray();
        });
    }

    public int GetPage()
    {
        throw new NotImplementedException();
    }

    public int GetPagesCount()
    {
        throw new NotImplementedException();
    }

    void IEntityBook<TEntity>.SetPage(int Page, int Size)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TEntity> GetItemOnPage()
    {
        throw new NotImplementedException();
    }
}