using System.Security.Claims;
using EsnafPlatformu.Business;
using EsnafPlatformu.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EsnafPlatformu.Web.Areas.BusinessPanel.Controllers;

[Authorize]
[Area("BusinessPanel")]
[Route("panel/magazalar")]
public class StoresController(StoreService storeService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(ownerUserId))
        {
            return Challenge();
        }

        var stores = await storeService.ListAsync(ownerUserId, cancellationToken);
        return View(stores);
    }

    [HttpGet("{storeId:int}/yonet")]
public async Task<IActionResult> Manage(
    int storeId, CancellationToken cancellationToken)
{
    var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        return Challenge();
    }

    var store = await storeService.GetAsync(
        storeId, ownerUserId, cancellationToken);

    if (store is null)
    {
        return NotFound();
    }

    return View(store);
}

    [HttpGet("ekle")]
    public IActionResult Create()
    {
        return View(new StoreCreateViewModel());
    }

    [HttpPost("ekle")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        StoreCreateViewModel model, CancellationToken cancellationToken)
    {
        var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(ownerUserId))
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await storeService.AddAsync(
            ownerUserId, model.Name, model.Category, cancellationToken);

        switch (result)
        {
            case StoreCreateResult.Created:
                TempData["StoreMessage"] = "Mağaza eklendi.";
                return RedirectToAction(nameof(Index));

            case StoreCreateResult.BusinessNotFound:
                return RedirectToAction("Create", "Businesses",
                    new { area = "BusinessPanel" });

            case StoreCreateResult.InvalidName:
                ModelState.AddModelError(nameof(model.Name),
                    "Mağaza adı 1 ile 100 karakter arasında olmalı.");
                break;

            case StoreCreateResult.InvalidCategory:
                ModelState.AddModelError(nameof(model.Category),
                    "Geçerli bir kategori seç.");
                break;
        }

        return View(model);
    }
}