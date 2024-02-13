using System.ComponentModel.DataAnnotations;

namespace Moment3.Models;

public class AuthorModel
{
    //propetites
    public int Id { get; set; }

  [Required(ErrorMessage = "Ange författarens namn")]
    [Display(Name = "Författare")]
    public string? Name{ get; set; }


    public List<BookModel>? Books {get; set; }
}