using System.Text;
using Epers.DataAccess;
using EpersBackend.Services.Authenticaion;
using EpersBackend.Services.Common;
using EpersBackend.Services.Competente;
using EpersBackend.Services.Email;
using EpersBackend.Services.Evaluare;
using EpersBackend.Services.Excel;
using EpersBackend.Services.Header;
using EpersBackend.Services.Nomenclatoare;
using EpersBackend.Services.ObiectivService;
using EpersBackend.Services.Pagination;
using EpersBackend.Services.PDF;
using EpersBackend.Services.PIP;
using EpersBackend.Services.Salesforce;
using EpersBackend.Services.Sincron;
using EpersBackend.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddDbContext<EpersContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Use AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add services to the container.

// Nomenclatoare
builder.Services.AddScoped<IEfLocatiiRepository, EfLocatiiRepository>();
builder.Services.AddScoped<IEfDiviziiRepository, EfDiviziiRepository>();
builder.Services.AddScoped<IEfCompartimenteRepository, EfCompartimenteRepository>();
builder.Services.AddScoped<IEfPosturiRepository, EfPosturiRepository>();
builder.Services.AddScoped<IEfSkillsRepository, EfSkillsRepository>();
builder.Services.AddScoped<IEfSetareProfilPostRepository, EfSetareProfilPostRepository>();
builder.Services.AddScoped<IEfNObiectiveRepository, EfNObiectiveRepository>();
builder.Services.AddScoped<IEfCursuriRepository, EfCursuriRepository>();
builder.Services.AddScoped<IEfStariPIPRepository, EfStariPIPRepository>();
builder.Services.AddScoped<IEfFirmeRepository, EfFirmeRepository>();

// PIP
builder.Services.AddScoped<IEfPIPRepository, EfPIPRepository>();
builder.Services.AddScoped<IPIPService, PIPService>();

// User and Authentication
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IPasswordManagement, PasswordManagement>();


//services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICompetenteService, CompetenteService>();
//services.AddScoped<IAspLoggedInUser, AspLoggedInUser>();
builder.Services.AddScoped<IEvaluareService, EvaluareService>();
builder.Services.AddScoped<IHeaderService, HeaderService>();
builder.Services.AddScoped<ISincronizareSincron, SincronizareSincron>();
builder.Services.AddScoped<INotiteService, NotiteService>();
builder.Services.AddScoped<IObiectiveService, ObiectiveService>();
builder.Services.AddScoped<IConcluziiService, ConcluziiService>();

// PDF
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IPdfEvaluareDocumentsDataSource, PdfEvaluareDocumentsDataSource>();
builder.Services.AddScoped<IPdfObiectiveDocumentsDataSource, PdfObiectiveDocumentsDataSource>();
builder.Services.AddScoped<IPdfEvalareDocumentsConclzuieDataSource, PdfEvalareDocumentsConclzuieDataSource>();
builder.Services.AddScoped<IPdfPipDocumentsDataSource, PdfPipDocumentsDataSource>();
builder.Services.AddScoped<IPdfFileOperationService, PdfFileOperationService>();

// Email
builder.Services.AddScoped<ISetEmailService, SetEmailService>();
builder.Services.AddScoped<IEmailSendService, EmailSendService>();

// Excel
builder.Services.AddScoped<IExcelRapoarteService, ExcelRapoarteService>();

builder.Services.AddScoped<IEfSelectionBoxRepository, EfSelectionBoxRepository>();
builder.Services.AddScoped<IDrodpwonRepository, DrodpwonRepository>();

builder.Services.AddScoped<IPagination, Pagination>();
builder.Services.AddScoped<IConversionHelper, ConversionHelper>();

// Salesforce
builder.Services.AddScoped<IEfAgentMetricsRepository, EfAgentMetricsRepository>();
builder.Services.AddScoped<IAgentMetricsService, AgentMetricsService>();

// Bearer Token Swager
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    //options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Authenticaion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Secret)),
            ValidIssuer = "Epers",
            ValidAudience = "Epers-Audience",
            ValidateIssuer = true,
            ValidateAudience = true
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Request.Cookies.TryGetValue("AccessToken", out var accessToken);
                if (!string.IsNullOrWhiteSpace(accessToken))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpClient();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy(name: "epers-front-end",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:86", "http://localhost:8182", "https://epers.careersblitz.com/", "https://epers.careersblitz.com/impact/")
        .AllowCredentials()
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .AllowAnyHeader();
    }));


// Logger
var _logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.File("EpersFullStackDemo/EpersBackend/EpersBackend/Log/backend-log.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.AddSerilog(_logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("epers-front-end");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    foreach (var cookie in context.Request.Cookies)
    {
        Console.WriteLine($"Cookie: {cookie.Key} = {cookie.Value}");
    }
    await next();
});

app.Run();

