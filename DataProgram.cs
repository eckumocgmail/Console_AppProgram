using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Программа управления источниками данных
/// Выполняет подключение источников данных ADO/ODBC/OLE
/// </summary>
public class DataProgram : TestingUnit
{


    internal static void Main(string[] args)
    {

        //Test
        var program = new DataProgram();
        program.Append(new ADODbTestUnit());
        program.Append(new ODBCDbTestUnit());

        program.DoTest().ToDocument().WriteToConsole();

        //Run
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<DataProgram>();
            }).Build().Run();
    }


    public IConfiguration Configuration { get; }

    public DataProgram()
    {
        var builder = new ConfigurationBuilder();
        foreach (var f in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory()).Where(f => f.ToLower().EndsWith(".json")))
            builder.AddJsonFile(f);
        this.Configuration = builder.Build();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        app.Run(async http =>
        {

            string[] path = http.Request.Path.ToString().Split("/");
            var odbc = new OdbcDriverManager();
            var dsnames = odbc.GetOdbcDatasourcesNames();
            if (path.Length > 1)
            {

                if (dsnames.Contains(path[1]) == false)
                {
                    await http.List("Источники данных", dsnames.Select(option => new System.Collections.Generic.KeyValuePair<string, string>(option, "/" + option)));
                }
                else
                {
                    string datasourceName = path[1];
                    var executor = new ODBCSqlExecutor(datasourceName, "", "");

                    if (path.Length == 2)
                    {
                        await http.List("Наборы данных " + datasourceName, executor.GetTables().Select(option =>
                            new System.Collections.Generic.KeyValuePair<string, string>(option, "/" + datasourceName + "/" + option)));
                    }
                    else
                    {

                        if (path.Length == 3)
                        {
                            var metadata = executor.GetDatabaseMetadata().GetTablesMetadata()[path[2]];
                            this.Info(metadata);
                            await http.Table("!",
                                "key value".Split(" "),
                                new Newtonsoft.Json.Linq.JArray());
                            await http.Table("2",
                                metadata.columns.Keys,
                                executor.GetJsonResult("SELECT * FROM " + path[2]));
                        }
                        else
                        {

                        }

                    }

                }


            }
            else
            {
                await http.List("Источники данных", dsnames.Select(option => new System.Collections.Generic.KeyValuePair<string, string>(option, "/" + option)));
            }




            await Task.CompletedTask;
        });
    }



}
