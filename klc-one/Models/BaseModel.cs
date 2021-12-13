using System.ComponentModel.DataAnnotations;

namespace klc_one.Models;

public class BaseModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Navn")]
    public string Name { get; set; }

    [Display(Name = "Oprettet")]
    [DisplayFormat(DataFormatString = "{0:f}")]
    public DateTime? CreatedAt { get; set; }
    [Display(Name = "Opdateret")]
    [DisplayFormat(DataFormatString = "{0:f}")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    [Display(Name = "Slettet")]
    [DisplayFormat(DataFormatString = "{0:f}")]
    public DateTime? DeletedAt { get; set; }
}
