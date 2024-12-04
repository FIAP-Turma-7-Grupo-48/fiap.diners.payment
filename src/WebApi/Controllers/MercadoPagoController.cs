using Microsoft.AspNetCore.Mvc;
using UseCase.Service.Interfaces;
using WebApi.Dtos;

namespace WebApi.Controllers.Webhooks;

[Route("api/webhook/[controller]")]
[ApiController]
public class MercadoPagoController : ControllerBase
{
    private const string UPDATE_ACTION_VAlUE = "payment.updated";
    private readonly IPaymentService _paymentService;
    public MercadoPagoController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(MercadoPagoEventRequest mercadoPagoEvent, CancellationToken cancellationToken)
    {
        if (mercadoPagoEvent.Action.Equals(UPDATE_ACTION_VAlUE) is false)
        {
            return Ok();
        }

        await _paymentService.ConfirmPaymentAsync(mercadoPagoEvent.Data.Id, cancellationToken);

        return Ok();
    }
}
