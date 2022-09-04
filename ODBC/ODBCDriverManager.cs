using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 

public class OdbcDriverManager: MyValidatableObject
{


    public class OdbcDriver
    {

        public string Name { get; set; }
        public string Platform { get; set; }
        public string Attribute { get; set; }
    }


    public class OdbcDatasourceDescription
    {

        public string Name { get; set; }
        public string DriverName { get; set; }
        public string DsnType { get; set; }
        public string Platform { get; set; }
        public string Attribute { get; set; }

    }
    public string[] GetOdbcDatasourcesNames() => GetOdbcDatasources().Select(d => d.Name).ToArray();
    public OdbcDatasourceDescription[] GetOdbcDatasources()
    {
        bool next = true;
        OdbcDatasourceDescription ds = null;
        var dss = new List<OdbcDatasourceDescription>();
        this.Execute("Get-OdbcDsn").ReplaceAll("\r", "").Split("\n").ToList().ForEach(line=>{
            if(string.IsNullOrEmpty(line)){
                if(ds != null)
                    dss.Add(ds);
                next = true;
                
            }
            else
            {
                //System.Console.WriteLine(line.Length+ " "+line);                
                if(next || ds==null)
                    ds = new OdbcDatasourceDescription();
                next = false;
                string[] arr = line.Split(":");
                string key = arr[0].Trim();
                string value = arr[1].Trim();

                //Info(key+"="+value);
                ds.GetType().GetProperty(key).SetValue(ds,value);
            }
        });
        return dss.ToArray();
    }


    public OdbcDriver[] GetOdbcDrivers()
    {
        bool next = true;
        OdbcDriver driver = null;
        var drivers = new List<OdbcDriver>();
        this.Execute("Get-OdbcDriver").ReplaceAll("\r", "").Split("\n").ToList().ForEach(line=>{
            if(string.IsNullOrEmpty(line)){
                if(driver != null)
                    drivers.Add(driver);
                next = true;
                
            }
            else
            {
                //System.Console.WriteLine(line.Length+ " "+line);                
                if(next || driver==null)
                    driver = new OdbcDriver();
                next = false;
                string[] arr = line.Split(":");
                string key = arr[0].Trim();
                string value = arr[1].Trim();

                //Info(key+"="+value);
                driver.GetType().GetProperty(key).SetValue(driver,value);
            }
        });
        return drivers.ToArray();
    }


    private string Execute(string command)
    {
        System.Console.WriteLine("Выполнение команды: "+command);

        ProcessStartInfo info = new ProcessStartInfo("PowerShell.exe", "/C " + command);
        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
                    
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        System.IO.StreamReader reader = process.StandardOutput;

        string result = reader.ReadToEnd();

        System.Console.WriteLine("Результат выполнения: \n"+result);
        return result;
    }
}


public class OdbcDriverManagerTest : TestingElement
{
    public override void OnTest()
    {
        var manager = new OdbcDriverManager();
        try
        {
            manager.GetOdbcDrivers().ToJsonOnScreen().WriteToConsole();
            Messages.Add("Функция получения списка драйверов ODBC работает корректно");
        }
        catch (Exception)
        {
            Messages.Add("Функция получения списка драйверов ODBC не работает корректно");

        }
        try
        {
            manager.GetOdbcDatasources().ToJsonOnScreen().WriteToConsole(); 
            Messages.Add("Функция получения списка источников ODBC работает корректно");
        }
        catch (Exception)
        {
            Messages.Add("Функция получения списка источников ODBC не работает корректно");

        }
        manager.GetOdbcDrivers().ToJsonOnScreen().WriteToConsole();
    }
}