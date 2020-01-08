using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ToDo.Controllers;
using ToDo.Models;

namespace ToDoTests
{
    [TestClass]
    public class UnitTest1
    {
        #region Initialization helpers
        ToDoContext db = new ToDoContext();
        HomeController controller = new HomeController();


        Task task = new Task { Title = "Test Task" };
        public void InitializationTask()
        {
            db.Tasks.Add(task);
            db.SaveChanges();
            task = db.Tasks.First(t => t.Title == task.Title);
        }
        CustomList list = new CustomList { Title = "Test List" };
        void InitializationList()
        {
            db.CustomLists.Add(list);
            db.SaveChanges();
            list = db.CustomLists.First(t => t.Title == list.Title);
        }

        #endregion

        #region Return View Verification       
        [TestMethod]
        public void TestIndexView()
        {
            var result = controller.Index(null) as ViewResult;
            Assert.AreEqual("", result.ViewName);
        }
        #region For Tasks
       
        [TestMethod]
        public void TestCreateTakGetView()
        {
            var result = controller.CreateTask(1) as ViewResult;
            Assert.AreEqual("", result.ViewName);
        }
        [TestMethod]
        public void TestCreateTaskPostView()
        {
            task = new Task { Title = "Test Task" };

            var result = controller.CreateTask(task) as RedirectToRouteResult;
            Assert.AreEqual("", result.RouteName);
            RemoveTask();
        }
        [TestMethod]
        public void TestDeleteTaskView()
        {
            InitializationTask();

            var result = controller.DeleteTask(task.Id) as ViewResult;
            Assert.AreEqual("", result.ViewName);
            RemoveTask();
        }
        [TestMethod]
        public void TestDeleteTaskPostView()
        {
            InitializationTask();
            var result = controller.DeleteTask(task) as RedirectToRouteResult;
            Assert.AreEqual("", result.RouteName);            
        }
        [TestMethod]
        public void TestDetailsTaskView()
        {
            InitializationTask();

            var result = controller.DetailsTask(task.Id) as ViewResult;

            Assert.AreEqual("", result.ViewName);

            RemoveTask();
        }
        [TestMethod]
        public void TestEditTaskGetView()
        {
            InitializationTask();

            var result = controller.EditTask(task.Id) as ViewResult;

            Assert.AreEqual("", result.ViewName);
            RemoveTask();
        }
        [TestMethod]
        public void TestEditTaskPostView()
        {
            InitializationTask();

            var result = controller.EditTask(task) as RedirectToRouteResult;

            Assert.AreEqual("", result.RouteName);
            RemoveTask();
        }
        [TestMethod]
        public void TestMultiItemsDeleteGetView()
        {

            var result = controller.MultiItemsDelete() as ViewResult;
            Assert.AreEqual("", result.ViewName);
        }
        [TestMethod]
        public void TestMultiItemsDeletePostView()
        {
            var items = new List<int> { 1, 2 };

            var result = controller.MultiItemsDelete(items) as RedirectToRouteResult;
            Assert.AreEqual("", result.RouteName);
        }
        [TestMethod]
        public void TestSearchTasksView()
        {

            var result = controller.SearchTasks() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
        [TestMethod]
        public void TestListOfTasksView()
        {
            var tasks = db.Tasks.ToList();

            var result = controller.ListOfTasks(tasks, true) as PartialViewResult;
            Assert.AreEqual("ListOfTasks", result.ViewName);
        }
       

        #endregion

        #region For Lists
       
        [TestMethod]
        public void TestCreateListGetView()
        {

            var result = controller.CreateList() as ViewResult;
            Assert.AreEqual("", result.ViewName);

        }
        [TestMethod]
        public void TestCreateListPostView()
        {
            list = new CustomList { Title = "Test List" };

            var result = controller.CreateList(list) as RedirectToRouteResult;
            Assert.AreEqual("", result.RouteName);
            RemoveList();

        }
        [TestMethod]
        public void TestDeleteListGetView()
        {
            InitializationList();

            var result = controller.DeleteList(list.Id) as ViewResult;

            Assert.AreEqual("", result.ViewName);
            RemoveList();
        }
        [TestMethod]
        public void TestDeleteListPostView()
        {
            InitializationList();

            var result = controller.DeleteList(list) as RedirectToRouteResult;

            Assert.AreEqual("", result.RouteName);
        }
        [TestMethod]
        public void TestDeailsListView()
        {
            InitializationList();

            var result = controller.DetailsList(list.Id) as ViewResult;

            Assert.AreEqual("", result.ViewName);
            RemoveList();
        }
        [TestMethod]
        public void TestEditListGetView()
        {
            InitializationList();


            var result = controller.EditList(list.Id) as ViewResult;

            Assert.AreEqual("", result.ViewName);
            RemoveList();

        }
        [TestMethod]
        public void TestEditListPostView()
        {
            InitializationList();


            var result = controller.EditList(list) as RedirectToRouteResult;

            Assert.AreEqual("", result.RouteName);
            RemoveList();

        }
        [TestMethod]
        public void TestSettingsListGetView()
        {

            var result = controller.SettingsList(1) as ViewResult;

            Assert.AreEqual("", result.ViewName);
        }
        [TestMethod]
        public void TestSettingsListPostView()
        {
            var smartlist = db.SmartLists.First();

            var result = controller.SettingsList(smartlist) as RedirectToRouteResult;

            Assert.AreEqual("", result.RouteName);
        }
        [TestMethod]
        public void TestDetailsSmartListView()
        {

            var result = controller.DetailsSmartList("Today") as ViewResult;

            Assert.AreEqual("DetailsList", result.ViewName);
        }
        [TestMethod]
        public void TestListOfListsView()
        {
            var result = controller.ListOfLists() as PartialViewResult;

            Assert.AreEqual("ListOfLists", result.ViewName);
        }
        
        #endregion
        #endregion

        #region DB Functionality Verification
        [TestMethod]
        public void TestCreateTask()
        {
            Task task = new Task { Title = "Test Task" };
            controller.CreateTask(task);
            var tasks = db.Tasks.ToList();
            task = tasks.Find(t => t.Title == task.Title);
            var result = tasks.Contains(task);
            Assert.IsTrue(result);
            RemoveTask(task.Id);
        }
        [TestMethod]
        public void TestEditTask()
        {
            InitializationTask();
            Task newtask = new Task { Id = task.Id, Title = "Test Task 1"};
            controller.EditTask(newtask);
            db = new ToDoContext();
            var tasks = db.Tasks.ToList();
            var result = tasks.Last(t => t.Id == task.Id).Title == newtask.Title;
            Assert.IsTrue(result);
            RemoveTask(newtask.Id);
        }
        [TestMethod]
        public void TestDeleteTask()
        {
            InitializationTask();         
            controller.DeleteTask(task);
            db = new ToDoContext();
            var result = db.Tasks.ToList().LastOrDefault(t=>t.Id==task.Id);
            Assert.IsNull(result);
        }
        [TestMethod]
        public void TestCreateList()
        {
            CustomList list = new CustomList { Title = "Test List" };
            controller.CreateList(list);
            var lists = db.CustomLists.ToList();
            list = lists.Find(l => l.Title == list.Title);
            var result = lists.Contains(list);
            Assert.IsTrue(result);
            RemoveList(list.Id);
        }
        [TestMethod]
        public void TestEditList()
        {
            InitializationList();
            CustomList newlist = new CustomList { Id = list.Id, Title = "Test List 1"};
            controller.EditList(newlist);
            db = new ToDoContext();
            var lists = db.CustomLists.ToList();
            var result = lists.Last(l => l.Id == list.Id).Title == newlist.Title;
            Assert.IsTrue(result);
            RemoveList(newlist.Id);
        }
        [TestMethod]
        public void TestDeleteList()
        {
            InitializationList();
            controller.DeleteList(list);
            var result = db.CustomLists.ToList().LastOrDefault(l => l.Id == list.Id);
            Assert.IsNull(result);
        }
        [TestMethod]
        public void TestSettingsList()
        {
            SmartList list = new SmartList { Title = "Test List" ,HideCompleted=true};
            db.SmartLists.Add(list);
            db.SaveChanges();           
            SmartList newlist = new SmartList { Id = list.Id, Title = "Test List",HideCompleted=false };
            controller.SettingsList(newlist);
            db = new ToDoContext();
            var lists = db.SmartLists.ToList();
            newlist = lists.Last(l => l.Id == list.Id);
            var result = newlist.HideCompleted != list.HideCompleted;
            Assert.IsTrue(result);                        
            //list = db.SmartLists.ToList().Last(l => l.Id == list.Id);
            db.Entry(newlist).State = EntityState.Deleted;
            db.SaveChanges();
        }
        #endregion

        #region Removal helpers
        public void RemoveTask()
        {
            db.Entry(task).State = EntityState.Deleted;
            db.SaveChanges();
        }
        public void RemoveTask(int id)
        {
            db = new ToDoContext();
            Task task = db.Tasks.ToList().Last(t => t.Id == id);
            db.Entry(task).State = EntityState.Deleted;
            db.SaveChanges();
        }
        void RemoveList()
        {
            db.Entry(list).State = EntityState.Deleted;
            db.SaveChanges();
        }
        void RemoveList(int id)
        {
            db = new ToDoContext();
            CustomList list = db.CustomLists.ToList().Last(l => l.Id == id);
            db.Entry(list).State = EntityState.Deleted;
            db.SaveChanges();
        }
        #endregion
    }
}
