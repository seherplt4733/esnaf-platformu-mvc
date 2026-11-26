using System.Globalization;
using System.Security.Claims;
using EsnafPlatformu.Business;
using EsnafPlatformu.DTOs;
using EsnafPlatformu.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EsnafPlatformu.Web.Areas.BusinessPanel.Controllers;

[Authorize]
[Area("BusinessPanel")]
[Route("panel/menu")]
public class MenuItemsController(MenuItemService menuItemService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(ownerUserId))
        {
            return Challenge();
        }

        return View(await menuItemService.ListAsync(ownerUserId, cancellationToken));
    }

    [HttpGet("ekle")]
    public IActionResult Create()
    {
        return View(new MenuItemFormViewModel());
    }

    [HttpPost("ekle")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(
    MenuItemFormViewModel model, CancellationToken cancellationToken)
{
    var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        return Challenge();
    }

    if (!TryReadFormPrice(model, out var price))
    {
        return View(model);
    }

    var input = new MenuItemDto(
        model.Name!.Trim(), price, model.SortOrder, model.IsVisible);

    var result = await menuItemService.AddAsync(
        ownerUserId, input, cancellationToken);

    switch (result)
    {
        case MenuItemWriteResult.Saved:
            return RedirectToAction(nameof(Index));

        case MenuItemWriteResult.NotFound:
            return RedirectToAction("Create", "Businesses",
                new { area = "BusinessPanel" });

        default:
            AddMenuValidationError(result);
            return View(model);
    }
}


    [HttpGet("{id:int}/duzenle")]
public async Task<IActionResult> Edit(
    [FromRoute] int id, CancellationToken cancellationToken)
{
    var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        return Challenge();
    }

    var item = await menuItemService.GetAsync(id, ownerUserId, cancellationToken);
    if (item is null)
    {
        return NotFound();
    }

    ViewData["MenuItemId"] = id;

    return View(new MenuItemFormViewModel
    {
        Name = item.Name,
        Price = item.Price.ToString("0.00", CultureInfo.GetCultureInfo("tr-TR")),
        SortOrder = item.SortOrder,
        IsVisible = item.IsVisible
    });
}

[HttpPost("{id:int}/duzenle")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(
    [FromRoute] int id,
    MenuItemFormViewModel model,
    CancellationToken cancellationToken)
{
    var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        return Challenge();
    }

    if (TryReadFormPrice(model, out var price))
    {
        var input = new MenuItemDto(
            model.Name!.Trim(), price, model.SortOrder, model.IsVisible);

        var result = await menuItemService.UpdateAsync(
            id, ownerUserId, input, cancellationToken);

        switch (result)
        {
            case MenuItemWriteResult.Saved:
                TempData["MenuMessage"] = "Menü öğesi güncellendi.";
                return RedirectToAction(nameof(Index));

            case MenuItemWriteResult.NotFound:
                return NotFound();

            default:
                AddMenuValidationError(result);
                break;
        }
    }

    if (await menuItemService.GetAsync(id, ownerUserId, cancellationToken) is null)
    {
        return NotFound();
    }

    ViewData["MenuItemId"] = id;
    return View(model);
}

[HttpPost("{id:int}/sil")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Delete(
    [FromRoute] int id, CancellationToken cancellationToken)
{
    var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        return Challenge();
    }

    if (!await menuItemService.DeleteAsync(id, ownerUserId, cancellationToken))
    {
        return NotFound();
    }

    TempData["MenuMessage"] = "Menü öğesi silindi.";
    return RedirectToAction(nameof(Index));
}

private bool TryReadFormPrice(MenuItemFormViewModel model, out decimal price)
{
    price = 0;

    if (!ModelState.IsValid)
    {
        return false;
    }

    if (decimal.TryParse(
            model.Price,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.GetCultureInfo("tr-TR"),
            out price))
    {
        return true;
    }

    ModelState.AddModelError(nameof(model.Price), "Fiyat sıfırdan büyük olmalı.");
    return false;
}

private void AddMenuValidationError(MenuItemWriteResult result)
{
    switch (result)
    {
        case MenuItemWriteResult.InvalidPrice:
            ModelState.AddModelError(
                nameof(MenuItemFormViewModel.Price),
                "Fiyat sıfırdan büyük olmalı.");
            break;

        case MenuItemWriteResult.InvalidSortOrder:
            ModelState.AddModelError(
                nameof(MenuItemFormViewModel.SortOrder),
                "Sıra numarası negatif olamaz.");
            break;

        default:
            throw new ArgumentOutOfRangeException(nameof(result), result, null);
    }
}
}
