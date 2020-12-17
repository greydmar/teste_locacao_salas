using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mtgroup.auth.Interfaces;
using mtgroup.locacao.Auxiliares;
using mtgroup.locacao.Servicos;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace mtgroup.locacao
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    ConfigSerializacaoJson.Setup(options.JsonSerializerOptions);
                });

            services
                .AddAuthentication(options =>
                {
                    
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtParameters = Configuracoes.Auth.ValidationParameters;
                    options.Audience = jwtParameters.ValidAudience;
                    options.AutomaticRefreshInterval = TimeSpan.FromMinutes(50); /* move to config*/
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = jwtParameters;
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },

                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicyUsuarioAutenticado();
            });

            services.AddHttpContextAccessor();
            
            // configura injeção dependência para os demais serviços
            ConfigureModules(services);

            ConfigureDocumentation(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //app.UseMiddleware<MiddlewareTokenJwt>();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                options.DocExpansion(DocExpansion.List);
                ////foreach (var description in options. provider.ApiVersionDescriptions)
                ////{
                ////    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                ////}

                //options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Api Testes");
                //options.RoutePrefix = string.Empty;
            });
        }
    }
}
