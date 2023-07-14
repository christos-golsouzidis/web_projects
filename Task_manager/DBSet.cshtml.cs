using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SQLite;

namespace TaskManager.Pages
{
    public class DBSetModel : PageModel
    {
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            DB.ConnStr = $"Data Source=file:///{Request.Form["filename"][0]}; Version=3;";
            if (DB.ConnStr != null)
            {
                using (SQLiteConnection conn = new SQLiteConnection(DB.ConnStr))
                {
                    try
                    {
                        conn.Open();
                    }
                    catch
                    {
                        return Redirect("/DBSet");
                    }

                }


                return Redirect("/");
            }
            return Redirect("/DBSet");
        }
    }
}
