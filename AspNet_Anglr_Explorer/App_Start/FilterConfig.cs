using System.Web;
using System.Web.Mvc;

namespace AspNet_Anglr_Explorer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
