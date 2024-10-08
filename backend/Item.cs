using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("item")]
class Item : BaseModel
{
	[PrimaryKey("id", false)]
	public string Id { get; set; }

	[Column("created_at")]
	public DateTime CreatedAt { get; set; }

	[Column("deleted_at")]
	public DateTime? DeletedAt { get; set; }

	[Column("name")]
	public string Name { get; set; }

	[Column("description")]
	public string? Description { get; set; }
}

class ItemModel
{
	public string Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? DeletedAt { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public List<InventoryEventsModel> Events { get; set; }
	public int Quantity { get; set; }
}
