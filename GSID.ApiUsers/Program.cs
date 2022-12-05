using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GSID.Logs;
using GSID.Users;

const string allowedOrigin = "allowedOrigin";

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<LogsSettings>(builder.Configuration.GetSection(nameof(LogsSettings)));
builder.Services.AddSingleton<ILogsSettings>(sp => sp.GetRequiredService<IOptions<LogsSettings>>().Value);
builder.Services.AddScoped<ILogsContext, LogsContext>();

builder.Services.Configure<UsersSettings>(builder.Configuration.GetSection(nameof(UsersSettings)));
builder.Services.AddSingleton<IUsersSettings>(sp => sp.GetRequiredService<IOptions<UsersSettings>>().Value);
builder.Services.AddScoped<IUsersContext, UsersContext>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API-Users",
        Description = "API users of the companies GrupoSID",
        TermsOfService = new Uri("https://api.gruposid.com.do/terms"),
        Contact = new OpenApiContact
        {
            Name = "Jose Buten",
            Email = "J.Buten@mercasid.com.do",
            Url = new Uri("https://api.gruposid.com.do/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Jose Buten",
            Url = new Uri("https://api.gruposid.com.do/license")
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = " Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["UsersSettings:JwtBearer:Issuer"],
        ValidAudience = builder.Configuration["UsersSettings:JwtBearer:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["UsersSettings:JwtBearer:Key"]))
    };
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigin,
                      builder =>
                      {
                          //builder.WithOrigins("https://gruposid.com.do/", "https://apps.gruposid.com.do/");
                          builder
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                      });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c => {
            c.DefaultModelsExpandDepth(-1);
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API users v1.0 @GrupoSID");
        }
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MapGet("/", () => $"API users v1.0 @GrupoSID  *  {DateTime.Now.ToString()}").ExcludeFromDescription();

app.MapGet("/api", () => $"API users v1.0 @GrupoSID  *  {DateTime.Now.ToString()} - API").ExcludeFromDescription();

app.Run();
