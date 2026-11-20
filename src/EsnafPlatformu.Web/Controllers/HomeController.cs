using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EsnafPlatformu.Web.Models;


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

    return View(menuItems);
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
