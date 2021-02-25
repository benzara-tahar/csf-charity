using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Application.Services;
using CSF.Charity.Infrastructure.Files;
using CSF.Charity.Infrastructure.Identity;
using CSF.Charity.Infrastructure.Persistance.Mongo;
using CSF.Charity.Infrastructure.Persistance.Mongo.Repositories;
using CSF.Charity.Infrastructure.Persistence.Mongo.Mappings;
using CSF.Charity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CSF.Charity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // setup mongodb
            // domain class mapping

            MongoDbClassMappings.Configure();
            MongoDbClassMappings.MapDomainEntities();
            var connectionString = configuration.GetSection("Database").GetValue<string>("ConnectionString");
            var databaseName = configuration.GetSection("Database").GetValue<string>("DatabaseName");

            services.AddSingleton<IMongoDatabase>(_ =>
            {
                var client = new MongoClient(connectionString);
                return client.GetDatabase(databaseName);
            });
            services.AddSingleton<IMongoContext>(_ =>
            {
                var mongoDatabase = services.BuildServiceProvider().GetRequiredService<IMongoDatabase>();
                return new MongoDbContext(mongoDatabase);
            });
            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // repositories
            services.AddScoped(typeof(IRepository<,>), typeof(MongoRepository<,>));
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();
            services.AddScoped<IAssociationRepository, AssociationRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ITownshipRepository, TownshipRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAllotmentRepository, AllotmentRepository>();

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
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IDateTime, MachineDateTimeService>();
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