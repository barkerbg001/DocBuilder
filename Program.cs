using DocBuilder.Class;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Method 1: Accepts a class, returns a byte[]
app.MapPost("/method1", (ReportDto input) => 
{
    // Implement your logic here to process 'input' and convert to byte[]
    byte[] result = null;
    return Results.Ok(result);
});

// Method 2: Accepts a class, returns a list of byte[]
app.MapPost("/method2", (ReportDto input) => 
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

app.Run();
