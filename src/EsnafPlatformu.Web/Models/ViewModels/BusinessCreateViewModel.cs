using System.ComponentModel.DataAnnotations;

namespace EsnafPlatformu.Web.ViewModels;

public class BusinessCreateViewModel
{
    [Required(ErrorMessage = "İşletme adını gir.")]
    [StringLength(100, ErrorMessage = "İşletme adı en fazla 100 karakter olabilir.")]
    public string? BusinessName
    {
        get;
        set => field = value?.Trim();
    }

    [Required(ErrorMessage = "Site kısa adını gir.")]
    [StringLength(60, ErrorMessage = "Site kısa adı en fazla 60 karakter olabilir.")]
    [RegularExpression(
        @"[a-z0-9]+(?:-[a-z0-9]+)*",
        ErrorMessage = "Küçük İngilizce harf, rakam ve kelimeler arasında tek kısa çizgi kullan.")]
    public string? Slug
    {
        get;
        set => field = value?.Trim();
    }

    [StringLength(80, ErrorMessage = "Şehir en fazla 80 karakter olabilir.")]
    public string? City
    {
        get;
        set => field = value?.Trim();
    }

    [StringLength(80, ErrorMessage = "İlçe en fazla 80 karakter olabilir.")]
    public string? District
    {
        get;
        set => field = value?.Trim();
    }
}