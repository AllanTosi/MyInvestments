using Volo.Abp;

namespace MyInvestments.Setores;

public class SetorAlreadyExistsException : BusinessException
{
    public SetorAlreadyExistsException(string descricao)
        : base(MyInvestmentsDomainErrorCodes.SetorAlreadyExists)
    {
        WithData("descricao", descricao);
    }
}
