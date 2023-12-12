using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;

namespace init_edgehub;

internal class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();
        
        string hostname = configuration.GetValue<string>("hostname")!;
        string edgeId = configuration.GetValue<string>("edgeId")!;
        string sasKey = configuration.GetValue<string>("sasKey")!;
        string modId = "$edgeHub";

        if (string.IsNullOrEmpty(hostname) || string.IsNullOrEmpty(edgeId) || string.IsNullOrEmpty(sasKey))
        {
            PrintUsage();
        }
        else
        {
            await InitModule(hostname, edgeId, sasKey, modId);
        }
    }

    private static async Task InitModule(string hostname, string edgeId, string sasKey, string moduleId)
    {
        const string Api_Version_2021_04_12 = "api-version=2021-04-12";

        string putUrl = $"https://{hostname}/devices/{edgeId}/modules/{moduleId}?{Api_Version_2021_04_12}";
        using HttpClient putClient = new();
        HttpRequestMessage reqPut = new(HttpMethod.Put, putUrl);
        reqPut.Headers.Add(HttpRequestHeader.Authorization.ToString(), Sas.GetToken(hostname, sasKey));
        reqPut.Headers.IfMatch.Add(new System.Net.Http.Headers.EntityTagHeaderValue("\"*\""));

        Module modIdentity = new()
        {
            Authentication = new Authentication { Type = "sas", SymmetricKey = new SymmetricKey { PrimaryKey = null } },
            ModuleId = moduleId,
            ManagedBy = "IotEdge",
            DeviceId = edgeId,
            ConnectionState = "Disconnected"
        };

        string modJson = JsonSerializer.Serialize(modIdentity);

        reqPut.Content = new StringContent(modJson, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage respPut = await putClient.SendAsync(reqPut);
        if (!respPut.IsSuccessStatusCode)
        {
            throw new ApplicationException("Error initializing module. " + respPut.ReasonPhrase);
        }
        else
        {
            string putRespJson = await respPut.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync(putRespJson);
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine("init-edgehub tool");
        Console.WriteLine(" requires hostname, edgeId and sasKey parameters");
        Console.WriteLine(" eg. init-edgehub --hostname=myhub.azure-devices.net --edgeId=edge01 --sasKey=<edgeDeviceSharedAccesssKey");
        Console.WriteLine();
    }
}