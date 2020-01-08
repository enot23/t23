using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class HomeController : Controller
    {
       
        ToDoContext db = new ToDoContext();
        public ActionResult Index(IEnumerable<Models.Task> tasks = null)//tasks - the tasks that will be displayed in the search results
        { 
            return View(tasks);
        }

        #region For Tasks
        [HttpGet]
        public ActionResult CreateTask(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult CreateTask(Task task)
        {
            if (task != null)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
            }
            return RedirectToAction("CreateTask");
        }

        [HttpGet]
        public ActionResult EditTask(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }
        [HttpPost]
        public ActionResult EditTask(Task task)
        {
            Task newtask = db.Tasks.Find(task.Id);
            newtask.Complete = task.Complete;
            newtask.Title = task.Title;
            newtask.Importance_Level = task.Importance_Level;
            newtask.Description = task.Description;
            newtask.Due_Date = task.Due_Date;

            db.Entry(newtask).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsTask(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [HttpGet]
        public ActionResult DeleteTask(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }
        [HttpPost]
        public ActionResult DeleteTask(Task task)
        {
            db.Entry(task).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult MultiItemsDelete()
        {
            Dictionary<int, string> List_Titles = new Dictionary<int, string>();
            foreach (var l in db.CustomLists)
            {
                List_Titles.Add(l.Id, l.Title);
            }
            ViewBag.List_Titles = List_Titles;
            return View(db.Tasks);

        }
        [HttpPost]
        public ActionResult MultiItemsDelete(IEnumerable<int> tasks_to_delete)
        {
            db.Tasks.RemoveRange(db.Tasks.Where(t => tasks_to_delete.Contains(t.Id)).ToList());
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SearchTasks(string request = null)
        {
            
            IEnumerable<Models.Task> Tasks = (request != null && request != "") ? db.Tasks.Where(t => t.Title.Contains(request)) : null;

            return View("Index", Tasks);
        }
        [HttpGet]
        public ActionResult ListOfTasks(IEnumerable<Models.Task> model,bool show_list_title)
        {
            ViewBag.show_list_title = show_list_title;
            if (show_list_title)
            {
                Dictionary<int, string> List_Titles = new Dictionary<int, string>();
                foreach (var l in db.CustomLists)
                {
                    List_Titles.Add(l.Id, l.Title);
                }
                ViewBag.List_Titles = List_Titles;
                
            }
            ViewBag.Sort_type_now = Sort_Type;
            if (model != null)
                switch ( Sort_Type)
                {

                    case Sort_Types.Importance_Ascending:
                        model = model.OrderBy(t => t.Importance_Level);
                        break;

                    case Sort_Types.Importance_Descending:
                        model = model.OrderByDescending(t => t.Importance_Level);
                        break;

                    case Sort_Types.Due_date_Ascending:
                        model = model.OrderBy(t => t.Due_Date);
                        break;

                    case Sort_Types.Due_date_Descending:
                        model = model.OrderByDescending(t => t.Due_Date);
                        break;

                    case Sort_Types.Uncommpleted_first:
                        model = model.OrderBy(t => t.Complete);
                        break;

                    case Sort_Types.Completed_First:
                        model = model.OrderByDescending(t => t.Complete);
                        break;

                    case Sort_Types.Alphabetically_Ascending:
                        model = model.OrderBy(t => t.Title);
                        break;

                    case Sort_Types.Alphabeticall_Descendingy:
                        model = model.OrderByDescending(t => t.Title);
                        break;

                    case Sort_Types.Creation_date_Ascending:
                        model = model.OrderBy(t => t.Id);
                        break;

                    case Sort_Types.Creation_date_Descending:
                        model = model.OrderByDescending(t => t.Id);
                        break;

                }
            return PartialView("ListOfTasks",model);
        }
        
        #endregion

        #region For Lists
        [HttpGet]
        public ActionResult CreateList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateList(CustomList list)
        {
            if (list != null)
            {
                db.CustomLists.Add(list);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditList(int id = 0)
        {
            CustomList list = db.CustomLists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult EditList(CustomList list)
        {
            CustomList newlist = db.CustomLists.Find(list.Id);
            newlist.Title = list.Title;
            newlist.HideCompleted = list.HideCompleted;

            db.Entry(newlist).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult SettingsList(int id = 0)
        {
            SmartList list = db.SmartLists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult SettingsList(SmartList list)
        {
            SmartList newlist = db.SmartLists.Find(list.Id);
            newlist.HideCompleted = list.HideCompleted;

            db.Entry(newlist).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsList(int id = 0)
        {
            ViewBag.Edit_Ability = true;
            CustomList list = db.CustomLists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            ViewBag.list = list;
            var TasksInList = db.Tasks.Where(t => t.List_Id == list.Id);
            TasksInList = list.HideCompleted ? TasksInList.Where(t => t.Complete != true) : TasksInList;
            return View(TasksInList);
        }
        [HttpGet]
        public ActionResult DetailsSmartList(string title)
        {
            ViewBag.Edit_Ability = false;
            SmartList list = null; IQueryable<Task> TasksInList = null;
            switch (title)
            {
                case "Today":
                    list = db.SmartLists.First(l => l.Title == "Today"); ViewBag.list = list;
                    TasksInList = db.Tasks.Where(t => DbFunctions.TruncateTime(t.Due_Date) == DateTime.Today);
                    break;

                case "All Tasks":
                    list = db.SmartLists.First(l => l.Title == "All Tasks"); ViewBag.list = list;
                    TasksInList = db.Tasks as IQueryable<Task>;
                    break;


                case "Important":
                    list = db.SmartLists.First(l => l.Title == "Important"); ViewBag.list = list;
                    TasksInList = db.Tasks.Where(t => t.Importance_Level == Importance_Levels.High);
                    break;

                case "Planned":
                    list = db.SmartLists.First(l => l.Title == "Planned"); ViewBag.list = list;
                    TasksInList = db.Tasks.Where(t => t.Due_Date != null);
                    break;

            }
            TasksInList = list.HideCompleted ? TasksInList.Where(t => t.Complete != true) : TasksInList;

            return View("DetailsList", TasksInList); ;
        }

        [HttpGet]
        public ActionResult DeleteList(int id = 0)
        {
            CustomList list = db.CustomLists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult DeleteList(CustomList list)
        {
            db.Tasks.RemoveRange(db.Tasks.Where(t => t.List_Id == list.Id));
            db.Entry(list).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ListOfLists()
        {
            Dictionary<int, int> Custom_Tasks_Count = new Dictionary<int, int>();
            Dictionary<int, int> Custom_Uncompleted_Tasks_Count = new Dictionary<int, int>();
            foreach (var l in db.CustomLists)
            {
                int count = db.Tasks.Count(t => t.List_Id == l.Id);
                Custom_Tasks_Count.Add(l.Id, count);

                int uncompleted_count = db.Tasks.Where(t => t.Complete != true).Count(t => t.List_Id == l.Id);
                Custom_Uncompleted_Tasks_Count.Add(l.Id, uncompleted_count);
            }

            ViewBag.Custom_Tasks_Count = Custom_Tasks_Count;
            ViewBag.Custom_Uncompleted_Tasks_Count = Custom_Uncompleted_Tasks_Count;

            ViewBag.Smart_Tasks_Count = new List<int>() {

            db.Tasks.Count(t => DbFunctions.TruncateTime(t.Due_Date) == DateTime.Today),
            //Number of Today's tasks

            db.Tasks.Count(),
            //Number of All tasks

            db.Tasks.Count(t => t.Importance_Level == Models.Importance_Levels.High),
            //Number of Important tasks

            db.Tasks.Count(t => t.Due_Date != null)
             //Number of Planned tasks
            };

            ViewBag.Smart_Uncompleted_Tasks_Count = new List<int>() {

            db.Tasks.Where(t => t.Complete != true)
                .Count(t => DbFunctions.TruncateTime(t.Due_Date) == DateTime.Today),
            //Number of Today's tasks, which are completed 

            db.Tasks.Where(t => t.Complete != true).Count(),
            //Number of All tasks, which are completed

            db.Tasks.Where(t => t.Complete != true)
                .Count(t => t.Importance_Level == Models.Importance_Levels.High),
            //Number of Important tasks, which are completed

            db.Tasks.Where(t => t.Complete != true)
                .Count(t => t.Due_Date != null)
            //Number of Planned tasks, which are completed
                    };

            ViewBag.SmartLists = db.SmartLists;
            ViewBag.CustomLists = db.CustomLists;
            
            return PartialView("ListOfLists");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpGet]
        public void SetSort(Sort_Types type = Sort_Types.Creation_date_Ascending)
        {
            Sort_Type = type;

            Response.Redirect(Request.UrlReferrer.ToString());

        }
        public static Sort_Types Sort_Type { get; set; } = Sort_Types.Creation_date_Ascending;
    }
    public enum Sort_Types
    {
        Creation_date_Ascending = 0,
        Creation_date_Descending,
        Importance_Ascending,
        Importance_Descending,
        Due_date_Ascending,
        Due_date_Descending,
        Completed_First,
        Uncommpleted_first,
        Alphabetically_Ascending,
        Alphabeticall_Descendingy

    }

}