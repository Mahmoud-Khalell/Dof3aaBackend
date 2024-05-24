using Core.Context;
using Core.entities;
using InfraStructure_Layer.Interfaces;
using InfraStructure_Layer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApi.Services;
using ServiceLayer.AnnouncementService;
using ServiceLayer.Authservice;
using ServiceLayer.CourceService;
using ServiceLayer.DocumentServices;
using ServiceLayer.NotificationService;
using ServiceLayer.TaskService;
using ServiceLayer.TopicService;
using System.Text;

namespace PresentationLayer

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddswaggerDoc();
            
            builder.Services.AddDbContext<Connector>(
                option =>
                    {
                        option.UseSqlServer(builder.Configuration.GetConnectionString("Connector"));
                    }
                );
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options => {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     
                     IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                     ValidateIssuer = false,
                     ValidateLifetime=false,
                     ValidateAudience = false
                 };
     });
            builder.Services.AddAuthorization();
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider= "Default";
            }).AddEntityFrameworkStores<Connector>().AddDefaultTokenProviders();
            
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof( GenericRepository<>));
            builder.Services.AddScoped<InfraStructure_Layer.Interfaces.IUnitOfWork, InfraStructure_Layer.Repository.UnitOfWork>();
            builder.Services.AddScoped<ICourceService, CourceService>();
            builder.Services.AddScoped<IauthService, Authservice>();
            builder.Services.AddScoped<HttpContextAccessor>();
            builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddCors(e =>
                {
                    e.AddPolicy("MyPloicy", policybuilder =>  policybuilder.AllowAnyOrigin().AllowAnyHeader());
                }
            );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
             app.UseCors("MyPloicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
