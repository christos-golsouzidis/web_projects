using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManager.Pages
{
    public class IndexModel : PageModel
    {
        public List<Task> Tasks;
        public List<int> Tasknum;
        public string Order = DB.SortStr;

        public IActionResult OnGet()
        {
            Tasks = new List<Task>();
            if (DB.ConnStr == null)
                return Redirect("/DBSet");

            Order = (string)Request.Query["sort"] ?? "id";
            Tasks = DB.Get(Order);

            Tasknum = DB.GetInfo();

            Tasks = DB.GTasks;

            return Page();
        }

        public IActionResult OnPost()
        {
            string ReqStr;

            if((string)Request.Form["edit"] != null)
                return Redirect("/Edit?" + Request.Form["edit"]);

            DB.SortStr = (string)Request.Form["sort"];


            return Redirect("/?sort=" + DB.SortStr);

        }
    }
}