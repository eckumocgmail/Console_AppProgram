using DataADO;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ADODbExecutorServiceUnit : TestingElement
{
    public ADODbExecutorServiceUnit()
    {

    }
    public override void OnTest()
    {
        SqlServerExecutorTest();
        SqlServerExecuteProcedureTest();
        MySqlExecutorTest();
        MySqlExecutorProcedureTest();
        PostgresExecutorTest();
        PostgresExecuteProcedureTest();
    }

    private void PostgresExecuteProcedureTest()
    {
    }

    private void PostgresExecutorTest()
    {
    }

    private void MySqlExecutorProcedureTest()
    {
    }

    private void MySqlExecutorTest()
    {
    }

    public void SqlServerExecutorTest()
    {
        try
        {
            using (SqlServerExecutor executor = new SqlServerExecutor())
            {
                executor.PrepareQuery(@"
                        DECLARE @COUNT INT
                        SET @COUNT=(SELECT Count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='History')
                        IF( @COUNT > 0 )
                        BEGIN
	                        DROP TABLE History
                        END
                    ");
                executor.PrepareQuery("CREATE TABLE History( ID INT PRIMARY KEY IDENTITY, CREATED DateTime NOT NULL DEFAULT(GetDate()), COMMAND nvarchar(512) NOT NULL )");
            
                executor.PrepareQuery("INSERT INTO History( COMMAND ) values ('CREATE DATABASE WORLD')");

                DataTable history = executor.ExecuteQuery("SELECT * FROM History");
                executor.Info(GetTextData(history).ToJsonOnScreen());
                executor.PrepareQuery("DROP TABLE History");
                Messages.Add("Функция выполнения запросов MS SQL SERVER работает корректно");
            }
        }catch (Exception )
        {
            Messages.Add("Функция выполнения запросов MS SQL SERVER не работает корректно");
        }
        
    }

    public void SqlServerExecuteProcedureTest()
    {
        using (SqlServerExecutor executor = new SqlServerExecutor("AGENT\\KILLER", "App"))
        {
            executor.PrepareQuery(@"DROP PROCEDURE [dbo].[ON_STARTED]");
            executor.PrepareQuery(@"
                    -- =============================================
                    -- Author:		<Author,,Name>
                    -- Create date: <Create Date,,>
                    -- Description:	<Description,,>
                    -- =============================================
                    CREATE PROCEDURE [dbo].[ON_STARTED]
	
	                    @URL nvarchar(max),
	                    @KEY nvarchar(max) OUTPUT
		
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;

                        -- Insert statements for procedure here
	                    SET @KEY = @URL
	
                    END
                ");
            DataTable Result = executor.ExecuteQuery(@"
                    DECLARE @KEY nvarchar(max)
                    EXEC [dbo].[ON_STARTED] 'http://localhost:8080',@KEY OUTPUT
                    SELECT @KEY AS 'KEY'
                ");
            GetTextData(Result).ToJsonOnScreen().WriteToConsole();
        }
    }


    /// <summary>
    /// Выбираем текстовые данные из результирующего набора
    /// </summary>
    private IEnumerable<IDictionary<string, string>> GetTextData(DataTable dataTable)
    {
        var result = new List<IDictionary<string, string>>();
        foreach (DataRow row in dataTable.Rows)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                object value = row[column.ColumnName];
                data[column.ColumnName] = value == null ? "NULL" : value.ToString();
            }
            result.Add(data);
        }
        return result.ToArray();
    }
}