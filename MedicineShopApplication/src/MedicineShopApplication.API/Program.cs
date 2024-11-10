using Serilog;
using MedicineShopApplication.DLL;
using MedicineShopApplication.BLL;
using MedicineShopApplication.API.Middleware;
using MedicineShopApplication.API.StartupExtension;


var builder = WebApplication.CreateBuilder(args);

// Configure the Database
builder.Services.AddDatabaseExtensionHelper(builder.Configuration);

// Register the Identity
builder.Services.AddIdentityCustomExtensionHelper();

// Register the OpenIddict
builder.Services.AddOauth2ExtensionHelper();

// Register the Redis Caching
builder.Services.AddRedisExtensionHelper(builder.Configuration);

// Register the Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Register the Serilog
builder.Services.AddSerilogApplicationExtensionHelper(builder.Configuration);
builder.Host.UseSerilog();

// Register the DLL Dependencies
builder.Services.AddDLLDependency();

// Register the BLL Dependencies
builder.Services.AddBLLDependency();

// Register the controllers
builder.Services.AddControllers();

// Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtensionHelper();

// Allow CORS for the client app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowAngularApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.RunMigration();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAppAuthentication();

app.MapControllers();

app.Run();
