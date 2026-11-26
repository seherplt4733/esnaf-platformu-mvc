using System.Security.Claims;
using EsnafPlatformu.Business;
using EsnafPlatformu.DTOs;
using EsnafPlatformu.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EsnafPlatformu.Web.Areas.BusinessPanel.Controllers;

[Authorize]
[Area("BusinessPanel")]
[Route("panel/isletme")]
public class BusinessesController(BusinessService businessService) : Controller
{
    [HttpGet("olustur")]
    public IActionResult Create()
    {
        return View(new BusinessCreateViewModel());
    }

    [HttpPost("olustur")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BusinessCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(ownerUserId))
        {
            return Challenge();
        }

        var input = new CreateBusinessDto(
            model.BusinessName!,
            model.Slug!,
            model.City,
            model.District);

        var result = await businessService.TryCreateAsync(
            input, ownerUserId, HttpContext.RequestAborted);

        if (result == BusinessCreateResult.OwnerAlreadyHasBusiness)
        {
            ModelState.AddModelError("", "Bu hesabın zaten bir işletmesi var.");
            return View(model);
        }

        if (result == BusinessCreateResult.SlugAlreadyUsed)
        {
            ModelState.AddModelError(
                nameof(model.Slug), "Bu site kısa adı zaten kullanılıyor.");
            return View(model);
        }

        TempData["BusinessFormSubmitted"] = true;
        return RedirectToAction(nameof(Success));
    }

    [HttpGet("olustur/basarili")]
    public IActionResult Success()
    {
        if (TempData["BusinessFormSubmitted"] is not true)
        {
            return RedirectToAction(nameof(Create));
        }

        return View();
    }


    [HttpGet("duzenle")]
public async Task<IActionResult> Edit(CancellationToken cancellationToken)
{
    var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        return Challenge();
    }

    var details = await businessService.GetProfileAsync(
        ownerUserId, cancellationToken);

    if (details is null)
    {
        return RedirectToAction(nameof(Create));
    }

    ViewData["Slug"] = details.Slug;

    return View(new BusinessEditViewModel
    {
        ShortDescription = details.Profile.ShortDescription,
        About = details.Profile.About,
        Phone = details.Profile.Phone,
        ContactEmail = details.Profile.ContactEmail
    });
}

[HttpPost("duzenle")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(
    BusinessEditViewModel model,
    CancellationToken cancellationToken)
{
    var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        return Challenge();
    }

    if (!ModelState.IsValid)
    {
        var details = await businessService.GetProfileAsync(
            ownerUserId, cancellationToken);

        if (details is null)
        {
            return RedirectToAction(nameof(Create));
        }

        ViewData["Slug"] = details.Slug;
        return View(model);
    }

    var input = new BusinessProfileDto(
        model.ShortDescription,
        model.About,
        model.Phone,
        model.ContactEmail);

    var saved = await businessService.UpdateProfileAsync(
        ownerUserId, input, cancellationToken);

    if (!saved)
    {
        return RedirectToAction(nameof(Create));
    }

    TempData["BusinessProfileSaved"] = "İşletme profili güncellendi.";
    return RedirectToAction(nameof(Edit));
}


    
}