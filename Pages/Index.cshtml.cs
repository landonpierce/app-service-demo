using Microsoft.AspNetCore.Mvc;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace app_service_demo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _config;
    private readonly SecretClient _keyVaultSecretClient;
    public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        // You should use dependency injection here to inject this client. Creating it on every request as shown here is expensive, but is fine for demo purposes.
        _keyVaultSecretClient = new SecretClient(new Uri(_config["KeyVault:Url"]), 
       // https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet 
        new DefaultAzureCredential());
    }

    public async Task OnGet()
    {
        // Option 1:
        // Pulls a secret from config (Injected from Key Vault on line 9 of Program.cs)
        ViewData["Secret1"] = _config["Secret1"];
        
        // Option 2:
        // Pulls a secret directly from KeyVault using the KeyVault SDK: https://www.nuget.org/packages/Azure.Security.KeyVault.Secrets/
        var secret = await _keyVaultSecretClient.GetSecretAsync("secret2");
        ViewData["Secret2"] = secret.Value.Value; 
    }
}
