using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.PasswordFolder;
using RepositoryLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLayer.Services.NotesServices;
using RepositoryLayer.Interfaces.NotesInterface;
using RepositoryLayer.Services.NotesServices;
using Microsoft.OpenApi.Models;
using static MassTransit.Logging.LogCategoryName;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);  // this line provide logger to our program

// Add services to the container.


/*builder.Logging.ClearProviders();
builder.Logging.AddDebug();*/
 

builder.Services.AddMassTransit(x => {
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.UseHealthCheck(provider);
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        }); 
    }));
});

builder.Services.AddMassTransitHostedService();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // below syntax to generate token
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // it validates the server, that generates the token
            ValidateAudience = true,// it validates the recipient of the token, is autherised to recieve
            ValidateLifetime = true,// it checks, if token is not expired at end the signing issuer key is valid or not
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // 
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // it valids the signature of the token

        };
    }); // raja


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None; // or SameSiteMode.Lax or SameSiteMode.Strict
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "FundooNew2", Version = "v1" });
    //option.OperationFilter<SwaggerFileOperationFilter>();

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    }); 
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        new string[]{}
    }
});
});



builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IUserLoginBusiness, UserLoginBusiness>();
builder.Services.AddScoped<IUserLoginRepository, UserLoginRepository>();
builder.Services.AddScoped<IPasswordBusiness, PasswordBusiness>();
builder.Services.AddScoped<IPasswordRepo, PasswordRepo>();
builder.Services.AddScoped<INotesBusiness,NotesBusiness >();
builder.Services.AddScoped<INotesRepository, NotesRepository>();

builder.Services.AddDbContext<UserContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); // this is there before
builder.Services.AddDbContext<NotesContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // raja
app.UseAuthorization();
app.UseSession();// raja2


app.MapControllers();

app.Run();
