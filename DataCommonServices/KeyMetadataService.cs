using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCommon.DatabaseMetadata
{
    public static class Extensions
    {



        public static string RemoveTokens(this string text, params string[] chars)
        {
            foreach (var ch in chars)
            {
                text = text.ReplaceAll(ch, "");
            }
            return text;
        }

        public static KeyValuePair<string, string> SplitSingle(this string text, string separator)
        {
            string[] arr = text.Split(separator);
            if (arr.Length != 2)
                throw new Exception($"Некорректные данные [{text}] для разбиения строки на две подстроки через разделитель " + separator);
            return new KeyValuePair<string, string>(arr[0], arr[1]);
        }
    }
    public class KeyMetadataService
    {
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReferenceKey"></param>
        /// <param name="SourceKey"></param>
        /// <returns></returns>
        public string[] CreateForeignKey(string Provider, string ReferenceKey, string SourceKey, string OnDelete, string OnUpdate)
        {

            //CASCADE
            if (string.IsNullOrEmpty(Provider)) throw new Exception("Аргумент Provider содержит нулевые данные либо нулевые ссылки");
            if (string.IsNullOrEmpty(ReferenceKey)) throw new Exception("Аргумент ReferenceKey содержит нулевые данные либо нулевые ссылки");
            if (string.IsNullOrEmpty(SourceKey)) throw new Exception("Аргумент SourceKey содержит нулевые данные либо нулевые ссылки");

            var Reference = ReferenceKey.SplitSingle("-");
            var Source = SourceKey.SplitSingle("-");
            
            string ReferenceTableName = Reference.Key.RemoveTokens("[", "]");
            string SourceTableName = Source.Key.RemoveTokens("[", "]");
            if (ReferenceTableName.IsTsqlStyled() == false) throw new Exception("Проверьте аргумент ReferenceKey");
            if (SourceTableName.IsTsqlStyled() == false) throw new Exception("Проверьте аргумент SourceKey");
            switch (Provider)
            {
                case "SqlServer":                    
                    return new string[]{
                        $@" ALTER TABLE {Reference.Key} WITH CHECK ADD CONSTRAINT [FK__{ReferenceTableName}_{SourceTableName}=>{Source.Value}{Source.Key}] FOREIGN KEY([{Source.Value}])
                           REFERENCES {Source.Key} ({Source.Value})
                           ON DELETE {OnDelete} ON UPDATE {OnUpdate}",
                        $@" ALTER TABLE [dbo].[Authors] CHECK CONSTRAINT [FK_Authors_Resources_ResourceID]"
                    };
                case "MySql":
                    throw new NotImplementedException();
                case "Postgre":
                    throw new NotImplementedException();
                default: throw new Exception($"Аргумент Provider может принимать значения: SqlServer,MySql,Postgre, но никак не "+Provider);
            }
        }

    }
}
