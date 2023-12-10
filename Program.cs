using System.Reflection;
using DocBuilder.Class;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Method 1: Accepts a class, returns a byte[]
app.MapPost("/generatePdf", (ReportDto input) => 
{
    // Implement your logic here to process 'input' and convert to byte[]
    byte[] result = null;
    return Results.Ok(result);
});

// Method 2: Accepts a class, returns a list of byte[]
app.MapPost("/generateDocumentImages", (ReportDto input) => 
{
    // Implement your logic here to process 'input' and convert to List<byte[]>
    List<byte[]> resultList = new List<byte[]>();
    return Results.Ok(resultList);
});

// Method 3: Accepts a class, returns another class
app.MapPost("/method3", (ReportDto input) => 
{
    // Implement your logic here to process 'input' and convert to OutputClass
    DocumentBuilder output = new DocumentBuilder();
    return Results.Ok(output);
});

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();
