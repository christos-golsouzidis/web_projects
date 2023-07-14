using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SQLite;

namespace TaskManager.Pages
{
    public class Task
    {
        public int Id { get; set; }

        public string Project { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public int Status { get; set; }

        public DateTime? Deadline { get; set; }

        public string ContactInfo { get; set; }

        public string AdditionalInfo { get; set; }

        public Task()
        {
            this.Id = 0;
            this.Priority = 0;
            this.Status = 3;
            this.Project = "";
            this.Description = "";
            this.ContactInfo = "";
            this.AdditionalInfo = "";
        }
    }

    public static class DB
    {
        public static string ConnStr { get; set; }
        public static string SortStr { get; set; }
        /// <summary>
        /// SortStr can have the following values: id, project, status, deadline, priority, contact
        /// </summary>
        public static List<Task> GTasks { get; set; }
        static DB()
        {
            DB.GTasks = new List<Task>();
            DB.SortStr = "id";
        }

        public static List<int> GetInfo()
        {
            List<int> count = new List<int>();

            using (SQLiteConnection conn = new SQLiteConnection(DB.ConnStr))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    return null;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(DB.ConnStr, conn))
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Tasks";
                    count.Add(Convert.ToInt32(cmd.ExecuteScalar()));

                    cmd.CommandText = "SELECT COUNT(*) FROM Tasks where Status = 3 OR Status = 4";
                    count.Add(Convert.ToInt32(cmd.ExecuteScalar()));

                    cmd.CommandText = "SELECT COUNT(*) FROM Tasks where Status = 5";
                    count.Add(Convert.ToInt32(cmd.ExecuteScalar()));

                    cmd.CommandText = "SELECT COUNT(*) FROM Tasks where Status = 2";
                    count.Add(Convert.ToInt32(cmd.ExecuteScalar()));

                    cmd.CommandText = "SELECT COUNT(*) FROM Tasks where Status = 1";
                    count.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                }
            }

