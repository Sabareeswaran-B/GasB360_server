using GasB360_server.Helpers;
using GasB360_server.Models;
using GasB360_server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers();
    // .AddNewtonsoftJson(
    //     options =>
    //         options.SerializerSettings.ReferenceLoopHandling =
    //             Newtonsoft.Json.ReferenceLoopHandling.Ignore
    // );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition(
            "Bearer",
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            }
        );
        options.AddSecurityRequirement(
            new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            }
        );
    }
);

builder.Services.AddCors();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAzureStorage, AzureStorage>();
builder.Services.AddDbContext<GasB360Context>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("GasB360Context"))
);

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(
        webBuilder =>
        {
            // Add the following line:
            webBuilder.UseSentry(
                o =>
                {
                    o.Dsn =
                        "https://a9deabdc14104bd18c63b9ba7411ba9f@o1169931.ingest.sentry.io/6294525";
                    o.Debug = true;
                    o.TracesSampleRate = 1.0;
                }
            );
        }
    );

builder.WebHost.UseSentry();

var app = builder.Build();

app.UseSentryTracing();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
// }
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseSentryTracing();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseMiddleware<JwtHelper>();

app.MapControllers();

app.Run();
