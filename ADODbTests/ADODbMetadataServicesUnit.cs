using DataADO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ADODbMetadataServicesUnit : TestingElement
{
    public ADODbMetadataServicesUnit()
    {

    }
    public override void OnTest()
    {
        SqlServerDatabase_GetTableMetadata_Test();
        SqlServerDatabase_GetProcedureMetadata_Test();
    }
    public void SqlServerDatabase_GetTableMetadata_Test()
    {
        try
        {
            using (var database = new SqlServerDbMetadata())
            {
                database.GetTablesMetadata().ToJsonOnScreen().WriteToConsole();
                Messages.Add("Получене списка мета данных о таблицах работает корректно");
            }
        }catch (Exception)
        {
            Messages.Add("Получене списка мета данных о таблицах НЕ работает корректно");
        }
    }

    public void SqlServerDatabase_GetProcedureMetadata_Test()
    {
        try
        {
            using (var database = new SqlServerDbMetadata())
            {
                foreach (var KeyValuePair in database.GetProceduresMetadata("dbo"))
                {
                    KeyValuePair.Value.ToJsonOnScreen().WriteToConsole();
                }
                Messages.Add("Получене списка мета данных о хранимых процедурах работает корректно");
            }
        }catch (Exception)
        {
            Messages.Add("Получене списка мета данных о хранимых процедурах не работает корректно");
        }
    }
}