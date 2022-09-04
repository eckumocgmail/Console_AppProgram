using DataADO;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    /// <summary>
    /// Модель параметров подключения к источнику данных ADO MySQL
    /// </summary>
    public class MySqlConnectionString: IDisposable
    {

        [Display(Name = "Сервер")]
        [Required(ErrorMessage = "Необходимо ввести наименование источника")]
        public string Server { get; internal set; }


        [Display(Name = "Пользователь")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string UserID { get; internal set; }


        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Password { get; internal set; }


        [Display(Name = "Порт")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int Port { get; internal set; }


        [Display(Name = "База данных")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Database { get;  internal set; }


        public MySqlConnectionString() : this("localhost", 3306, "mysql", "root", "sgdf1423")
        {
        }

        public MySqlConnectionString(string server, int port, string database, string userID, string password)
        {
            Server = server;
            UserID = userID;
            Password = password;
            Port = port;
            Database = database;
        }

        /// <summary>
        /// Получение строки соединения ADO.NET
        /// </summary>
        /// <returns> строка соединения </returns>
        public override string ToString()
        {
            return $"Server={Server};Port={Port};Database={Database};User ID={UserID};Password={Password};PersistSecurityInfo=True;CharSet=utf8;SslMode=none";
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
