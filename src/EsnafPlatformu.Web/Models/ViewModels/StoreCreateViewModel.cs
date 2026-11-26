using System.ComponentModel.DataAnnotations;

namespace EsnafPlatformu.Web.ViewModels;

public class StoreCreateViewModel
{
    [Display(Name = "Mağaza adı")]
    [Required(ErrorMessage = "Mağaza adını gir.")]
    [StringLength(100, ErrorMessage = "Mağaza adı en fazla 100 karakter olabilir.")]
    public string? Name { get; set; }

    [Display(Name = "Kategori")]
    [Required(ErrorMessage = "Bir kategori seç.")]
    public int? Category { get; set; }
}