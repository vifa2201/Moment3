using System.ComponentModel.DataAnnotations;

namespace Moment3.Models;

public class LoanModel
{
   public int Id { get; set; }
    [Required(ErrorMessage = "Välj en bok")]
     [Display(Name = "Bok som lånas")]
    public int BookId { get; set; }
    public BookModel? Book { get; set; }
      [Required(ErrorMessage = "Ange en epostadress")]
    [Display(Name = "Användares epostadress")]
    public int UserId { get; set; }
    public UserModel? User { get; set; }
      [Required(ErrorMessage = "Ange ett datum")]
      [Display(Name = "Låne datum")]
    public DateTime LoanDate { get; set; }

}