

using BusinessLogic.Data;
using BusinessLogic.Logic;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Security.Principal;
using System.Text;
using WebApi.Dtos;
using WebApi.MiddleWare;

namespace WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services) {

        services.AddScoped<ITokenService, TokenService>();

        var builder = services.AddIdentityCore<Usuario>();
        builder = new IdentityBuilder(builder.UserType,builder.Services);
        builder.AddEntityFrameworkStores<SeguridadDBContext>();
        builder.AddSignInManager<SignInManager<Usuario>>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:key"])),
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false,
                
                };
            }
            );

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

        services.AddDbContext<MarketDbContext>(opt =>
        {
            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConection"));

        });

        services.AddDbContext<SeguridadDBContext>(opt => {
            opt.UseSqlServer(Configuration.GetConnectionString("IdentitySeguridad"));
        });

        //conexion para redis
        services.AddSingleton<IConnectionMultiplexer>(c => {
            //esto se ejecuta dentro de un docker container
            var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"),true);// cadena configurada en appsettings
            return ConnectionMultiplexer.Connect(configuration);
        });
        // fin conexion redis

    services.AddTransient<IProductoRepository, ProductoRepository>();
        services.AddScoped<ICarritoCompraRepository,CarritoCompraRepository>();

    services.AddControllers();
    services.AddCors(opt =>
    {
        opt.AddPolicy("CorsRule", rule =>
        {
            rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
        });
    });
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        //if (env.IsDevelopment()) {
        //  app.UseDeveloperExceptionPage();
        //}
        app.UseMiddleware<ExceptionMiddleWare>();
        app.UseStatusCodePagesWithReExecute("/error","?code={0}");
        
        
        app.UseRouting();
        app.UseCors("CorsRule");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

}

