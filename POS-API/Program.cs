using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS_API.Controllers;
using POS_API.Data;
using POS_API.Data.Repositories;
using POS_API.Interfaces;
using POS_API.Services;

namespace POS_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      // Ini cara termudah untuk development
                                      // Mengizinkan origin, method, dan header APAPUN
                                      policy.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();

                                      // --- CATATAN ---
                                      // Untuk produksi, lebih aman seperti ini:
                                      //policy.WithOrigins("http://localhost:5500") // Port dari Live Server
                                      //       .AllowAnyHeader()
                                      //       .AllowAnyMethod();
                                  });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));



            // Add services to the container.

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            string Base64UrlToBase64(string base64url)
            {
                string output = base64url.Replace('-', '+').Replace('_', '/');
                switch (output.Length % 4)
                {
                    case 2: output += "=="; break;
                    case 3: output += "="; break;
                }

                return output;
            }


            var secretKey = builder.Configuration["JwtSettings:SecretKey"];



            var normalBase64 = Base64UrlToBase64(secretKey!);

            var keyBytes = Convert.FromBase64String(normalBase64);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();


            app.UseCors(MyAllowSpecificOrigins);

            app.MapControllers();

            app.Run();
        }
    }
}
