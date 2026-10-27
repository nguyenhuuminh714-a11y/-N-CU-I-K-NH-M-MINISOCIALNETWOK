using System.Web;
using System.Web.Mvc;

namespace ĐỀ_ÁN_CUỐI_KỲ_NHÓM_MINISOCIALNETWOK
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
