public class ODBCDbTestUnit : TestingUnit
{
    public ODBCDbTestUnit()
    {
        Append(new OdbcDriverManagerTest());
        Append(new OdbcDbMetaDataTest());
    }
}