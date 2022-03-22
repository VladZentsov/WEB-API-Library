using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Interfaces;
using Business.Services;
using Data;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Helpers;

namespace WebApi
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
            services.AddRazorPages();

            services.AddControllers();

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IReaderRepository, ReaderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IReaderService, ReaderService>();
            services.AddScoped<IStatisticService, StatisticService>();

                services.AddSingleton(provider => new MapperConfiguration(cfg =>
                 {
                     cfg.AddProfile(new AutomapperProfile());
 
                 }
                ).CreateMapper());



            services.AddDbContext<ILibraryDbContext, LibraryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Library")));

            //services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddControllers();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("../swagger/v1/swagger.json", "MyAPI V1");
                    options.RoutePrefix = string.Empty;
                });

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}