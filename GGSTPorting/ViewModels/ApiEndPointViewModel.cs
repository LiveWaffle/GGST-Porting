using System;
using System.IO;
using System.Threading.Tasks;
using GGSTPorting.Services.Endpoints;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace GGSTPorting.ViewModels;

public class ApiEndpointViewModel
{
    private readonly RestClient _client = new (new RestClientOptions
    {
        UserAgent = $"FModel/{Globals.APP_VERSION}",
        MaxTimeout = 3 * 1000
    });
    
    public 
        ApiEndpoint ValorantApi { get; }
    
    public ApiEndpointViewModel()
    {
        ValorantApi = "f";
    }

    public async Task DownloadFileAsync(string fileLink, string installationPath)
    {
        var request = new RestRequest(fileLink);
        var data = _client.DownloadData(request) ?? Array.Empty<byte>();
        await File.WriteAllBytesAsync(installationPath, data);
    }

    public void DownloadFile(string fileLink, string installationPath)
    {
        DownloadFileAsync(fileLink, installationPath).GetAwaiter().GetResult();
    }
}

public class ApiEndpoint
{
    public static implicit operator ApiEndpoint(string v)
    {
        throw new NotImplementedException();
    }
}