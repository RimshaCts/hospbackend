//using HospitalAppointment.Aspects;
//using HospitalAppointment.Auth;
//using HospitalAppointment.Repository;
//using HospitalAppointment.Service;
//using HospitalAppointment.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Http.Json;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace HospitalAppointment
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args); 

//            // Configure DbContext
//            builder.Services.AddDbContext<Appointment_BookingContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("Appointment_BookingContext")
//                    ?? throw new InvalidOperationException("Connection string 'Appointment_BookingContext' not found.")));

//            builder.Services.AddScoped<ExceptionHandlerAttribute>();

//            builder.Services.AddControllers(options =>
//            {
//                options.Filters.Add<ExceptionHandlerAttribute>();
//            })
//            .AddJsonOptions(options =>
//            {
//                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
//            });

//            // Swagger configuration
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            // Register repositories and services
//            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
//            builder.Services.AddScoped<IDoctorService, DoctorService>();
//            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
//            builder.Services.AddScoped<ILocationService, LocationService>();
//            builder.Services.AddScoped<IMedicalHistRepository, MedicalHistRepository>();
//            builder.Services.AddScoped<IMedicalHistService, MedicalHistService>();
//            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
//            builder.Services.AddScoped<IPatientService, PatientService>();
//            builder.Services.AddScoped<IUser, UserRepository>();
//            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
//            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
//            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
//            builder.Services.AddScoped<INotificationService, NotificationService>();
//            builder.Services.AddScoped<IUserService, UserService>();
//            builder.Services.AddScoped<IFAQRepository, FAQRepository>();
//            builder.Services.AddScoped<IFAQService, FAQService>();
//            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
//            builder.Services.AddScoped<IRatingService, RatingService>();
//            builder.Services.AddScoped<ITokenService, TokenService>();
//            builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
//            builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();



//            // Add JWT Authentication
//            var jwtSettings = builder.Configuration.GetSection("Jwt");

//            builder.Services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(options =>
//            {
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = jwtSettings["Issuer"],
//                    ValidAudience = jwtSettings["Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
//                };
//            });

//            builder.Services.Configure<JsonOptions>(options =>
//            {
//                options.SerializerOptions.PropertyNamingPolicy = null; // Keep original casing
//            });


//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("MycorsPolicy", builder =>
//                {
//                    builder.WithOrigins("http://localhost:3000")
//                           .AllowAnyMethod()
//                           .AllowAnyHeader();
//                });
//            });

//            builder.Services.AddAuthorization();

//            var app = builder.Build();
//            // Configure middleware
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }
//            app.UseDeveloperExceptionPage();
//            app.UseHttpsRedirection();
//            app.UseStaticFiles();
//            app.UseCors("MycorsPolicy");
//            app.UseAuthentication();
//            app.UseAuthorization();
//            app.MapControllers();
//            app.Run();
//        }
//    }
//}
using HospitalAppointment.Aspects;
using HospitalAppointment.Auth;
using HospitalAppointment.Repository;
using HospitalAppointment.Service;
using HospitalAppointment.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HospitalAppointment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure DbContext
            builder.Services.AddDbContext<Appointment_BookingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Appointment_BookingContext")
                    ?? throw new InvalidOperationException("Connection string 'Appointment_BookingContext' not found.")));

            builder.Services.AddScoped<ExceptionHandlerAttribute>();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionHandlerAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });

            // Swagger configuration with JWT
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hospital Appointment API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter 'Bearer' [space] and your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });

            // Register repositories and services
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IMedicalHistRepository, MedicalHistRepository>();
            builder.Services.AddScoped<IMedicalHistService, MedicalHistService>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IUser, UserRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFAQRepository, FAQRepository>();
            builder.Services.AddScoped<IFAQService, FAQService>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
            builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();

            // Add JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = null; // Keep original casing
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MycorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();
            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MycorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}