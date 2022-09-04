using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ADODbTestUnit : TestingUnit
{
    public ADODbTestUnit()
    {
        this.Push(new ADODbConnectorServiceUnit());
        this.Push(new ADODbExecutorServiceUnit());
        this.Push(new ADODbMetadataServicesUnit());
        this.Push(new ADODbMigBuilderServiceUnit());
        this.Push(new ADODbModelServiceUnit());
        this.Push(new ADODbApiServiceUnit());
    }

    public override TestingReport DoTest()
    {
        return base.DoTest();
    }
}