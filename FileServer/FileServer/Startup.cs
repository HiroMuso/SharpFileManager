using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FileServer.Installers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;

namespace FileServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.InstallServicesEnAssembly(Configuration);
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddMvcCore().AddDataAnnotations();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });


            app.UseHttpsRedirection();

            app.UseFileServer();
            //var option = new StaticFileOptions();
            //var contentTypeProvider = (FileExtensionContentTypeProvider)option.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            //contentTypeProvider.Mappings.Add(".unityweb", "application/octet-stream");
            //option.ContentTypeProvider = contentTypeProvider;
            //app.UseStaticFiles(option);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
