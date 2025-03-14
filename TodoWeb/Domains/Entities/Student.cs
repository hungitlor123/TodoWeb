using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TodoWeb.Domains.Entities
{
    [Table("Students")]   
    
    public class Student : ISoftDelete
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        
        [MaxLength(255)]
        public string? FirstName { get; set; }

        [Column("Surname")]
        [MaxLength(255)]
        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        /*        [MaxLength(2000)]
                public byte[] Image { get; set; }*/
        
            /*[Timestamp]
        public byte[] RowVersion { get; set; }*/
        [ConcurrencyCheck]
        public decimal Balance { get; set; }
        
        public int Age { get; set; } 
        
        [ForeignKey("School")]
        public int SId { get; set; }
        
        [JsonIgnore]
        public virtual School School { get; set; }
        
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
        
        public int? DeletedBy { get; set; }
            
        public DateTime? DeletedAt { get; set; }
    }
}
