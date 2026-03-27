using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),o =>
{
    o.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    o.CommandTimeout(60);
}));

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddScoped<GreenhouseService>();
builder.Services.AddScoped<JobProcessor>();
builder.Services.AddScoped<NotificationService>();

builder.Services.AddHostedService<JobBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowAll");
app.Run();
