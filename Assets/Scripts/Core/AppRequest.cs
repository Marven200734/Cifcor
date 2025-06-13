using System;
using UnityEngine.Networking;

public class AppRequest
{
    public string URL { get; }
    public string Tag { get; }
    public Action<string> OnSuccess { get; }
    public Action<string> OnFailure { get; }


    public UnityWebRequest WebRequest { get; set; }

    public AppRequest(string url, string tag, Action<string> onSuccess, Action<string> onFailure)
    {
        URL = url;
        Tag = tag;
        OnSuccess = onSuccess;
        OnFailure = onFailure;
    }
}
