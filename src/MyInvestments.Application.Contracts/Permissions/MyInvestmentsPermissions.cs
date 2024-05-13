namespace MyInvestments.Permissions;

public static class MyInvestmentsPermissions
{
    public const string GroupName = "MyInvestments";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Ativos
    {
        public const string Default = GroupName + ".Ativos";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    
    public static class ClasseAtivos
    {
        public const string Default = GroupName + ".ClasseAtivos";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    
    public static class Operacoes
    {
        public const string Default = GroupName + ".Operacoes";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    
    public static class Setores
    {
        public const string Default = GroupName + ".Setores";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    
    public static class TipoTransacoes
    {
        public const string Default = GroupName + ".TipoTransacoes";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
