using System.ComponentModel.DataAnnotations;

namespace Moment3.Models;

public class CategoryModel{
    //properties
    public int Id { get; set; }
  [Required(ErrorMessage = "Ange en kategori")]
     [Display(Name = "Kategori")]
    public string? Name { get; set; }

    public List<BookModel>? BookModels {get; set; }
}