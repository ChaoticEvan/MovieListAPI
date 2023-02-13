using Microsoft.OpenApi.Models;

namespace MovieListAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        public void Configure()
        {
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            { 
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MovieListAPI",
                    Version = "v1",
                    Description = "API Used for Movie List React app",
                    Contact = new OpenApiContact
                    {
                        Name = "Evan Voordeckers",
                        Email= "evan.voordeckers@gmail.com",
                        Url = new Uri("https://github.com/ChaoticEvan")
                    }
                });
            });
            builder.Services.AddHttpClient();

            var app = builder.Build();
            app.UseSwagger();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
