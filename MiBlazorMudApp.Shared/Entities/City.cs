using System.ComponentModel.DataAnnotations;

namespace MiBlazorMudApp.Shared.Entities;

public class City
{
    [Key]
    public int CityId { get; set; }
    
    public int StateId { get; set; }
    
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor de {1} caracteres")]
    [Display(Name = "Ciudad")]
    public string Name { get; set; } = null!;
    
    //relaciones
    public State? State { get; set; }
}