using System.Web;
using System.Web.Mvc;

namespace JoshDevenyi_PokemonPassionProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
