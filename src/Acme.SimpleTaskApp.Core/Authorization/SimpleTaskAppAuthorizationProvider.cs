using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Acme.SimpleTaskApp.Authorization;

public class SimpleTaskAppAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
        context.CreatePermission(PermissionNames.Pages_Users_Edit, L("EditUser"));

        context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
        context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
        context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

        var products = context.CreatePermission(PermissionNames.Pages_Products, L("Products"));
        products.CreateChildPermission(PermissionNames.Pages_Products_Create, L("CreateProduct"));
        products.CreateChildPermission(PermissionNames.Pages_Products_Edit, L("EditProduct"));
        products.CreateChildPermission(PermissionNames.Pages_Products_Delete, L("DeleteProduct"));


        var categories = context.CreatePermission(PermissionNames.Pages_Categories, L("Categories"));
        categories.CreateChildPermission(PermissionNames.Pages_Categories_Create, L("CreateCategory"));
        categories.CreateChildPermission(PermissionNames.Pages_Categories_Edit, L("EditCategory"));
        categories.CreateChildPermission(PermissionNames.Pages_Categories_Delete, L("DeleteCategory"));

    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, SimpleTaskAppConsts.LocalizationSourceName);
    }
}
