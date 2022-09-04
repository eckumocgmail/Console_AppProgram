using DataADO;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{

    public class PostgresConnectionString: IDisposable

    {

        [Display(Name = "Сервер")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Host { get; set; }

        [Display(Name = "Порт")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int Port { get; set; }


        [Display(Name = "База данных")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Database { get; set; }




        [Display(Name = "Пользователь")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string UserID { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Password { get; set; }

        public PostgresConnectionString() : this("127.0.0.1", 5432, "postgres", "postgres", "sgdf1423")
        {

           
        }

        public PostgresConnectionString(string host, int port, string database, string userID, string password)
        {
             
            Host = host;
            Port = port;
            Database = database;
            UserID = userID;
            Password = password;
        }

        public override string ToString()
        {
            return $"Host={Host};Port={Port};User ID={UserID};Password={Password};Database={Database}";
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
