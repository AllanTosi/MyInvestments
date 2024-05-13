using AutoMapper;
using MyInvestments.Ativos;
using MyInvestments.ClasseAtivos;
using MyInvestments.Operacoes;
using MyInvestments.Setores;
using MyInvestments.TipoTransacoes;

namespace MyInvestments.Blazor;

public class MyInvestmentsBlazorAutoMapperProfile : Profile
{
    public MyInvestmentsBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.

        CreateMap<OperacaoDto, UpdateOperacaoDto>();
        CreateMap<AtivoDto, UpdateAtivoDto>();
        CreateMap<ClasseAtivoDto, UpdateClasseAtivoDto>();
        CreateMap<SetorDto, UpdateSetorDto>();
        CreateMap<TipoTransacaoDto, UpdateTipoTransacaoDto>();


    }
}
