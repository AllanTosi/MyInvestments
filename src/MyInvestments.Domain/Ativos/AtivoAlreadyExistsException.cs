using Volo.Abp;

namespace MyInvestments.Ativos;

public class AtivoAlreadyExistsException : BusinessException
{
    public AtivoAlreadyExistsException(string ticker)
        : base(MyInvestmentsDomainErrorCodes.AtivoAlreadyExists)
    {
        WithData("ticker", ticker);
    }
}
