using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.Setores;
using MyInvestments.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite;
using System.Diagnostics;
using Microsoft.JSInterop;

namespace MyInvestments.Blazor.Pages;

public partial class Setores
{
    private IReadOnlyList<SetorDto> SetorList { get; set; }

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    private bool CanCreateSetor { get; set; }
    private bool CanEditSetor { get; set; }
    private bool CanDeleteSetor { get; set; }

    private CreateSetorDto NewSetor { get; set; }

    private Guid EditingSetorId { get; set; }
    private UpdateSetorDto EditingSetor { get; set; }

    private Modal CreateSetorModal { get; set; }
    private Modal EditSetorModal { get; set; }

    private Validations CreateValidationsRef;

    private Validations EditValidationsRef;

    private Validations validationsRef;
    private String SearchSetor { get; set; }

    public Setores()
    {
        NewSetor = new CreateSetorDto();
        EditingSetor = new UpdateSetorDto();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetSetoresAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateSetor = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Setores.Create);

        CanEditSetor = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Setores.Edit);

        CanDeleteSetor = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.Setores.Delete);
    }

    private async Task GetSetoresAsync()
    {
        var result = await SetorAppService.GetListAsync(
            new GetSetorListDto
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting
            }
        );

        SetorList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<SetorDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetSetoresAsync();

        await InvokeAsync(StateHasChanged);
    }

    private void OpenCreateSetorModal()
    {
        CreateValidationsRef.ClearAll();

        NewSetor = new CreateSetorDto();
        CreateSetorModal.Show();
    }

    private void CloseCreateSetorModal()
    {
        CreateSetorModal.Hide();
    }

    private void OpenEditSetorModal(SetorDto setor)
    {
        EditValidationsRef.ClearAll();

        EditingSetorId = setor.Id;
        EditingSetor = ObjectMapper.Map<SetorDto, UpdateSetorDto>(setor);
        EditSetorModal.Show();
    }

    private async Task DeleteSetorAsync(SetorDto setor)
    {
        var confirmMessage = L["AreYouSureToDelete", setor.Descricao];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }

        await SetorAppService.DeleteAsync(setor.Id);
        await GetSetoresAsync();
    }

    private void CloseEditSetorModal()
    {
        EditSetorModal.Hide();
    }

    private async Task CreateSetorAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await SetorAppService.CreateAsync(NewSetor);
            await GetSetoresAsync();
            var addSuccessMessage = L["AddSuccessMessage"];
            await Message.Success(addSuccessMessage);
            await CreateSetorModal.Hide();
        }
    }

    private async Task UpdateSetorAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await SetorAppService.UpdateAsync(EditingSetorId, EditingSetor);
            await GetSetoresAsync();
            var updateSuccessMessage = L["UpdateSuccessMessage"];
            await Message.Success(updateSuccessMessage);
            await EditSetorModal.Hide();
        }
    }
    
    private async Task Search()
    {
        if (await validationsRef.ValidateAll())
        {
            var result = await SetorAppService.GetListByDescricaoAsync(SearchSetor);
            SetorList = result;
            TotalCount = (int)result.Count;
        }

        GetSetoresAsync();
    }
    private async Task ExportToExcel()
    {
        var excelBytes = await ExportToExcelAppService.ExportToExcelSetores();
        await JsRuntime.InvokeVoidAsync("saveAsFile", $"Setores_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx",
            Convert.ToBase64String(excelBytes));
    }
}
