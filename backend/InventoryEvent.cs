using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("inventory_events")]
class InventoryEvents : BaseModel
{
	[PrimaryKey("id", false)]
	public string Id { get; set; }

	[Column("created_at")]
	public DateTime CreatedAt { get; set; }

	[Column("aggregate_id")]
	public string AggregateId { get; set; }

	[Column("quantity")]
	public int Quantity { get; set; }

	[Column("reason")]
	public string Reason { get; set; }
}

class InventoryEventsModel
{
	public string Id { get; set; }
	public string AggregateId { get; set; }
	public int Quantity { get; set; }
	public string Reason { get; set; }
	public DateTime CreatedAt { get; set; }
}
