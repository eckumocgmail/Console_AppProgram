using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public interface IEntityModel
{
    public string GetNameRu();
    public string GetNameEng();
    public string GetDescriptionRus();
    public string GetDescriptionEng();
    public string GetPrimaryKey();
    public bool IsAutoIncrement();
    public bool IsHierDictionary();



    public string[] GetNavigationProperties();

    // файлы, бинарные данные, изображения, блобы, большой текст
    public string[] GetBigDataColumns();


    // список колонок которые необходимо заполнить для регистрации информации
    public string[] GetRequiredInput();

    public string[] GetOneToManyRelations();
    public string[] GetOneToOneRelations();
    public string[] GetManyToManyRelations();
}

