using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IEntityRepository<TEntity>
{
    public Task Create(TEntity target);
    public Task Delete(TEntity p);
    public Task<TEntity> Find(int id);
    public Task<IEnumerable<TEntity>> Get();
    public Task Update(TEntity targetData);
    public int Post(TEntity data);
    public int Put(TEntity data);
    public int Patch(params TEntity[] dataset);
    public int Delete(int id);
    public IEnumerable<TEntity> Get(int? id);
}