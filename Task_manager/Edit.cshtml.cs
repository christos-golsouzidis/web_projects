using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManager.Pages
{
    public class EditModel : PageModel
    {
        public Task? task { get; set; }
        public bool ErrorEmptyname { get; set; }
        public IActionResult OnGet()
        {
            ErrorEmptyname = false;
            var a = Request.Query["id"];
            if(Request.Query["err"] == "1")
                ErrorEmptyname = true;
            task = DB.Find(a);

            if (task == null)
                return Redirect("/?" + DB.SortStr);

            return Page();
        }
        public IActionResult OnPost()
        {
            Task task = new Task();
            task.Project = Request.Form["project"][0];
            task.Description = Request.Form["description"][0];
            task.Status = Convert.ToInt32(Request.Form["status"][0]);
            var deadline = Request.Form["deadline"][0];
            if (deadline != "")
                task.Deadline = Convert.ToDateTime(deadline);
            task.Priority = Convert.ToInt32(Request.Form["priority"][0]);
            task.ContactInfo = Request.Form["contact"][0];
            task.AdditionalInfo = Request.Form["additional"][0];
            task.Id = Convert.ToInt32(Request.Form["id"][0]);

            //write to db
            var a = Request.Form["Action"][0];
            if (a == "E")
            {
                if(task.Project == "")
                    return Redirect($"/Edit?id={task.Id}&err=1");
                if (DB.Set(task) == 0)
                    return Redirect($"/Edit?id={task.Id}&err=2");
            }
            else if (a == "D")
            {
                if (DB.Delete(task) == 0)
                    return Redirect($"/Edit?id={task.Id}&err=2");
            }
            else
                return Redirect($"/Edit?id={task.Id}&err=2");

            return Redirect($"/?sort={DB.SortStr}");
        }
    }
}
