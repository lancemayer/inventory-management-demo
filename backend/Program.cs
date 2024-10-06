using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using dotenv.net;


DotEnv.Load();

var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabase = new Supabase.Client(url, key, options);
await supabase.InitializeAsync();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*");
                      });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/items", async () =>
{
    var result = await supabase.From<Item>().Get();
    var item = result.Models;

    List<ItemModel> itemModels = item.Select(w => new ItemModel
    {
        Id = w.Id,
        CreatedAt = w.CreatedAt,
        DeletedAt = w.DeletedAt,
        Name = w.Name,
        Description = w.Description
    }).ToList();

    return itemModels;
})
.WithName("GetItems")
.WithOpenApi();

app.MapPost("/create-item", async (NewItem item) =>
{
    var model = new Item
    {
        CreatedAt = DateTime.Now,
        DeletedAt = null,
        Name = item.Name,
        Description = item.Description
    };

    await supabase.From<Item>().Insert(model);

    return $"Inserted {item.Name}";
})
.WithName("CreateItem")
.WithOpenApi();

app.UseCors(MyAllowSpecificOrigins);

app.Run();

public record NewItem(string Name, string? Description);

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, string? Test)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
