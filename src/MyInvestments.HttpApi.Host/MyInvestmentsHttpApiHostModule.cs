using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyInvestments.EntityFrameworkCore;
using MyInvestments.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.OpenIddict;
using static IdentityModel.ClaimComparer;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.Extensions.Hosting.Internal;

namespace MyInvestments;

[DependsOn(
    typeof(MyInvestmentsHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(MyInvestmentsApplicationModule),
    typeof(MyInvestmentsEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class MyInvestmentsHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        /*
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("MyInvestments");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
        */

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("MyInvestments");
                options.UseLocalServer();
                options.UseAspNetCore();
            });

            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            if (hostingEnvironment.IsDevelopment()) return;

            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                //serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", configuration["OpenIddictCertificate:X590:Password"]);
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", configuration["OpenIddictCertificate:X590:PassWord"]);
                //serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "f97f0fb2-6263-4464-945c-a636cc6b1cb3");
            });
        });

        // Segunda solução
        //var hostingEnvironment = context.Services.GetHostingEnvironment();

        //if (!hostingEnvironment.IsDevelopment())
        //{
        //    PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        //    {
        //        options.AddDevelopmentEncryptionAndSigningCertificate = false;
        //    });

        //    PreConfigure<OpenIddictServerBuilder>(builder =>
        //    {
        //        // In production, it is recommended to use two RSA certificates, 
        //        // one for encryption, one for signing.
        //        builder.AddEncryptionCertificate(
        //                GetEncryptionCertificate(hostingEnvironment, context.Services.GetConfiguration()));
        //        builder.AddSigningCertificate(
        //                GetSigningCertificate(hostingEnvironment, context.Services.GetConfiguration()));
        //    });
        //}
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);

        //Para exibir erros genéricos
        //context.Services.Configure<AbpExceptionHandlingOptions>(options =>
        //{
        //    options.SendExceptionsDetailsToClients = true;
        //    options.SendStackTraceToClients = true;
        //});
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<MyInvestmentsDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}MyInvestments.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<MyInvestmentsDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}MyInvestments.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<MyInvestmentsApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}MyInvestments.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<MyInvestmentsApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}MyInvestments.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(MyInvestmentsApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                    {"MyInvestments", "MyInvestments API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyInvestments API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyInvestments API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("MyInvestments");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }

    // Segunda solução
    //private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv,
    //                            IConfiguration configuration)
    //{
    //    var fileName = $"cert-signing.pfx";
    //    var passPhrase = configuration["MyAppCertificate:X590:PassPhrase"];
    //    var file = Path.Combine(hostingEnv.ContentRootPath, fileName);
    //    if (File.Exists(file))
    //    {
    //        var created = File.GetCreationTime(file);
    //        var days = (DateTime.Now - created).TotalDays;
    //        if (days > 180)
    //            File.Delete(file);
    //        else
    //            return new X509Certificate2(file, passPhrase,
    //                         X509KeyStorageFlags.MachineKeySet);
    //    }

    //    // file doesn't exist or was deleted because it expired
    //    using var algorithm = RSA.Create(keySizeInBits: 2048);
    //    var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
    //    var request = new CertificateRequest(subject, algorithm,
    //                        HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    //    request.CertificateExtensions.Add(new X509KeyUsageExtension(
    //                        X509KeyUsageFlags.DigitalSignature, critical: true));
    //    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow,
    //                        DateTimeOffset.UtcNow.AddYears(2));
    //    File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
    //    return new X509Certificate2(file, passPhrase,
    //                        X509KeyStorageFlags.MachineKeySet);
    //}

    //private X509Certificate2 GetEncryptionCertificate(IWebHostEnvironment hostingEnv,
    //                             IConfiguration configuration)
    //{
    //    var fileName = $"cert-encryption.pfx";
    //    var passPhrase = configuration["MyAppCertificate:X590:PassPhrase"];
    //    var file = Path.Combine(hostingEnv.ContentRootPath, fileName);
    //    if (File.Exists(file))
    //    {
    //        var created = File.GetCreationTime(file);
    //        var days = (DateTime.Now - created).TotalDays;
    //        if (days > 180)
    //            File.Delete(file);
    //        else
    //            return new X509Certificate2(file, passPhrase,
    //                            X509KeyStorageFlags.MachineKeySet);
    //    }

    //    // file doesn't exist or was deleted because it expired
    //    using var algorithm = RSA.Create(keySizeInBits: 2048);
    //    var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
    //    var request = new CertificateRequest(subject, algorithm,
    //                        HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    //    request.CertificateExtensions.Add(new X509KeyUsageExtension(
    //                        X509KeyUsageFlags.KeyEncipherment, critical: true));
    //    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow,
    //                        DateTimeOffset.UtcNow.AddYears(2));
    //    File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
    //    return new X509Certificate2(file, passPhrase, X509KeyStorageFlags.MachineKeySet);
    //}

}
