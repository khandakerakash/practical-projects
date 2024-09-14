using MedicineShopApplication.DLL;
using MedicineShopApplication.BLL;
using MedicineShopApplication.API.StartupExtension;

var builder = WebApplication.CreateBuilder(args);

// Configure the Database
builder.Services.AddDatabaseExtensionHelper(builder.Configuration);

// Register the DLL Dependencies
builder.Services.AddDLLDependency();

// Register the BLL Dependencies
builder.Services.AddBLLDependency();


// Register the controllers
builder.Services.AddControllers();

// Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.RunMigration();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
