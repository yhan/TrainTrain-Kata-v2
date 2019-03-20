using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainTrain.Domain.Port;
using TrainTrain.Infrastructure.Adapter;

namespace TrainTrain.Api
{
    using Domain;

    public class Startup
    {
        public const string UriBookingReferenceService = "http://localhost:51691/";
        public const string UriTrainDataService = "http://localhost:50680";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var bookingReferenceAdapter = new BookingReferenceAdapter(UriBookingReferenceService);
            var trainDataAdapter = new TrainDataAdapter(UriTrainDataService);
            var hexagon = new TicketOfficeService(trainDataAdapter, bookingReferenceAdapter);
            var seatReservationAdapter = new SeatReservationAdapter(hexagon);

            services.AddSingleton(seatReservationAdapter);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
        }
    }
}
