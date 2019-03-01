using GiftAidCalculator.Config;
using GiftAidCalculator.Persistance;
using GiftAidCalculator.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace GiftAidCalculator
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<PostCodeValidationConfig>(Configuration.GetSection("PostCodeValidation"));
            services.AddTransient<IGiftAidCalculator, GiftAidCalculator>();
            services.AddTransient<IGiftAidRepository, GiftAidRepository>();
            services.AddTransient<IPostCodeValidator, PostCodeValidator>();
            services.AddHttpClient<IPostCodeValidator, PostCodeValidator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("1.0.0", new Info { Title = "Giftaid API", Version = "1.0.0" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "status/discover/ui";
                c.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "Auto-gen");
            });
        }
    }
}
