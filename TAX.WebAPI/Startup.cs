using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using UEditor.Core;
using Microsoft.AspNetCore.Http;

namespace UIDP.WebAPI
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
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); 
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
            {

                x.ValueLengthLimit = int.MaxValue;

                x.MultipartBodyLengthLimit = int.MaxValue;
 
                x.MultipartHeadersLengthLimit = int.MaxValue;


            });
            services.AddUEditorService("ueditor.json", true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "ExcelModel")),
                RequestPath = "/ExcelModel"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
       System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "ExcelModel/Templates")),
                RequestPath = "/ExcelModel/Templates"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Files/export")),
                RequestPath = "/Files/export"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
               System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "UploadFiles/img")),
                RequestPath = "/UploadFiles/img",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "UploadFiles/notice")),
                RequestPath = "/UploadFiles/notice",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });
            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });
            #endregion
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });
            app.UseMvc();
        }
    }
}
