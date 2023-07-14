using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TaskManager.Pages
{
    public class CreateModel : PageModel
    {
        public Task task = new Task();
        public void OnGet()
        {
            // configure db





        }

        public IActionResult OnPost() 
        {
            task.Project = Request.Form["project"];
            task.Description = Request.Form["description"];
            task.Status = Convert.ToInt32(Request.Form["status"]);
            task.Priority = Convert.ToInt32(Request.Form["priority"]);
            var deadline = Request.Form["deadline"];
            if (deadline != "")
                task.Deadline = DateTime.Parse(deadline);
            task.ContactInfo = Request.Form["contact"];
            task.AdditionalInfo = Request.Form["additional"];
            if (task.Project == "")
                task.Project = "Default";
            // write entry to db
            if(DB.Create(task) == 0)
                return Redirect("/Create");

            return Redirect("/");
        }
    }
}
