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
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
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


app.MapGet("/item/{id}", async (string id) =>
{
    var result = await supabase.From<Item>()
        .Where(x => x.Id == id)
        .Get();
    var item = result.Models;

    var eventsResult = await supabase.From<InventoryEvents>()
        .Where(x => x.AggregateId == id)
        .Get();
    var events = eventsResult.Models;

    ItemModel itemModel = item.Select(w => new ItemModel
    {
        Id = w.Id,
        CreatedAt = w.CreatedAt,
        DeletedAt = w.DeletedAt,
        Name = w.Name,
        Description = w.Description,
        Events = events.Select(e => new InventoryEventsModel
        {
            Id = e.Id,
            AggregateId = e.AggregateId,
            Quantity = e.Quantity,
            Reason = e.Reason,
            CreatedAt = e.CreatedAt
        }).ToList()
    }).FirstOrDefault();

    return itemModel;
})
.WithName("GetItem")
.WithOpenApi();

app.MapGet("/items", async () =>
{
    var result = await supabase.From<Item>().Get();
    var item = result.Models;

    var eventsResult = await supabase.From<InventoryEvents>().Get();
    var events = eventsResult.Models;

    List<ItemModel> itemModels = item.Select(w => new ItemModel
    {
        Id = w.Id,
        CreatedAt = w.CreatedAt,
        DeletedAt = w.DeletedAt,
        Name = w.Name,
        Description = w.Description,
        Quantity = events.Where(e => e.AggregateId == w.Id).Sum(e => e.Quantity),
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

app.MapPost("/create-event", async (NewEvent inventoryEvent) =>
{
    var model = new InventoryEvents
    {
        AggregateId = inventoryEvent.AggregateId,
        Quantity = inventoryEvent.Quantity,
        Reason = inventoryEvent.Reason,
        CreatedAt = DateTime.Now
    };

    await supabase.From<InventoryEvents>().Insert(model);

    return $"Inserted event {inventoryEvent.Reason}";
})
.WithName("CreateEvent")
.WithOpenApi();

app.UseCors(MyAllowSpecificOrigins);

app.Run();

public record NewItem(string Name, string? Description);
public record NewEvent(string AggregateId, int Quantity, string Reason);

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, string? Test)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
