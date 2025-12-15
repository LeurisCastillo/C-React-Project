using CandidatesChannels.Application;
using CandidatesChannels.Infrastructure;
using CandidatesChannels.Infrastructure.Options;
using CandidatesChannels.Infrastructure.Seed;
using CandidatesChannels.WebApi.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

var jwt = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()!;

// Config
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CandidatesChannels API", Version = "v1" });

    // JWT bearer in Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {token}"
    });

    c.AddSecurityRequirement(new()
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
            new string[] {}
        }
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ProblemDetails (built-in)
builder.Services.AddProblemDetails();
builder.Services.AddTransient<CandidatesChannels.WebApi.Middleware.ExceptionHandlingMiddleware>();

var corsPolicyName = "ClientCors";

// CORS (Vite default)
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    // We'll handle validation manually for consistent errors
    opt.SuppressModelStateInvalidFilter = true;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DbSeeder.SeedAsync(app.Services);

app.Run();
