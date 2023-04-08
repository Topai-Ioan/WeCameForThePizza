using Chess.Middlewares;

namespace Chess
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<Chat>("/chat");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Demo}/{action=Index}/{id?}"
                    );
            });

        }
    }
}
