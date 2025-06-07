using Acme.SimpleTaskApp.Sessions.Dto;

namespace Acme.SimpleTaskApp.Web.Views.Shared.Components.SideBarUserArea;

public class SideBarUserAreaViewModel
{
    public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

    public bool IsMultiTenancyEnabled { get; set; }

    public string GetShownLoginName()
    {
        var user = LoginInformations?.User;

        if(user == null)
        {
            return "<div class='btn-login'><i class='fas fa-sign-in-alt '></i> Đăng nhập</div>";
        }
        var userName = user.UserName;

        if (!IsMultiTenancyEnabled)
        {
            return userName;
        }

        return LoginInformations.Tenant == null
            ?  userName
            :  "Xin chào! " + userName;

        //: LoginInformations.Tenant.TenancyName + "\\" + userName;

    }
}
