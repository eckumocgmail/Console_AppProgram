internal class SqlServerOdbcConnectionString
{
    public SqlServerOdbcConnectionString()
    {
    }

    public string Server { get; set; }
    public string Database { get; set; }
    public bool TrustedConnection { get; set; }
    public string UserID { get; set; }
    public string Password { get; set; }
}