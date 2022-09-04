using DataADO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ADODbConnectorServiceUnit : TestingElement
{
    public ADODbConnectorServiceUnit()
    {

    }
    public override void OnTest()
    {
        //PostgresDbConnector_Test();
        //MySqlDbConnector_Test();
        SqlServerDbConnector_Test();
    }

    private void PostgresDbConnector_Test()
    {
        try
        {
            PostgresDbConnector connector = new PostgresDbConnector();
            connector.Info(connector.ToString());
            using (var connection = connector.CreateAndOpenConnection())
            {
                connector.Info(connection.ConnectionString);
            }
        }
        catch(Exception ex)
        {
            this.Messages.Add("Не удалось установить соединение с Postgres");
        }
    }

    private void MySqlDbConnector_Test()
    {
        MySqlDbConnector connector = new MySqlDbConnector();
        using (var connection = connector.CreateAndOpenConnection())
        {
            connector.Info(connection.ConnectionString);
        }
    }

    private void SqlServerDbConnector_Test()
    {
        SqlServerDbConnector connector = new SqlServerDbConnector();

        try
        {
            using (var connection = connector.CreateAndOpenConnection())
            {
              
                this.Messages.Add("Функция установки соединения с источниками MSSQLServer работает корректно/Проверено на \"" + connector.ToString() + "\"");
                connector.Info(connection.ConnectionString);
            }
        }
        catch(Exception ex)
        {
            this.Messages.Add("Функция установки соединения с источниками MSSQLServer не работает/Проверено на \"" + connector.ToString() + "\"");
        }
    }
}