using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

/// <summary>
/// Выполнение операция Create,Read,Update,Delete
/// </summary>
public abstract class EntityRepository<TEntity> :
                IEntityRepository<TEntity> where TEntity: class
{
    protected DbContext _context { get; set; }





    public void LogInformation(string message)
    {
        System.Console.WriteLine($"[{DateTime.Now}][{GetType().GetTypeName()}]: {message}");
    }

 

    public EntityRepository(DbContext context)
    {
        _context = context;
    }

    public abstract DbSet<TEntity> GetDbSet(DbContext context) ;

    public int Post(TEntity data)
    {
        var dbset = this.GetDbSet(_context);
        dbset.Add(data);
        int result = _context.SaveChanges();
        return result;
    }

    public int Put(TEntity data)
    {
        var dbset = this.GetDbSet(_context);
        dbset.Update(data);
        int result = _context.SaveChanges();
        return result;
    }

    public int Patch(params TEntity[] dataset)
    {
        int result = 0;
        var dbset = this.GetDbSet(_context);
        foreach (TEntity next in dbset.ToArray())
        {
            result += Delete((int)next.GetProperty("ID"));
        }
        foreach (TEntity next in dataset)
        {
            result += Put(next);
        }
        return result;
    }

    public int Delete(int id)
    {
        var dbset = this.GetDbSet(_context);
        IEnumerable<TEntity> records = Get(id);
        if (records.Count() == 0)
        {
            return 0;
        }
        else
        {
            dbset.Remove(records.FirstOrDefault());
            int result = _context.SaveChanges();
            return result;
        }
    }

    public IEnumerable<TEntity> Get(int? id)
    {
        if (id == null)
        {
            return this.GetDbSet(_context).ToArray();
        }
        else
        {
            return this.GetDbSet(_context).Where(record => (int)record.GetProperty("ID") == id).ToArray();
        }
    }
    public async Task Create(TEntity target)
    {
        this.LogInformation($"Create({target})");
        _context.Add(target);
        await _context.SaveChangesAsync();
    }


    public async Task Delete(TEntity p)
    {
        this.LogInformation($"Delete({p.ToJson()})");

        _context.Remove(p);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<TEntity>> Get( )
    {
        this.LogInformation($"Get( )");
        return Task.Run(() =>
        {
            return (IEnumerable<TEntity>)(_context.GetDbSet(typeof(TEntity).Name));
        });
    }

    public async Task<TEntity> Find(int id)
    {
        this.LogInformation($"Find({id})");
        return await _context.GetDbSet(typeof(TEntity).Name).FindAsync(id);
    }

    public async Task Update(TEntity targetData)
    {
        this.LogInformation($"Update({targetData.ToJson()})");
    
        /*object targetInstance = Find(((dynamic)targetData).ID);
        foreach (PropertyInfo propertyInfo in targetInstance.GetType().GetProperties())
        {
            if (Typing.IsPrimitive(propertyInfo.PropertyType))
            {
                propertyInfo.SetValue(targetInstance, propertyInfo.GetValue(targetData));
            }
        }*/
        _context.Update(targetData);
        await _context.SaveChangesAsync();
    }

    /*
    public async Task<int> CountRecord(string entity)
    {
        this.LogInformation($"CountRecord({entity})");
        int count = await (from p in ((IQueryable<dynamic>)_db.GetDbSet(entity)) select p).CountAsync();
        return count;
    }

    public async Task<int> PagesCount(string entity, int size)
    {
        this.LogInformation($"PagesCount({entity},$"{ size}
        ")");
        int c = await this.CountRecord(entity);
        return ((c % size) == 0) ? ((int)(c / size)) : (1 + ((int)((c - ((c % size))) / size)));
    }

    object Page(string entity, int page, int size)
    {
        this.LogInformation($"Page({$"{entity},{page},{size}"})");
        return (from p in ((IQueryable<dynamic>)_db.GetDbSet(entity)) select p).Skip((page - 1) * size).Take(size).ToList();
    }

    public TEntity[] List( )
    {
        return (from p in ((IQueryable<TEntity>)_db.GetDbSet(typeof(TEntity).Name)) select p).ToArray<TEntity>();
    }

    public TEntity[] List(  int page, int size)
    {
        TEntity[] resultset = (from p in ((IQueryable<TEntity>)_db.GetDbSet(typeof(TEntity).Name)) select p).Skip((page - 1) * size).Take(size).ToArray<TEntity>();
        return resultset;
    }

    

    object Where(string entity, string expression)
    {
        return (from p in ((IQueryable<dynamic>)(_db.GetDbSet(entity))) select p).ToList();
    }
    object Where(string entity, string key, object value)
    {
        return (from p in ((IQueryable<dynamic>)(_db.GetDbSet(entity))) where ((object)p).GetPropertyValue(key) == value select p).ToList();
    }

     
     












    IQueryable<dynamic> Page(IQueryable<dynamic> items, int page, int size)
    {
        return items.Skip((page - 1) * size).Take(size);
    }

    HashSet<string> GetKeywords(string entity, string query)
    {
        IQueryable<object> q = ((IQueryable<object>)(_db.GetDbSet(entity)));
        HashSet<string> keywords = Expressions.GetKeywords(q, entity, query);
        return keywords;
    }

    public async Task<int> Count(string entity)
    {
        this.LogInformation($"Count({entity})");
        int count = await (from p in ((IQueryable<dynamic>)_db.GetDbSet(entity)) select p).CountAsync();
        return count;
    }

    public void CreateBySqlScript(object item)
    {
        throw new NotImplementedException();
    }



    */

  
   

   
    /* public JArray Search(string entity, string query)
        {

        DatabaseManager dbm = _db.GetDatabaseManager();
        TableManager tm = dbm.fasade[Counting.GetMultiCountName(entity)];


        return tm.Search(GetIndexes(entity), query);
        }

        private object GetValue(object i, string v)
        {
        PropertyInfo propertyInfo = i.GetType().GetProperty(v);
        FieldInfo fieldInfo = i.GetType().GetField(v);
        return
          fieldInfo != null ? fieldInfo.GetValue(i) :
          propertyInfo != null ? propertyInfo.GetValue(i) :
          null;
        }
    */




}
