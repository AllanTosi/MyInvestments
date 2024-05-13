using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyInvestments.Setores;

public class SetorManager : DomainService
{
    private readonly ISetorRepository _setorRepository;

    public SetorManager(ISetorRepository setorRepository)
    {
        _setorRepository = setorRepository;
    }

    public async Task<Setor> CreateAsync(
        string descricao)
    {
        Check.NotNullOrWhiteSpace(descricao, nameof(descricao));

        var existingSetor = await _setorRepository.FindByDescricaoAsync(descricao);
        if (existingSetor != null)
        {
            throw new SetorAlreadyExistsException(descricao);
        }

        return new Setor(
            GuidGenerator.Create(),
            descricao
        );
    }

    public async Task ChangeDescricaoAsync(
        Setor setor,
        string novaDescricao)
    {
        Check.NotNull(setor, nameof(setor));
        Check.NotNullOrWhiteSpace(novaDescricao, nameof(novaDescricao));

        var existingSetor = await _setorRepository.FindByDescricaoAsync(novaDescricao);
        if (existingSetor != null && existingSetor.Id != setor.Id)
        {
            throw new SetorAlreadyExistsException(novaDescricao);
        }

        setor.ChangeDescricao(novaDescricao);
    }
}
