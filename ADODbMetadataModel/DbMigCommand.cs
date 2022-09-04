using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

 
public class DbMigCommand 
{
    [NotNullNotEmpty]        
    [InputPositiveInt]
    public int Version { get; set; }

    [NotNullNotEmpty]
    [InputMultilineText]
    public string Up { get; set; }

    [NotNullNotEmpty]
    [InputMultilineText]
    public string Down { get; set; }

    public DbMigCommand(string up, string down, int version)
    {
        Up = up;
        Down = down;
        Version = version;
    }
}
 