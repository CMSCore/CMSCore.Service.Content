namespace CMSCore.Service.Content
{
    using System;
    using System.Threading.Tasks;
    using Library.Data.Models;
    using Library.GrainInterfaces;
    using Library.Messages;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Hosting;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IClusterClient>(CreateClusterClient);

            services.AddCors(cors => { cors.AddPolicy("client", builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowCredentials().Build()); });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "CMSCore.Service.Content", Version = "v1", Description = "Content delivery API for CMSCore" }); });
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMSCore.Service.Content"); });
            app.UseCors("client");

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetService<ILogger<Startup>>();

            var client = new ClientBuilder()
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(IReadContentGrain).Assembly).WithCodeGeneration();
                    parts.AddApplicationPart(typeof(FeedViewModel).Assembly).WithReferences();
                    //parts.AddApplicationPart(typeof(ReadContentRepository).Assembly).WithReferences();
                    //parts.AddApplicationPart(typeof(Page).Assembly).WithReferences();
                })
                //.Configure<ClusterOptions>(options =>
                //{
                //    options.ClusterId = "orleans-docker";
                //    options.ServiceId = "AspNetSampleApp";
                //})
                .UseLocalhostClustering()
                .Build();

            client.Connect(RetryFilter).GetAwaiter().GetResult();
            return client;

            async Task<bool> RetryFilter(Exception exception)
            {
                log?.LogWarning("Exception while attempting to connect to Orleans cluster: {Exception}", exception);
                await Task.Delay(TimeSpan.FromSeconds(2));
                return true;
            }
        }
    }
}