using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.DependencyInjection;
using WebServer.Core.Configuration;
using WebServer.Core.MVC;

namespace TestWebApp
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddService<IMessageSender, SmsMessageSender>();
            services.AddService<IMessageSender, EmailMessageSender>();
            services.AddService<TimeService>();
        }

        public void Configure(ApplicationBuilder app)
        {
            //app.Use((context, next) =>
            //{
            //    context.items["cache"] = "1234567890";
            //    next?.Invoke(context);
            //});

            //app.Use((context, next) =>
            //{
            //    var sender = context.GetService<IMessageSender>();
            //    var time = context.GetService<TimeService>();

            //        //context.Response.Write(sender.Send());              
            //        context.Response.Write(time.GetTime());

            //        //context.Response.Write(context.items["cache"]);
            //        next?.Invoke(context);
            //});

            app.UseMap("/index", (context) =>
            {
                context.Response.Write("Index");
            });
            app.UseMap("/about", (context) =>
            {
                context.Response.Write("About");
            });

            app.UseMap("/home/careers", (context) =>
            {
                context.Response.Write("Careers");
            });

            //app.UseMiddleWare<TokenMiddleWare>();

            app.UseMVC(
                name     : "default",
                template : @"{controller}/{action}",
                _default: "controller=Home/action=index"
                );
        }
    }
}
