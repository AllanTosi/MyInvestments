using Volo.Abp;

namespace MyInvestments.TipoTransacoes;

public class TipoTransacaoAlreadyExistsException : BusinessException
{
    public TipoTransacaoAlreadyExistsException(string descricao)
        : base(MyInvestmentsDomainErrorCodes.TipoTransacaoAlreadyExists)
    {
        WithData("descricao", descricao);
    }
}
