using Microsoft.AspNetCore.Authorization;
using MyInvestments.Ativos;
using MyInvestments.ClasseAtivos;
using MyInvestments.Operacoes;
using MyInvestments.Permissions;
using MyInvestments.Setores;
using MyInvestments.TipoTransacoes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;
using static MyInvestments.ExcelExport.ExcelFileGenerator;

namespace MyInvestments.ExcelExport
{
    public class ExportToExcelAppService : ApplicationService, IExportToExcelAppService
    {
        protected IAtivoRepository _ativoRepository;
        protected ISetorRepository _setorRepository;
        protected IClasseAtivoRepository _classeAtivoRepository;
        protected ITipoTransacaoRepository _transacaoRepository;
        protected IOperacaoRepository _operacaoRepository;

        public ExportToExcelAppService(
            IAtivoRepository ativoRepository,
            ISetorRepository setorRepositor,
            IClasseAtivoRepository classeAtivoRepository,
            ITipoTransacaoRepository transacaoRepository,
            IOperacaoRepository operacaoRepository

        )
        {
            _ativoRepository = ativoRepository;
            _setorRepository = setorRepositor;
            _classeAtivoRepository = classeAtivoRepository;
            _transacaoRepository = transacaoRepository;
            _operacaoRepository = operacaoRepository;
        }

        public Task<byte[]> ExportToExcel() => Task.FromResult(GenerateExcelFile());

        public async Task<byte[]> ExportToExcelAtivos()
        {
            await CheckPolicyAsync(MyInvestmentsPermissions.Ativos.Default);

            var lAtivos = await _ativoRepository.GetListWithRelationshipAsync();

            return GenerateExcelFileAtivos(lAtivos);
        }

        public async Task<byte[]> ExportToExcelTipoTransacoes()
        {
            await CheckPolicyAsync(MyInvestmentsPermissions.TipoTransacoes.Default);

            var lTipoTransacoes = await _transacaoRepository.GetListAsync();

            return GenerateExcelFileTipoTransacoes(lTipoTransacoes);
        }

        public async Task<byte[]> ExportToExcelSetores()
        {
            await CheckPolicyAsync(MyInvestmentsPermissions.Setores.Default);

            var lSetores = await _setorRepository.GetListAsync();

            return GenerateExcelFileSetores(lSetores);
        }

        public async Task<byte[]> ExportToExcelClasseAtivos()
        {
            await CheckPolicyAsync(MyInvestmentsPermissions.ClasseAtivos.Default);

            var lClasseAtivos = await _classeAtivoRepository.GetListAsync();

            return GenerateExcelFileClasseAtivos(lClasseAtivos);
        }

        public async Task<byte[]> ExportToExcelOperacoes()
        {
            await CheckPolicyAsync(MyInvestmentsPermissions.Operacoes.Default);

            //quando fizer os relacionamentos tera que presonalizar igual ao Ativos
            var lOperacao = await _operacaoRepository.GetListWithRelationshipAsync();

            return GenerateExcelFileOperacoes(lOperacao);
        }
    }
}