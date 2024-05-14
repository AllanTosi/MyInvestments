using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Ativos;
using MyInvestments.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite;

namespace MyInvestments.Blazor.Pages;

public partial class Ativos
{
    private IReadOnlyList<AtivoDto> AtivoList { get; set; }

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    private bool CanCreateAtivo { get; set; }
    private bool CanEditAtivo { get; set; }
    private bool CanDeleteAtivo { get; set; }

    private CreateAtivoDto NewAtivo { get; set; }

    private Guid EditingAtivoId { get; set; }
    private UpdateAtivoDto EditingAtivo { get; set; }

    private Modal CreateAtivoModal { get; set; }
    private Modal EditAtivoModal { get; set; }

    private Validations CreateValidationsRef;

    private Validations EditValidationsRef;

    private Validations validationsRef;
    private String SearchAtivo { get; set; }

    public Ativos()
    {
        NewAtivo = new CreateAtivoDto();
        EditingAtivo = new UpdateAtivoDto();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetAtivosAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateAtivo = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Ativos.Create);

        CanEditAtivo = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Ativos.Edit);

        CanDeleteAtivo = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Ativos.Delete);
    }

    private async Task GetAtivosAsync()
    {
        var result = await AtivoAppService.GetListAsync(
            new GetAtivoListDto
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting
            }
        );

        AtivoList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<AtivoDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetAtivosAsync();

        await InvokeAsync(StateHasChanged);
    }

    private void OpenCreateAtivoModal()
    {
        CreateValidationsRef.ClearAll();

        NewAtivo = new CreateAtivoDto();
        CreateAtivoModal.Show();
    }

    private void CloseCreateAtivoModal()
    {
        CreateAtivoModal.Hide();
    }

    private void OpenEditAtivoModal(AtivoDto ativo)
    {
        EditValidationsRef.ClearAll();

        EditingAtivoId = ativo.Id;
        EditingAtivo = ObjectMapper.Map<AtivoDto, UpdateAtivoDto>(ativo);
        EditAtivoModal.Show();
    }

    private async Task DeleteAtivoAsync(AtivoDto ativo)
    {
        var confirmMessage = L["AreYouSureToDelete", ativo.Descricao];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }

        await AtivoAppService.DeleteAsync(ativo.Id);
        await GetAtivosAsync();
    }

    private void CloseEditAtivoModal()
    {
        EditAtivoModal.Hide();
    }

    private async Task CreateAtivoAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await AtivoAppService.CreateAsync(NewAtivo);
            await GetAtivosAsync();
            var addSuccessMessage = L["AddSuccessMessage"];
            await Message.Success(addSuccessMessage);
            await CreateAtivoModal.Hide();
        }
    }

    private async Task UpdateAtivoAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await AtivoAppService.UpdateAsync(EditingAtivoId, EditingAtivo);
            await GetAtivosAsync();
            var updateSuccessMessage = L["UpdateSuccessMessage"];
            await Message.Success(updateSuccessMessage);
            await EditAtivoModal.Hide();
        }
    }

    private async Task Search()
    {
        if (await validationsRef.ValidateAll())
        {
            var result = await AtivoAppService.GetListByTickerAsync(SearchAtivo);
            AtivoList = result;
            TotalCount = (int)result.Count;
        }

        GetAtivosAsync();
    }
}
