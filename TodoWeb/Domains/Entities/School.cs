using System.Text.Json.Serialization;

namespace TodoWeb.Domains.Entities;

public class School
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Student> Students { get; set; }
}