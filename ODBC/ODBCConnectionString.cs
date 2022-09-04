
public class ODBCConnectionString
{
    public ODBCConnectionString(): this("msdb","admin","p@ssword")
    {
    }

    public ODBCConnectionString(string dataSourceName, string userID, string password)
    {
        DataSourceName = dataSourceName;
        UserID = userID;
        Password = password;
    }

    public string DataSourceName { get; set; }
    public string UserID { get; set; }
    public string Password { get; set; }

    public override string ToString()
        => $"dsn={DataSourceName};UID={UserID};PWD={Password};";
    
}
