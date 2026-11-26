using System.ComponentModel.DataAnnotations;

namespace EsnafPlatformu.Web.ViewModels;

public class MenuItemFormViewModel
{
    [Display(Name = "Menü öğesi adı")]
    [Required(ErrorMessage = "Menü öğesinin adını gir.")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir.")]
    public string? Name { get; set; }

    [Display(Name = "Fiyat (TL)")]
    [Required(ErrorMessage = "Fiyatı gir.")]
    [RegularExpression(@"[0-9]{1,8}(,[0-9]{1,2})?",
        ErrorMessage = "Fiyatı 280 veya 280,50 biçiminde gir.")]
    public string? Price { get; set; }

    [Display(Name = "Sıra")]
    [Range(0, int.MaxValue, ErrorMessage = "Sıra negatif olamaz.")]
    public int SortOrder { get; set; } = 10;

    [Display(Name = "Menüde göster")]
    public bool IsVisible { get; set; } = true;
}