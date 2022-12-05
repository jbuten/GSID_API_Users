using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SIDEnterpriseApp.Shared.Contexts;
using SIDEnterpriseApp.Shared.Models;

var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", false);


IConfiguration config = builder.Build();

var serviceProvider = new ServiceCollection()
            .Configure<AppSettings>(opt => {
                opt.JWTKey = config["AppSettings:JWTKey"];
                opt.DatabaseName = config["AppSettings:DatabaseName"];
                opt.ConnectionString = config["AppSettings:ConnectionString"];
                opt.CollectionName = config["AppSettings:CollectionName"];
                opt.DomainServer = config["AppSettings:DomainServer"];
                opt.DomainUser = config["AppSettings:DomainUser"];
                opt.DomainPass = config["AppSettings:DomainPass"];
                opt.PhotoPath = config["AppSettings:PhotoPath"];
            })
            .AddSingleton<IAppSettings, AppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value)
            .AddScoped<IDbService, DbService>()
            .BuildServiceProvider();


var iAppSettings = serviceProvider.GetService<IAppSettings>();
var iDbService = serviceProvider.GetService<IDbService>();

//Console.WriteLine($"Connection String is: {iAppSettings.ConnectionString}");
//Console.WriteLine($"Email Host is: {iAppSettings.DomainServer}");
//Console.WriteLine(iAppSettings.PhotoPath);

SIDEnterpriseApp.Console.ActiveDirectory AD = new SIDEnterpriseApp.Console.ActiveDirectory(iDbService, iAppSettings);
//AD.GetUserProperty();
AD.SetADUsers();
//Console.ReadLine();