using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInvestments.ClasseAtivos;
using MyInvestments.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite;
using AutoMapper.Internal.Mappers;

namespace MyInvestments.Blazor.Pages;

public partial class ClasseAtivos
{
    private IReadOnlyList<ClasseAtivoDto> ClasseAtivoList { get; set; }

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    private bool CanCreateClasseAtivo { get; set; }
    private bool CanEditClasseAtivo { get; set; }
    private bool CanDeleteClasseAtivo { get; set; }

    private CreateClasseAtivoDto NewClasseAtivo { get; set; }

    private Guid EditingClasseAtivoId { get; set; }
    private UpdateClasseAtivoDto EditingClasseAtivo { get; set; }

    private Modal CreateClasseAtivoModal { get; set; }
    private Modal EditClasseAtivoModal { get; set; }

    private Validations CreateValidationsRef;

    private Validations EditValidationsRef;

    private Validations validationsRef;
    private String SearchClasseAtivo { get; set; }

    public ClasseAtivos()
    {
        NewClasseAtivo = new CreateClasseAtivoDto();
        EditingClasseAtivo = new UpdateClasseAtivoDto();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetClasseAtivosAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateClasseAtivo = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.ClasseAtivos.Create);

        CanEditClasseAtivo = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.ClasseAtivos.Edit);

        CanDeleteClasseAtivo = await AuthorizationService
            .IsGrantedAsync(MyInvestmentsPermissions.ClasseAtivos.Delete);
    }

    private async Task GetClasseAtivosAsync()
    {
        var result = await ClasseAtivoAppService.GetListAsync(
            new GetClasseAtivoListDto
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting
            }
        );

        ClasseAtivoList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ClasseAtivoDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetClasseAtivosAsync();

        await InvokeAsync(StateHasChanged);
    }

    private void OpenCreateClasseAtivoModal()
    {
        CreateValidationsRef.ClearAll();

        NewClasseAtivo = new CreateClasseAtivoDto();
        CreateClasseAtivoModal.Show();
    }

    private void CloseCreateClasseAtivoModal()
    {
        CreateClasseAtivoModal.Hide();
    }

    private void OpenEditClasseAtivoModal(ClasseAtivoDto classeAtivo)
    {
        EditValidationsRef.ClearAll();

        EditingClasseAtivoId = classeAtivo.Id;
        EditingClasseAtivo = ObjectMapper.Map<ClasseAtivoDto, UpdateClasseAtivoDto>(classeAtivo);
        EditClasseAtivoModal.Show();
    }

    private async Task DeleteClasseAtivoAsync(ClasseAtivoDto classeAtivo)
    {
        var confirmMessage = L["AreYouSureToDelete", classeAtivo.Nome];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }

        await ClasseAtivoAppService.DeleteAsync(classeAtivo.Id);
        await GetClasseAtivosAsync();
    }

    private void CloseEditClasseAtivoModal()
    {
        EditClasseAtivoModal.Hide();
    }

    private async Task CreateClasseAtivoAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await ClasseAtivoAppService.CreateAsync(NewClasseAtivo);
            await GetClasseAtivosAsync();
            var addSuccessMessage = L["AddSuccessMessage"];
            await Message.Success(addSuccessMessage);
            await CreateClasseAtivoModal.Hide();
        }
    }

    private async Task UpdateClasseAtivoAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await ClasseAtivoAppService.UpdateAsync(EditingClasseAtivoId, EditingClasseAtivo);
            await GetClasseAtivosAsync();
            var updateSuccessMessage = L["UpdateSuccessMessage"];
            await Message.Success(updateSuccessMessage);
            await EditClasseAtivoModal.Hide();
        }
    }

    private async Task Search()
    {
        if (await validationsRef.ValidateAll())
        {
            var result = await ClasseAtivoAppService.GetListByNameAsync(SearchClasseAtivo);
            ClasseAtivoList = result;
            TotalCount = (int)result.Count;
        }

        GetClasseAtivosAsync();
    }
}
