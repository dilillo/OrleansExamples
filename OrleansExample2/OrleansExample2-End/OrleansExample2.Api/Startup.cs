using OrleansExample2.Api.Extensions;
using OrleansExample2.Api.Infrastructure;

namespace OrleansExample2.Api
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IProductOperations, ProductOperations>();
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddApplicationInsights("Silo");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.Map("/dashboard", x => x.UseOrleansDashboard());
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();

            app.UseMvc();
        }
    }
}
