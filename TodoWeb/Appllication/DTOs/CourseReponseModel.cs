using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.DTOs;

public class CourseReponseModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    
    public List<StudentViewModel> Students { get; set; } // 🛠 Đảm bảo kiểu danh sách DTO
    
    
}