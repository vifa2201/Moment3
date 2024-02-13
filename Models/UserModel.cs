using System.ComponentModel.DataAnnotations;

namespace Moment3.Models;



public class UserModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Ange ett namn")]
    [Display(Name = "Namn")]
    public string? Name { get; set; }
      [Required(ErrorMessage = "Ange en epostadress")]
     [Display(Name = "Epostadress")]
    public string? Email { get; set; }
      public List<LoanModel>? Loans { get; set; } // En lista över lån som tillhör användaren
    
}
