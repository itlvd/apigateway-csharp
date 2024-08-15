using Microsoft.AspNetCore.Mvc;

namespace Invoice.Controllers;

[Route("api/invoice")]
[ApiController]
public class InvoiceController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get()
    {

        var re = Request;
        await Task.Delay(100);
        return new string[] { "Dhananjay", "Nidhish", "Vijay", "Nazim", "Alpesh" };
    }
}