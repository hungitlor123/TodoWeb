using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Domains.Entities;

public class ToDo
{
    [Key]
    public int Id { get; set; }
    
    public string Description { get; set; }
    
    public bool IsCompeleted { get; set; }
}

//POST: 
//PUT: Idempotency
//Frontend =>>>>> Back end, Back end create data
//Frontend <<<<<= Back end