using Domain.Entities.Enums;
using Domain.Exceptions;
using Helpers;

namespace Domain.Entities.Exceptions;

public class UnableToChangePaymentProiderIdException : DomainException
{
	private const string DEFAULT_MESSAGE_TEMPLATE =
		"Is not possible change a providerId when status is {0}";

	private UnableToChangePaymentProiderIdException(PaymentStatus status) : base(string.Format(DEFAULT_MESSAGE_TEMPLATE, status.GetEnumDescription()))
	{
		
	}

	public static void ThrowIfUnableToChangeStatus(PaymentStatus paymentStatus, PaymentStatus PaymentStatusAtualExpectd)
	{
		if(paymentStatus != PaymentStatusAtualExpectd)
		{
			throw new UnableToChangePaymentProiderIdException(paymentStatus);
		}
	}
}
