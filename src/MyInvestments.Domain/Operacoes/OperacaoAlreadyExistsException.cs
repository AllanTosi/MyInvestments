using Volo.Abp;

namespace MyInvestments.Operacoes;

public class OperacaoAlreadyExistsException : BusinessException
{
    //public OperacaoAlreadyExistsException(string name)
    //    : base(MyInvestmentsDomainErrorCodes.OperacaoAlreadyExists)
    //{
    //    WithData("name", name);
    //}
}
