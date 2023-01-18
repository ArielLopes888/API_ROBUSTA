using AutoMapper;
using Infra.Context;
using Infra.Interfaces;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services.Interfaces;
using Domain.Entities;
using Services.DTO;
using API.ROBUSTA.ViewModels;
using Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.ROBUSTA.Token;

namespace API.ROBUSTA
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

            services.AddControllers();
            services.AddSingleton(cfg => Configuration);

            #region Jwt

            var secretKey = Configuration["Jwt:Key"];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #endregion

            #region AutoMapper

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<CreateUserViewModel, UserDto>().ReverseMap();
                cfg.CreateMap<UpdateUserViewModel, UserDto>().ReverseMap();
            });

            services.AddSingleton(autoMapperConfig.CreateMapper());

            #endregion

            #region Database
            services.AddDbContext<ManagerContext>(options => options
                .UseSqlServer(Configuration["ConnectionStrings:API.ROBUSTA"])
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())),
            ServiceLifetime.Transient);

            #endregion

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();

            #endregion

            #region Services

            services.AddScoped<IUserService, UserServices>();

            services.AddScoped<ITokenGenerator, TokenGenerator>();

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "AL2",
                    Title = "API ROBUSTA",
                    Description = "Aprendendo a construir uma API robusta e moderna utilizando .NET. Curso do Lucas Eschechola.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ariel Lisboa Lopes",
                        Email = "ariellopes888@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/ariel-lisboa-lopes-6ba7a7168")
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor utilize Bearer <TOKEN>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });

            #endregion

            #region Hash

            //var config = new Argon2Config
            //{
            //    Type = Argon2Type.DataIndependentAddressing,
            //    Version = Argon2Version.Nineteen,
            //    TimeCost = int.Parse(Configuration["Hash:TimeCost"]),
            //    MemoryCost = int.Parse(Configuration["Hash:MemoryCost"]),
            //    Lanes = int.Parse(Configuration["Hash:Lanes"]),
            //    Threads = Environment.ProcessorCount,
            //    Salt = Encoding.UTF8.GetBytes(Configuration["Hash:Salt"]),
            //    HashLength = int.Parse(Configuration["Hash:HashLength"])
            //};

            //services.AddArgon2IdHasher(config);

            //#endregion

            //#region Mediator

            //services.AddMediatR(typeof(Startup));
            //services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            //services.AddScoped<IMediatorHandler, MediatorHandler>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manager.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseCustomExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

