using System;

public class SingleSelectApiAttribute : Attribute
{
    private readonly string _resource;

    public SingleSelectApiAttribute( string resource )
    {
        _resource = resource;
    }
}