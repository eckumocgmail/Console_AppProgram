using DataADO;

using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace DataADO
{
    public class SqlServerConnectionString : IDisposable
    {
        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
        [Display(Name = "Сервер")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string DataSource { get; set; }

        [Display(Name = "База данных")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string InitialCatalog { get; set; }

        [Display(Name = "Проверка подлинности Window")]
        [Required(ErrorMessage = "Обязательное поле")]
        public bool IntegratedSecurity { get; set; } = true;

        [Display(Name = "Исп.сертификата")]
        [Required(ErrorMessage = "Обязательное поле")]
        public bool TrustServerCertificate { get; set; } = true;

        [Display(Name = "Пользователь")]
        public string UserID { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public SqlServerConnectionString() : this("AGENT\\KILLER", "Mvc.Apteka",true,false) { }
 
        public SqlServerConnectionString(string DataSource, string InitialCatalog, bool IntegratedSeurity, bool TrustServerSertificate, string userID="", string password="")
        {
            this.DataSource = DataSource;
            this.InitialCatalog = InitialCatalog;
            this.IntegratedSecurity = IntegratedSeurity;
            this.TrustServerCertificate = TrustServerSertificate;
            this.UserID = userID;
            this.Password = password;
        }

        public override string ToString()
        {
            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.DataSource = this.DataSource;
            builder.InitialCatalog = this.InitialCatalog;
             
            builder.TrustServerCertificate = this.TrustServerCertificate;
            if((builder.IntegratedSecurity = this.IntegratedSecurity) == false)
            {
                builder.UserID = this.UserID;
                builder.Password = this.Password;
            }
            return builder.ToString();
        }

    }

}