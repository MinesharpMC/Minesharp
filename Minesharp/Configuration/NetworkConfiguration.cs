using System.Net;
using System.Text.Json.Serialization;

namespace Minesharp.Configuration;

public class NetworkConfiguration
{
    public string Host { get; init; }
    public int Port { get; init; }
    
    [JsonIgnore]
    public IPEndPoint Ip => IPEndPoint.Parse($"{Host}:{Port}");
}