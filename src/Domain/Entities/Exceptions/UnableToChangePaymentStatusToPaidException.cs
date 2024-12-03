using Domain.Entities.Enums;
using Domain.Exceptions;
using Helpers;

namespace Domain.Entities.Exceptions;

public class UnableToChangePaymentStatusToPaidException : DomainException
{
	private const string DEFAULT_MESSAGE_TEMPLATE =
		"Is not possible change a order when status is {0}";
	private static readonly IReadOnlyCollection<PaymentStatus> FinalStatus = new List<PaymentStatus>()
	{
        PaymentStatus.Creating,
        PaymentStatus.Paid,
		PaymentStatus.Cancelled
	};
	private UnableToChangePaymentStatusToPaidException(PaymentStatus status) : base(string.Format(DEFAULT_MESSAGE_TEMPLATE, status.GetEnumDescription()))
	{
		
	}

	public static void ThrowIfUnableToChangeStatus(PaymentStatus paymentStatus)
	{
		if(FinalStatus.Contains(paymentStatus))
		{
			throw new UnableToChangePaymentStatusToPaidException(paymentStatus);
		}
	}
}
