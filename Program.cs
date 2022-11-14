using AutoMapper;
using BL;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DataBase
var ConnetionString = builder.Configuration.GetConnectionString("NetMixDB");
builder.Services.AddDbContext<NetMixDbContext>(options => options.UseSqlServer(ConnetionString));
#endregion

#region Services
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddScoped<IMovie, MovieServices>();
builder.Services.AddScoped<IDirector, DirectorServices>();
builder.Services.AddScoped<IActor, ActorServices>();
builder.Services.AddScoped<IGenre, GenreServices>();

builder.Services.AddScoped<IUser, UserServices>();
builder.Services.AddScoped<IPasspwordHasher, PasswordServices>();
builder.Services.AddScoped<IRegister, RegisterServices>();
builder.Services.AddScoped<IJwtInitializer, JWTServices>();
builder.Services.AddScoped<IClaimsInitializer, ClaimsServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


#endregion

#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Default";
    options.DefaultChallengeScheme = "Default";
})
    .AddJwtBearer("Default", options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = NetMixKey.CreateKey(),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
#endregion



#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DashBoard",
        p => p.RequireClaim(ClaimTypes.Role, "admin"));
});
#endregion

var app = builder.Build();



#region MiddleWares

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion
