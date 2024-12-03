using Microsoft.AspNetCore.Mvc;
using UseCase.Service.Interfaces;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;   
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Route("image")]
    public async Task<IActionResult> CreatePixAsync(int externalId, CancellationToken cancellationToken)
    {
        var response = await _paymentService.GetImage(externalId, cancellationToken);

        if(response == null)
        {
            return NotFound();
        }

        return File(response.Value.Data!, response.Value.ContentType!);
    }

}
