using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Application.Core.Entity;
using Application.Core.Interface;
using Application.Entity;
using Application.Entity.Repository;
using Application.Web.Middleware;
using Application.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Application.Web
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DataContext")));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            services.AddMvc();
            services.AddScoped<IRepository<TopicNode>, Repository<TopicNode>>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITopicReplyRepository, TopicReplyRepository>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<UserServices>();
            services.AddMemoryCache();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("Admin", "Allowed");
                    });
            });
            //ÎÄ×Ö±»±àÂë https://github.com/aspnet/HttpAbstractions/issues/315
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRequestIPMiddleware();

            InitializeDatabase(app.ApplicationServices);
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseStatusCodePages();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action=Index}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<DataContext>();
                db.Database.Migrate();
                if (!db.TopicNodes.Any())
                {
                    db.TopicNodes.AddRange(GetTopicNodes());
                    db.SaveChanges();
                }
            }
        }

        IEnumerable<TopicNode> GetTopicNodes()
        {
            return new List<TopicNode>()
            {
                new TopicNode() { Name=".NET Core", NodeName="", ParentId=0, Order=1, CreateOn=DateTime.Now, },
                new TopicNode() { Name=".NET Core", NodeName="netcore", ParentId=1, Order=1, CreateOn=DateTime.Now, },
                new TopicNode() { Name="ASP.NET Core", NodeName="aspnetcore", ParentId=1, Order=1, CreateOn=DateTime.Now, }
            };
        }
    }
}
