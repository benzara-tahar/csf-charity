using CSF.Charity.Application.Common.Interfaces;
using CSF.Charity.Application.TodoItems.Repositories;
using CSF.Charity.Infrastructure.Files;
using CSF.Charity.Infrastructure.Identity;
using CSF.Charity.Infrastructure.Persistence;
using CSF.Charity.Infrastructure.Persistence.Mappings;
using CSF.Charity.Infrastructure.Repositories;
using CSF.Charity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Text;

namespace CSF.Charity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // setup mongodb
            // domain class mapping
            MongoDbClassMappings.InitializeGuidRepresentation();
            MongoDbClassMappings.MapDomainEntities();
            var connectionString = configuration.GetSection("Database").GetValue<string>("ConnectionString");
            var databaseName = configuration.GetSection("Database").GetValue<string>("DatabaseName");

            services.AddSingleton<IMongoDatabase>(_ =>
            {
                var client = new MongoClient(connectionString);
                return client.GetDatabase(databaseName);
            });
            services.AddSingleton<IMongoDbContext>(_ =>
            {
                var mongoDatabase = services.BuildServiceProvider().GetRequiredService<IMongoDatabase>();
                return new MongoDbContext(mongoDatabase);
            });
        
            // repositories
            services.AddScoped(typeof(IRepository<,>), typeof(MongoRepository<,>));
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();

            // events
            services.AddScoped<IDomainEventService, DomainEventService>();

            // identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddMongoDbStores<ApplicationUser, ApplicationRole, string>(
                 connectionString,
                 databaseName
             ).AddDefaultTokenProviders();

            //var jwtSection = configuration.GetSection(nameof(JwtBearerTokenSettings));
            //services.Configure<JwtBearerTokenSettings>(jwtSection);
            //var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
            //var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidIssuer = jwtBearerTokenSettings.Issuer,
            //        ValidAudience = jwtBearerTokenSettings.Audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //    };
            //});

            // services
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            services.AddRazorPages();
            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }
    }
}