            return count;
        }

        public static List<Task> Get(string SortBy = "")
        {
            DB.GTasks = new List<Task>();

            using (SQLiteConnection conn = new SQLiteConnection(DB.ConnStr))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    return null;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(DB.ConnStr, conn))
                {
                    string CmdTxt = "SELECT * FROM Tasks ";
                    if (SortBy != "")
                        CmdTxt += $"ORDER BY {SortBy}";
                    cmd.CommandText = CmdTxt;

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task();
                            task.Id = Convert.ToInt32(reader["Id"]);
                            try
                            {
                                task.Project = (string)reader.GetValue("Project");
                            }
                            catch { task.Project = "Default"; }
                            try
                            {
                                task.Description = (string)reader.GetValue("Description");
                            }
                            catch { task.Description = ""; }
                            try
                            {
                                task.Status = reader.GetInt32("Status");
                            }
                            catch { task.Status = 3; }
                            try
                            {
                                task.Priority = reader.GetInt32("Priority");
                            }
                            catch { task.Priority = 0; }
                            try
                            {
                                task.Deadline = Convert.ToDateTime((string)reader.GetValue("Deadline"));
                            }
                            catch { task.Deadline = null; }
                            try
                            {
                                task.ContactInfo = (string)reader.GetValue("Contact");
                            }
                            catch { task.ContactInfo = ""; }
                            try
                            {
                                task.AdditionalInfo = (string)reader.GetValue("Additional");
                            }
                            catch { task.AdditionalInfo = ""; }

                            DB.GTasks.Add(task);
                        }
                    }
                }
            }
            return DB.GTasks;
        }
        public static Task Find(string Idx)
        {
            Task task = new Task();

            using (SQLiteConnection conn = new SQLiteConnection(DB.ConnStr))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    return null;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(DB.ConnStr, conn))
                {
                    cmd.CommandText = $"SELECT * FROM Tasks where Id = '{Idx}'";

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            task.Id = Convert.ToInt32(Idx);
                            try
                            {
                                task.Project = (string)reader.GetValue("Project");
                            }
                            catch { task.Project = "Empty task"; }
                            try
                            {
                                task.Description = (string)reader.GetValue("Description");
                            }
                            catch { task.Description = ""; }
                            try
                            {
                                task.Status = reader.GetInt32("Status");
                            }
                            catch { task.Status = 3; }
                            try
                            {
                                task.Priority = reader.GetInt32("Priority");
                            }
                            catch { task.Priority = 0; }
                            try
                            {
                                task.Deadline = Convert.ToDateTime((string)reader.GetValue("Deadline"));
                            }
                            catch { task.Deadline = null; }
                            try
                            {
                                task.ContactInfo = (string)reader.GetValue("Contact");
                            }
                            catch { task.ContactInfo = ""; }
                            try
                            {
                                task.AdditionalInfo = (string)reader.GetValue("Additional");
                            }
                            catch { task.AdditionalInfo = ""; }
                            return task;

                        }
                    }
                }
            }
            return null;
        }

        public static int Set(Task task)
        {
            int r;
            using (SQLiteConnection conn = new SQLiteConnection(DB.ConnStr))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    return 0;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(DB.ConnStr, conn))
                {
                    cmd.CommandText = "UPDATE Tasks SET " +
                        "Project=@_taskProject ," +
                        "Description=@_taskDescription ," +
                        "Status=@_taskStatus ," +
                        "Deadline=@_taskDeadline ," +
                        "Priority=@_taskPriority ," +
                        "Contact=@_taskContactInfo ," +
                        "Additional=@_taskAdditionalInfo " +
                        "WHERE Id = @_taskId";

                    cmd.Parameters.AddWithValue("@_taskProject", task.Project);
                    cmd.Parameters.AddWithValue("@_taskDescription", task.Description);
                    cmd.Parameters.AddWithValue("@_taskStatus", task.Status);
                    cmd.Parameters.AddWithValue("@_taskDeadline", task.Deadline);
                    cmd.Parameters.AddWithValue("@_taskPriority", task.Priority);
                    cmd.Parameters.AddWithValue("@_taskContactInfo", task.ContactInfo);
                    cmd.Parameters.AddWithValue("@_taskAdditionalInfo", task.AdditionalInfo);
                    cmd.Parameters.AddWithValue("@_taskId", task.Id);

                    r = cmd.ExecuteNonQuery();
                }
            }
            return r;
        }
        public static int Create(Task task)
        {
            int r;
            using (SQLiteConnection conn = new SQLiteConnection(DB.ConnStr))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    return 0;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(DB.ConnStr, conn))
                {
                    cmd.CommandText = "INSERT INTO Tasks ( " +
                        "Project, " +
                        "Description, " +
                        "Status, " +
                        "Deadline, " +
                        "Priority, " +
                        "Contact, " +
                        "Additional ) VALUES ( " +
                        "@_taskProject , " +
                        "@_taskDescription , " +
                        "@_taskStatus , " +
                        "@_taskDeadline , " +
                        "@_taskPriority , " +
                        "@_taskContactInfo , " +
                        "@_taskAdditionalInfo ) ";

                    cmd.Parameters.AddWithValue("@_taskProject", task.Project);
                    cmd.Parameters.AddWithValue("@_taskDescription", task.Description);
                    cmd.Parameters.AddWithValue("@_taskStatus", task.Status);
                    cmd.Parameters.AddWithValue("@_taskDeadline", task.Deadline);
                    cmd.Parameters.AddWithValue("@_taskPriority", task.Priority);
                    cmd.Parameters.AddWithValue("@_taskContactInfo", task.ContactInfo);
                    cmd.Parameters.AddWithValue("@_taskAdditionalInfo", task.AdditionalInfo);

                    r = cmd.ExecuteNonQuery();
                }
            }
            return r;
        }

        public static int Delete(Task task)
        {
            int r;
            using (SQLiteConnection conn = new SQLiteConnection(DB.ConnStr))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    return 0;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(DB.ConnStr, conn))
                {
                    cmd.CommandText = "DELETE FROM Tasks WHERE Id = @_taskId";
                    cmd.Parameters.AddWithValue("@_taskId", task.Id);

                    r = cmd.ExecuteNonQuery();
                }
            }
            return r;
        }

    }
}
