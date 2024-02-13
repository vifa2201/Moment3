using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moment3.Models;

public class BookModel
{
    //Properties
    public int Id { get; set; } //primary key

    [Required(ErrorMessage = "Ange en titel")]
    [Display(Name = "Titel")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Ange en beskrivning")]
    [Display(Name = "Beskrivning")]
    public string? Description { get; set; }
  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  [Required(ErrorMessage = "Ange ett datum")]
    [Display(Name = "Publiceringsdatum")]
     public DateTime? PublishDate { get; set; }

    public string? ImageName { get; set; } //lagra i databasen
    [NotMapped]

    public IFormFile? ImageFile { get; set; }  //lagra i gränsnitt



    //relationer referens
 [Display(Name = "Kategori")]
    public int? CategoryId {get; set;}
  
    public CategoryModel? Category {get; set;}
 [Display(Name = "Författare")]
    public int AuthorId {get; set;}

    public AuthorModel? Author {get; set;}



}