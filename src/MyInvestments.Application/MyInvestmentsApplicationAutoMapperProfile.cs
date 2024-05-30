using AutoMapper;
using MyInvestments.Ativos;
using MyInvestments.ClasseAtivos;
using MyInvestments.Operacoes;
using MyInvestments.Setores;
using MyInvestments.TipoTransacoes;

namespace MyInvestments;

public class MyInvestmentsApplicationAutoMapperProfile : Profile
{
    public MyInvestmentsApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Ativo, AtivoDto>();
        CreateMap<ClasseAtivo, ClasseAtivoDto>();
        CreateMap<Operacao, OperacaoDto>();
        CreateMap<Setor, SetorDto>();
        CreateMap<TipoTransacao, TipoTransacaoDto>();

        //Adiciona Relacionamento
        CreateMap<ClasseAtivo, ClasseAtivoLookupDto>();
        CreateMap<Setor, SetorLookupDto>();
        CreateMap<Ativo, AtivoLookupDto>();
        CreateMap<TipoTransacao, TipoTransacaoLookupDto>();

    }
}
