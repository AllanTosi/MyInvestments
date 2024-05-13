using Volo.Abp;

namespace MyInvestments.ClasseAtivos;

public class ClasseAtivoAlreadyExistsException : BusinessException
{
    public ClasseAtivoAlreadyExistsException(string nome)
        : base(MyInvestmentsDomainErrorCodes.ClasseAtivoAlreadyExists)
    {
        WithData("nome", nome);
    }
}
