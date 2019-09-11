using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Homeworks.BusinessLogic;
using Homeworks.BusinessLogic.Extensions;
using Homeworks.BusinessLogic.Interface;
using Homeworks.Domain;
using Microsoft.EntityFrameworkCore;
using Homeworks.DataAccess;
using Homeworks.DataAccess.Interface;

namespace Homeworks.WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            /* services.AddDbContext<DbContext, HomeworksContext>(
                o => o.UseSqlServer(Configuration.GetConnectionString("HomeworksDB"))
            ); */
            services.AddDbContext<DbContext, HomeworksContext>(
                o => o.UseInMemoryDatabase("HomeworksDB")
            );

            services.AddScoped<ILogic<Homework>, HomeworkLogic>();
            services.AddScoped<IRepository<Homework>, HomeworkRepository>();
            services.AddScoped<ILogic<User>, UserLogic>();
            services.AddScoped<IRepository<User>, UserRepository>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
