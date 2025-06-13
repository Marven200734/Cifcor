using System;
using System.Collections.Generic;

[Serializable]
public class ContextWrapper
{
    public GeoJsonLdContext context;
    public Properties properties;
}

[Serializable]
public class GeoJsonLdContext
{
    
}


[Serializable]
public class Period
{
    public string name;
    public int temperature;
    public string temperatureUnit;
    public string icon;
}

[Serializable]
public class Properties
{
    public List<Period> periods;
}
