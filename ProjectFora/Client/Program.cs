global using ProjectFora.Shared;
global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectFora.Client;
using ProjectFora.Client.Services;
using ProjectFora.Client.CustomStateProvider;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IInterestManager, InterestManager>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IMessageManager, MessageManager>();
builder.Services.AddScoped<IThreadsManager, ThreadsManager>();
builder.Services.AddScoped<IUserInterestManager, UserInterestManager>();

builder.Services.AddOptions();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
