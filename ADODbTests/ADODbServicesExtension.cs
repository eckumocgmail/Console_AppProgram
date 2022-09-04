
using DataADO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ADODbServicesExtension
{
   
    public static IServiceCollection AddSqlServer<TDBContext>(this IServiceCollection services,
        string Server, bool TrustedConnection, string Username = null, string Password = null)
        where TDBContext : DbContext
        
    {
        System.Console.WriteLine($"[Info][{typeof(TDBContext).Name}]: AddSqlServer({Server},{typeof(TDBContext).Name},{TrustedConnection},{Username},{Password})");
        services.AddDbContext<TDBContext>(options => {
            var connectionString = new SqlServerOdbcConnectionString()
            {
                Server = Server,
                Database = typeof(TDBContext).Name,
                TrustedConnection = TrustedConnection,
                UserID = Username,
                Password = Password
            };
            var validator = new SqlServerWebApi();
            if (validator.CanConnect( ))
            {
                //options.UseInMemory(connectionString.ToString());
            }
            else
            {
                connectionString.Database = "Model";
                if (validator.CanConnect( ))
                {
                    validator.PrepareQuery(
                       
                        $"Create database {typeof(TDBContext).Name}");
                    connectionString.Database = typeof(TDBContext).Name;
                    //options.UseInMemory(connectionString.ToString()); ;
                }
                else
                {
                    throw new System.Exception(
                        "Не удалось подключиться к источнику медицинских данных. " +
                        "Проверьте строку подключения: " + connectionString.ToString());
                }
                
            }
        });
        
        return services;
    }


}