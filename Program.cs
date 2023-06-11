using webapi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json",
        optional: false,
        reloadOnChange: true);

// Add HttpClient services.
builder.Services.AddHttpClient();

// Add your ApiSelector service.
builder.Services.AddScoped<IApiSelector, ApiSelector>();

// Add CORS services.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Replace this with the Angular app's origin
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS.
app.UseCors("AllowMyOrigin"); // This applies the CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
