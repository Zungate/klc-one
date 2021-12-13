using Microsoft.AspNetCore.Mvc;

namespace klc_one.Controllers;
public class ErrorController : Controller
{
    public IActionResult Error404()
    {
        return View();
    }
}
