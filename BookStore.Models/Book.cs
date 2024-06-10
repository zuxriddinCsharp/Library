using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookStore.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public string ISBN { get; set; }
    [Required]
    public string Author { get; set; }
    public int? GenreId { get; set; }
    [ForeignKey("GenreId")]
    [ValidateNever]
    public Genre? Genre { get; set; }
    [ValidateNever]
    public string ImageUrl { get; set; }
    public string?  FilePath { get; set; }
    public string? UserId { get; set; }
}
