using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EsnafPlatformu.Web.Models;
using EsnafPlatformu.Web.ViewModels;
using EsnafPlatformu.Entities;


namespace EsnafPlatformu.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
{
    var menuItems = new List<MenuItem>
    {
        new MenuItem
        {
            Name = "Limonlu tavuk",
            Price = 280m,
            SortOrder = 20,
            IsVisible = true
        }
    };

    var themes = new List<ThemeCardViewModel>
{
    new ThemeCardViewModel
    {
        Name = "Ada",
        Description = "Geniş görseller ve ferah bir yerleşim."
    },
    new ThemeCardViewModel
    {
        Name = "Çizgi",
        Description = "Net başlıklar ve sade bir menü düzeni."
    }
};

return View(new HomeIndexViewModel
{
    MenuItems = menuItems,
    Themes = themes
});
}

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
