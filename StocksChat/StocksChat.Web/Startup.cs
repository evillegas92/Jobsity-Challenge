using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StocksChat.Business.Interfaces.Services;
using StocksChat.Business.MappingProfiles;
using StocksChat.Business.Services;
using StocksChat.Persistence;
using StocksChat.Persistence.Brokers;
using StocksChat.Persistence.Contexts;
using StocksChat.Persistence.Entities;
using StocksChat.Persistence.Interfaces.Brokers;
using StocksChat.Web.HostedServices;
using StocksChat.Web.Hubs;

namespace StocksChat.Web
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
            services.AddIdentity<AppUserEntity, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<StocksChatContext>();
            services.AddDbContext<StocksChatContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("StocksChatConnectionString"),
                    assembly => assembly.MigrationsAssembly(typeof(StocksChatContext).Assembly.FullName));
            });

            services.AddScoped<IMessagesBroker, MessagesBroker>();
            services.AddScoped<IUsersBroker, UsersBroker>();

            services.AddTransient<Seeder>();
            services.AddTransient<IMessagesService, MessagesService>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddControllersWithViews();

            services.AddSignalR();

            services.AddHostedService<BotsRpcHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
