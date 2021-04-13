using CSF.Charity.Api.Extensions;
using CSF.Charity.Api.Filters;
using CSF.Charity.Api.Services;
using CSF.Charity.Application;
using CSF.Charity.Application.Services;
using CSF.Charity.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CSF.Charity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();

            services.AddInfrastructure(Configuration);

            services.AddSwaggerExtension();
            
            services.AddApiVersioningExtension();
            services.AddCors();
            

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            //services.AddHealthChecks();
                //.AddDbContextCheck<ApplicationDbContext>();

            services.AddControllers(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddFluentValidation(fv =>
                    {
                        fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                        fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                        fv.ImplicitlyValidateChildProperties = true;
                    });


            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            

         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSwaggerExtension();
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            
            
            

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

   
        }
    }
}
