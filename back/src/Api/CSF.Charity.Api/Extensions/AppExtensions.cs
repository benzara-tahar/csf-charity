using Microsoft.AspNetCore.Builder;

namespace CSF.Charity.Api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppExtensions
    {
        /// <summary>
        /// use swagger extension method
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.WebApi");
            });
        }
       
    }
}
