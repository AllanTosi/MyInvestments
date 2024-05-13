using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MyInvestments.ClasseAtivos;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyInvestments.ClasseAtivos;

public class ClasseAtivoManager : DomainService
{
    private readonly IClasseAtivoRepository _classeAtivoRepository;

    public ClasseAtivoManager(IClasseAtivoRepository classeAtivoRepository)
    {
        _classeAtivoRepository = classeAtivoRepository;
    }

    public async Task<ClasseAtivo> CreateAsync(
        string nome)
    {
        Check.NotNullOrWhiteSpace(nome, nameof(nome));

        var existingClasseAtivo = await _classeAtivoRepository.FindByNomeAsync(nome);
        if (existingClasseAtivo != null)
        {
            throw new ClasseAtivoAlreadyExistsException(nome); 
        }

        return new ClasseAtivo(
            GuidGenerator.Create(),
            nome
        );
    }

    public async Task ChangeNomeAsync(
        ClasseAtivo classeAtivo,
        string novoNome)
    {
        Check.NotNull(classeAtivo, nameof(classeAtivo));
        Check.NotNullOrWhiteSpace(novoNome, nameof(novoNome));

        var existingClasseAtivo = await _classeAtivoRepository.FindByNomeAsync(novoNome);
        if (existingClasseAtivo != null && existingClasseAtivo.Id != classeAtivo.Id)
        {
            throw new ClasseAtivoAlreadyExistsException(novoNome);
        }

        classeAtivo.ChangeNome(novoNome);
    }
}
