using Microsoft.EntityFrameworkCore;

using CustomerRegistration.Application.Common.Constants;
using CustomerRegistration.Application.Interfaces.Repository;
using CustomerRegistration.Application.Interfaces.Service;
using CustomerRegistration.Application.Security;
using CustomerRegistration.Application.Services;

using CustomerRegistration.Infrastructure.Data;
using CustomerRegistration.Infrastructure.Middlewares;
using CustomerRegistration.Infrastructure.Respositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IHashingService, HashingService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("CustomerManagementConnection"),
        ServerVersion.Parse("8.0.41")
    );
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseRouting();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .WithExposedHeaders(APP_HTTPHEADERS.CustomErrorCode, APP_HTTPHEADERS.CustomErrorMessage)
                .AllowCredentials());

app.MapControllers();

app.Run();