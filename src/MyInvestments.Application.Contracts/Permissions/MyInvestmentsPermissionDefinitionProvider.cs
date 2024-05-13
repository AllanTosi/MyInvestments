using MyInvestments.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MyInvestments.Permissions;

public class MyInvestmentsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //var myGroup = context.AddGroup(MyInvestmentsPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MyInvestmentsPermissions.MyPermission1, L("Permission:MyPermission1"));

        var myInvestmentsGroup = context.AddGroup(MyInvestmentsPermissions.GroupName, L("Permission:MyInvestments"));

        var ativosPermission = myInvestmentsGroup.AddPermission(MyInvestmentsPermissions.Ativos.Default, L("Permission:Ativos"));
        ativosPermission.AddChild(MyInvestmentsPermissions.Ativos.Create, L("Permission:Ativos.Create"));
        ativosPermission.AddChild(MyInvestmentsPermissions.Ativos.Edit, L("Permission:Ativos.Edit"));
        ativosPermission.AddChild(MyInvestmentsPermissions.Ativos.Delete, L("Permission:Ativos.Delete"));

        var classeAtivosPermission = myInvestmentsGroup.AddPermission(MyInvestmentsPermissions.ClasseAtivos.Default, L("Permission:ClasseAtivos"));
        classeAtivosPermission.AddChild(MyInvestmentsPermissions.ClasseAtivos.Create, L("Permission:ClasseAtivos.Create"));
        classeAtivosPermission.AddChild(MyInvestmentsPermissions.ClasseAtivos.Edit, L("Permission:ClasseAtivos.Edit"));
        classeAtivosPermission.AddChild(MyInvestmentsPermissions.ClasseAtivos.Delete, L("Permission:ClasseAtivos.Delete"));
        
        var operacoesPermission = myInvestmentsGroup.AddPermission(MyInvestmentsPermissions.Operacoes.Default, L("Permission:Operacoes"));
        operacoesPermission.AddChild(MyInvestmentsPermissions.Operacoes.Create, L("Permission:Operacoes.Create"));
        operacoesPermission.AddChild(MyInvestmentsPermissions.Operacoes.Edit, L("Permission:Operacoes.Edit"));
        operacoesPermission.AddChild(MyInvestmentsPermissions.Operacoes.Delete, L("Permission:Operacoes.Delete"));

        var setoresPermission = myInvestmentsGroup.AddPermission(MyInvestmentsPermissions.Setores.Default, L("Permission:Setores"));
        setoresPermission.AddChild(MyInvestmentsPermissions.Setores.Create, L("Permission:Setores.Create"));
        setoresPermission.AddChild(MyInvestmentsPermissions.Setores.Edit, L("Permission:Setores.Edit"));
        setoresPermission.AddChild(MyInvestmentsPermissions.Setores.Delete, L("Permission:Setores.Delete"));

        var tipoTransacoesPermission = myInvestmentsGroup.AddPermission(MyInvestmentsPermissions.TipoTransacoes.Default, L("Permission:TipoTransacoes"));
        tipoTransacoesPermission.AddChild(MyInvestmentsPermissions.TipoTransacoes.Create, L("Permission:TipoTransacoes.Create"));
        tipoTransacoesPermission.AddChild(MyInvestmentsPermissions.TipoTransacoes.Edit, L("Permission:TipoTransacoes.Edit"));
        tipoTransacoesPermission.AddChild(MyInvestmentsPermissions.TipoTransacoes.Delete, L("Permission:TipoTransacoes.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MyInvestmentsResource>(name);
    }
}
