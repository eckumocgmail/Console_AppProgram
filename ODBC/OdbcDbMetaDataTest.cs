public class OdbcDbMetaDataTest : TestingElement
{
    public override void OnTest()
    {
        Messages.Add(new ODBCSqlExecutor("www","","").GetDatabaseMetadata().ToJsonOnScreen());
    }
}