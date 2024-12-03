using Domain.Exceptions;

namespace UseCase.Service.Exception;

public class ConfirmPaymentNotFoundException : DomainException
{
    private const string MessageTemplate = "The Payment with provider External Id: {0} was not found";
    private ConfirmPaymentNotFoundException(string providerExternalId) : base(string.Format(MessageTemplate, providerExternalId))
    {
        
    }

    public static void ThrowIfNull(object? value, string providerExternalId)
    {
        if (value == null)
        {
            throw new ConfirmPaymentNotFoundException(providerExternalId);
        }
    }
}
