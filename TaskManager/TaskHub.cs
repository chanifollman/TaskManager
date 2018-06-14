using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager_data;

namespace TaskManager
{
    public class TaskHub : Hub
    {
        UserTaskRepository repo = new UserTaskRepository(Properties.Settings.Default.Constr);
        public void SendTask (string task)
        {
            var ta = new Task();
            ta.Title = task;
            repo.AddTask(ta);
            var tasks = repo.GetTasks();
            foreach (Task t in tasks)
            {
                if (t.UserId == null)
                {
                    t.User = new User
                    {
                        FirstName = "",
                        LastName = ""
                    };
                }
            }
            Clients.All.AppendTask(tasks.Select(t => new { Id = t.Id, Title = t.Title, Status = t.Status, UserId = t.UserId, FirstName = t.User.FirstName, LastName = t.User.LastName }));
        }
        
        public void SendId(int userId, int taskId )
        {
            repo.SelectTask(userId, taskId);
            var tasks = repo.GetTasks();
            foreach (Task t in tasks)
            {
                if (t.UserId == null)
                {
                    t.User = new User
                    {
                        FirstName = "",
                        LastName = ""
                    };
                }
            }
            Clients.All.AppendTask(tasks.Select(t => new { Id = t.Id, Title = t.Title, Status = t.Status,UserId = t.UserId, FirstName = t.User.FirstName, LastName = t.User.LastName }));
        }
        //public void GetAll()
        //{
        //    SendTask();
        //}
        public void CompleteTask(int taskId)
        {
            repo.CompleteTask(taskId);
            var tasks = repo.GetTasks();
            foreach (Task t in tasks)
            {
                if (t.UserId == null)
                {
                    t.User = new User
                    {
                        FirstName = "",
                        LastName = ""

                    };
                }
            }
            Clients.All.AppendTask(tasks.Select(t => new { Id = t.Id, Title = t.Title, Status = t.Status, UserId = t.UserId, FirstName = t.User.FirstName, LastName = t.User.LastName }));
        }
    }
}
