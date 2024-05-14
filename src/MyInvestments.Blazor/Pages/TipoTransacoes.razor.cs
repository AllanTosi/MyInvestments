using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.TipoTransacoes;
using MyInvestments.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite;

namespace MyInvestments.Blazor.Pages;

public partial class TipoTransacoes
{
    private IReadOnlyList<TipoTransacaoDto> TipoTransacaoList { get; set; }

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    private bool CanCreateTipoTransacao { get; set; }
    private bool CanEditTipoTransacao { get; set; }
    private bool CanDeleteTipoTransacao { get; set; }

    private CreateTipoTransacaoDto NewTipoTransacao { get; set; }

    private Guid EditingTipoTransacaoId { get; set; }
    private UpdateTipoTransacaoDto EditingTipoTransacao { get; set; }

    private Modal CreateTipoTransacaoModal { get; set; }
    private Modal EditTipoTransacaoModal { get; set; }

    private Validations CreateValidationsRef;

    private Validations EditValidationsRef;

    private Validations validationsRef;
    private String SearchTipoTransacao { get; set; }


    public TipoTransacoes()
    {
        NewTipoTransacao = new CreateTipoTransacaoDto();
        EditingTipoTransacao = new UpdateTipoTransacaoDto();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetTipoTransacoesAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateTipoTransacao = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.TipoTransacoes.Create);

        CanEditTipoTransacao = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.TipoTransacoes.Edit);

        CanDeleteTipoTransacao = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.TipoTransacoes.Delete);
    }

    private async Task GetTipoTransacoesAsync()
    {
        var result = await TipoTransacaoAppService.GetListAsync(
            new GetTipoTransacaoListDto
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting
            }
        );

        TipoTransacaoList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TipoTransacaoDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetTipoTransacoesAsync();

        await InvokeAsync(StateHasChanged);
    }

    private void OpenCreateTipoTransacaoModal()
    {
        CreateValidationsRef.ClearAll();

        NewTipoTransacao = new CreateTipoTransacaoDto();
        CreateTipoTransacaoModal.Show();
    }

    private void CloseCreateTipoTransacaoModal()
    {
        CreateTipoTransacaoModal.Hide();
    }

    private void OpenEditTipoTransacaoModal(TipoTransacaoDto tipoTransacao)
    {
        EditValidationsRef.ClearAll();

        EditingTipoTransacaoId = tipoTransacao.Id;
        EditingTipoTransacao = ObjectMapper.Map<TipoTransacaoDto, UpdateTipoTransacaoDto>(tipoTransacao);
        EditTipoTransacaoModal.Show();
    }

    private async Task DeleteTipoTransacaoAsync(TipoTransacaoDto tipoTransacao)
    {
        var confirmMessage = L["AreYouSureToDelete", tipoTransacao.Descricao];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }

        await TipoTransacaoAppService.DeleteAsync(tipoTransacao.Id);
        await GetTipoTransacoesAsync();
    }

    private void CloseEditTipoTransacaoModal()
    {
        EditTipoTransacaoModal.Hide();
    }

    private async Task CreateTipoTransacaoAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await TipoTransacaoAppService.CreateAsync(NewTipoTransacao);
            await GetTipoTransacoesAsync();
            var addSuccessMessage = L["AddSuccessMessage"];
            await Message.Success(addSuccessMessage);
            await CreateTipoTransacaoModal.Hide();
        }
    }

    private async Task UpdateTipoTransacaoAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await TipoTransacaoAppService.UpdateAsync(EditingTipoTransacaoId, EditingTipoTransacao);
            await GetTipoTransacoesAsync();
            var updateSuccessMessage = L["UpdateSuccessMessage"];
            await Message.Success(updateSuccessMessage);
            await EditTipoTransacaoModal.Hide();
        }
    }
    private async Task Search()
    {
        if (await validationsRef.ValidateAll())
        {
            var result = await TipoTransacaoAppService.GetListByDescricaoAsync(SearchTipoTransacao);
            TipoTransacaoList = result;
            TotalCount = (int)result.Count;
        }

        GetTipoTransacoesAsync();
    }
}
