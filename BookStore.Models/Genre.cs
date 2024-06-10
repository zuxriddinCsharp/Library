using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public class Genre  
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("Genre")]
    [MaxLength(30)]
    public string Name { get; set; }
}
