namespace TodoWeb.Domains.Entities;

public interface ISoftDelete
{
    int? DeletedBy { get; set; } 
    DateTime? DeletedAt { get; set; }
}