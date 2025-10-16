using System.ComponentModel.DataAnnotations;

namespace MiBlazorMudApp.Shared.Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }
    
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor de {1} caracteres")]
    [Display(Name = "Pais")]
    public string Name { get; set; } = null!;

    [MaxLength(10, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    [Display(Name = "Código")]
    public string? Code { get; set; }
    
    //relaciones
    public ICollection<State>? States { get; set; }
}
