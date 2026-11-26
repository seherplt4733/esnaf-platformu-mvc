using EsnafPlatformu.Web.Models;
using EsnafPlatformu.Entities;

namespace EsnafPlatformu.Web.ViewModels;

public class HomeIndexViewModel
{
    public IReadOnlyList<MenuItem> MenuItems { get; init; } = [];
    public IReadOnlyList<ThemeCardViewModel> Themes { get; init; } = [];
}