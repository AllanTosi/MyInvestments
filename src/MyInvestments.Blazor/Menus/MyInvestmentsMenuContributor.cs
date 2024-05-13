using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyInvestments.Localization;
using MyInvestments.MultiTenancy;
using MyInvestments.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace MyInvestments.Blazor.Menus;

public class MyInvestmentsMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public MyInvestmentsMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private async Task<Task> ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<MyInvestmentsResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                MyInvestmentsMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home"
            )
        );
     
        var administration = context.Menu.GetAdministration();

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        var myInvestmentsMenu = new ApplicationMenuItem(
            "MyInvestments",
            l["Menu:MyInvestments"],
            icon: "fa fa-book"
        );

        context.Menu.AddItem(myInvestmentsMenu);

        //CHECK the PERMISSION

        if (await context.IsGrantedAsync(MyInvestmentsPermissions.Ativos.Default))
        {
            myInvestmentsMenu.AddItem(new ApplicationMenuItem(
                "MyInvestments.Ativos",
                l["Menu:Ativos"],
                url: "/ativos"
            ));
        }

        if (await context.IsGrantedAsync(MyInvestmentsPermissions.ClasseAtivos.Default))
        {
            myInvestmentsMenu.AddItem(new ApplicationMenuItem(
                "MyInvestments.ClasseAtivos",
                l["Menu:ClasseAtivos"],
                url: "/classeativos"
            ));
        }

        if (await context.IsGrantedAsync(MyInvestmentsPermissions.Operacoes.Default))
        {
            myInvestmentsMenu.AddItem(new ApplicationMenuItem(
                "MyInvestments.Operacoes",
                l["Menu:Operacoes"],
                url: "/operacoes"
            ));
        }

        if (await context.IsGrantedAsync(MyInvestmentsPermissions.Setores.Default))
        {
            myInvestmentsMenu.AddItem(new ApplicationMenuItem(
                "MyInvestments.Setores",
                l["Menu:Setores"],
                url: "/setores"
            ));
        }

        if (await context.IsGrantedAsync(MyInvestmentsPermissions.TipoTransacoes.Default))
        {
            myInvestmentsMenu.AddItem(new ApplicationMenuItem(
                "MyInvestments.TipoTransacoes",
                l["Menu:TipoTransacoes"],
                url: "/tipotransacoes"
            ));
        }

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();

        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
