using Microsoft.AspNetCore.Mvc;
using FodraszApplication.Models;
using System.Diagnostics;

public class HibaController : Controller
{
    [Route("Hiba")]
    public IActionResult Index()
    {
        return View("~/Views/Shared/Error.cshtml",
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
