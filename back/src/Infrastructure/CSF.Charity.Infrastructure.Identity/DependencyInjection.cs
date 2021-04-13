
using AspNetCore.Identity.Mongo;
using CSF.Charity.Domain.Identity;
using CSF.Charity.Infrastructure.Identity.Configurations;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Text;

namespace CSF.Charity.Infrastructure.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityServerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            MapIdentityEntities();
            var url = configuration["Database:ConnectionString"];
            var databaseName = configuration["Database:DatabaseName"];
            var connectionString = $"{url}/{databaseName}";
            services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole, ObjectId>(identity =>
             {
                 identity.Password.RequireDigit = false;
                 identity.Password.RequireLowercase = false;
                 identity.Password.RequireNonAlphanumeric = false;
                 identity.Password.RequireUppercase = false;
                 identity.Password.RequiredLength = 1;
                 identity.Password.RequiredUniqueChars = 0;
             },
                mongo =>
                {
                    mongo.ConnectionString = connectionString;
                });

            // configure strongly typed settings objects
            var jwtSection = configuration.GetSection(nameof(JwtBearerTokenSettings));
            services.Configure<JwtBearerTokenSettings>(jwtSection);
            var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtBearerTokenSettings.Issuer,
                    ValidAudience = jwtBearerTokenSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            return services;
        }

        private static void MapIdentityEntities()
        {


            //BsonClassMap.RegisterClassMap<IdentityUser<string>>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.MapIdMember(c => c.Id);
            //});
            //BsonClassMap.RegisterClassMap<IdentityRole<string>>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.MapIdMember(c => c.Id);
            //});


        }
    }
}
