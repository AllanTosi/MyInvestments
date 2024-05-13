using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using MyInvestments.Ativos;
using MyInvestments.ClasseAtivos;
using MyInvestments.Operacoes;
using MyInvestments.Setores;
using MyInvestments.TipoTransacoes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyInvestments.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class MyInvestmentsDbContext :
    AbpDbContext<MyInvestmentsDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<Ativo> Ativos { get; set; }
    public DbSet<ClasseAtivo> ClasseAtivos { get; set; }
    public DbSet<Operacao> Operacoes { get; set; }
    public DbSet<Setor> Setores { get; set; }
    public DbSet<TipoTransacao> TipoTransacoes { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public MyInvestmentsDbContext(DbContextOptions<MyInvestmentsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();        

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(MyInvestmentsConsts.DbTablePrefix + "YourEntities", MyInvestmentsConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<Ativo>(b =>
        {
            b.ToTable(MyInvestmentsConsts.DbTablePrefix + "Ativos",
                MyInvestmentsConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Ticker).IsRequired().HasMaxLength(AtivoConsts.MaxTickerLength);
            b.HasIndex(u => u.Ticker);
            b.Property(x => x.Nome).IsRequired().HasMaxLength(AtivoConsts.MaxNomeLength);
            b.HasIndex(u => u.Nome);

                    // ADD THE MAPPING FOR THE RELATION
                    b.HasOne<ClasseAtivo>().WithMany().HasForeignKey(x => x.ClasseAtivoId).IsRequired();
                    b.HasOne<Setor>().WithMany().HasForeignKey(x => x.SetorId).IsRequired();
        });

        builder.Entity<ClasseAtivo>(b =>
        {
            b.ToTable(MyInvestmentsConsts.DbTablePrefix + "ClasseAtivos",
                MyInvestmentsConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Nome).IsRequired().HasMaxLength(ClasseAtivoConsts.MaxNomeLength);
            b.HasIndex(u => u.Nome);
        });

        builder.Entity<Operacao>(b =>
        {
            b.ToTable(MyInvestmentsConsts.DbTablePrefix + "Operacoes",
                MyInvestmentsConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.DataOperacao).IsRequired();
            b.Property(x => x.Quantidade).IsRequired();
            b.Property(x => x.Preco).IsRequired();
            b.Property(x => x.ValorEmulumento).IsRequired();
            b.Property(x => x.ValorIrpf).IsRequired();
        });

        builder.Entity<Setor>(b =>
        {
            b.ToTable(MyInvestmentsConsts.DbTablePrefix + "Setores",
                MyInvestmentsConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Descricao).IsRequired();
            b.HasIndex(u => u.Descricao);
        });

        builder.Entity<TipoTransacao>(b =>
        {
            b.ToTable(MyInvestmentsConsts.DbTablePrefix + "TipoTransacoes",
                MyInvestmentsConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Descricao).IsRequired();
            b.HasIndex(u => u.Descricao);
        });
        //builder.Entity<Ativo>(b =>
        //{
        //    b.ToTable(MyInvestmentsConsts.DbTablePrefix + "Ativos",
        //        MyInvestmentsConsts.DbSchema);

        //    b.ConfigureByConvention();

        //    b.Property(x => x.Name)
        //        .IsRequired()
        //        .HasMaxLength(MyInvestmentsConsts.MaxNameLength);

        //    b.HasIndex(x => x.Name);
        //});
    }
}
