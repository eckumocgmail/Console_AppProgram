using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IEntityBook<TEntityModel>
{
    public int GetPage();
    public int GetPagesCount();
    public void SetPage(int Page, int Size);
    public IEnumerable<TEntityModel> GetItemOnPage();
}