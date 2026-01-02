using NetReach.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddHttpClient<CryptomusService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<EmailService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://net-reach.vercel.app",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ❌ امسح UseHttpsRedirection
// app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
