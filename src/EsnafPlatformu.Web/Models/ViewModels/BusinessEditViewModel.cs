using System.ComponentModel.DataAnnotations;

namespace EsnafPlatformu.Web.ViewModels;

public class BusinessEditViewModel
{
    [Display(Name = "Kısa açıklama")]
    [StringLength(200, ErrorMessage = "Kısa açıklama en fazla 200 karakter olabilir.")]
    public string? ShortDescription { get; set; }

    [Display(Name = "Hakkımızda")]
    [StringLength(3000, ErrorMessage = "Hakkımızda metni en fazla 3000 karakter olabilir.")]
    public string? About { get; set; }

    [Display(Name = "Telefon")]
    [StringLength(30, ErrorMessage = "Telefon en fazla 30 karakter olabilir.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası gir.")]
    public string? Phone { get; set; }

    [Display(Name = "İletişim e-postası")]
    [StringLength(254, ErrorMessage = "E-posta en fazla 254 karakter olabilir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi gir.")]
    public string? ContactEmail { get; set; }
}