using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:Url"]), 
    // https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet 
    new DefaultAzureCredential());

}
catch (Exception e)
{
    // Do not do this in a real scenario. We are "swallowing" the error here for the purpose of demonstration. 
    // In a real scenario, you probably would want your app to fail startup if it cannot connect to keyvault for whatever reason (bad URI, no auth, etc)
    Console.WriteLine("Exception encountered adding azure key vault");
}

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
