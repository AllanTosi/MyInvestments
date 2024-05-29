using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Operacoes;
using MyInvestments.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite;
using AutoMapper.Internal.Mappers;
using Microsoft.JSInterop;

namespace MyInvestments.Blazor.Pages;

public partial class Operacoes
{
    private IReadOnlyList<OperacaoDto> OperacaoList { get; set; }

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    private bool CanCreateOperacao { get; set; }
    private bool CanEditOperacao { get; set; }
    private bool CanDeleteOperacao { get; set; }

    private CreateOperacaoDto NewOperacao { get; set; }

    private Guid EditingOperacaoId { get; set; }
    private UpdateOperacaoDto EditingOperacao { get; set; }

    private Modal CreateOperacaoModal { get; set; }
    private Modal EditOperacaoModal { get; set; }

    private Validations CreateValidationsRef;

    private Validations EditValidationsRef;

    private Validations validationsRef;
    private DateTime SearchOperacao { get; set; } = DateTime.Today;

    IReadOnlyList<AtivoLookupDto> ativoList = Array.Empty<AtivoLookupDto>();

    public Operacoes()
    {
        NewOperacao = new CreateOperacaoDto();
        EditingOperacao = new UpdateOperacaoDto();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetOperacoesAsync();
        ativoList = (await OperacaoAppService.GetAtivoLookupAsync()).Items;
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateOperacao = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Operacoes.Create);

        CanEditOperacao = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Operacoes.Edit);

        CanDeleteOperacao = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Operacoes.Delete);
    }

    private async Task GetOperacoesAsync()
    {
        var result = await OperacaoAppService.GetListAsync(
            new GetOperacaoListDto
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting
            }
        );

        OperacaoList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<OperacaoDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetOperacoesAsync();

        await InvokeAsync(StateHasChanged);
    }

    private void OpenCreateOperacaoModal()
    {
        CreateValidationsRef.ClearAll();

        NewOperacao = new CreateOperacaoDto();
        CreateOperacaoModal.Show();
        //NewOperacao.AtivoId = ativoList.First().Id;
    }

    private void CloseCreateOperacaoModal()
    {
        CreateOperacaoModal.Hide();
    }

    private void OpenEditOperacaoModal(OperacaoDto operacao)
    {
        EditValidationsRef.ClearAll();

        EditingOperacaoId = operacao.Id;
        EditingOperacao = ObjectMapper.Map<OperacaoDto, UpdateOperacaoDto>(operacao);
        EditOperacaoModal.Show();
    }

    private async Task DeleteOperacaoAsync(OperacaoDto operacao)
    {
        var confirmMessage = L["AreYouSureToDelete", operacao.DataOperacao];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }

        await OperacaoAppService.DeleteAsync(operacao.Id);
        await GetOperacoesAsync();
    }

    private void CloseEditOperacaoModal()
    {
        EditOperacaoModal.Hide();
    }

    private async Task CreateOperacaoAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await OperacaoAppService.CreateAsync(NewOperacao);
            await GetOperacoesAsync();
            var addSuccessMessage = L["AddSuccessMessage"];
            await Message.Success(addSuccessMessage);
            await CreateOperacaoModal.Hide();
        }
    }

    private async Task UpdateOperacaoAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await OperacaoAppService.UpdateAsync(EditingOperacaoId, EditingOperacao);
            await GetOperacoesAsync();
            var updateSuccessMessage = L["UpdateSuccessMessage"];
            await Message.Success(updateSuccessMessage);
            await EditOperacaoModal.Hide();
        }
    }

    private async Task Search()
    {
        //if (await validationsRef.ValidateAll())
        //{
            var result = await OperacaoAppService.GetListByDataAsync(SearchOperacao.Date);
            OperacaoList = result;
            TotalCount = (int)result.Count;
        //}

        GetOperacoesAsync();
    }
    private async Task ExportToExcel()
    {
        var excelBytes = await ExportToExcelAppService.ExportToExcelOperacoes();
        await JsRuntime.InvokeVoidAsync("saveAsFile", $"Operacoes_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx",
            Convert.ToBase64String(excelBytes));
    }
}
