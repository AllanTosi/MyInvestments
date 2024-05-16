using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyInvestments.Ativos;

public class AtivoManager : DomainService
{
    private readonly IAtivoRepository _ativoRepository;

    public AtivoManager(IAtivoRepository authorRepository)
    {
        _ativoRepository = authorRepository;
    }

    public async Task<Ativo> CreateAsync(
        string ticker,
        string nome,
        Guid setorId,
        Guid classeAtivoId,
        string? descricao = null
        )
    {
        Check.NotNullOrWhiteSpace(ticker, nameof(ticker));
        Check.NotNullOrWhiteSpace(nome, nameof(nome));

        // Falta Verificar o nome também
        var existingAtivo = await _ativoRepository.FindByTickerAsync(ticker);
        if (existingAtivo != null)
        {
            throw new AtivoAlreadyExistsException(ticker);
        }

        return new Ativo(
            GuidGenerator.Create(),
            ticker,
            nome,
            setorId,
            classeAtivoId,
            descricao
        );
    }

    public async Task ChangeTickerAsync(
        Ativo ticker,
        string novoTicker)
    {
        Check.NotNull(ticker, nameof(ticker));
        Check.NotNullOrWhiteSpace(novoTicker, nameof(novoTicker));

        var existingAtivo = await _ativoRepository.FindByTickerAsync(novoTicker);
        if (existingAtivo != null && existingAtivo.Id != ticker.Id)
        {
            throw new AtivoAlreadyExistsException(novoTicker);
        }

        ticker.ChangeTicker(novoTicker);
    }

    public async Task ChangeNomeAsync(
    Ativo nome,
    string novoNome)
    {
        Check.NotNull(nome, nameof(nome));
        Check.NotNullOrWhiteSpace(novoNome, nameof(novoNome));

        var existingAtivo = await _ativoRepository.FindByNomeAsync(novoNome);
        if (existingAtivo != null && existingAtivo.Id != nome.Id)
        {
            throw new AtivoAlreadyExistsException(novoNome);
        }

        nome.ChangeNome(novoNome);
    }
}
