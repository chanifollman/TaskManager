using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager_data;

namespace TaskManager.Models
{
    public class HomeVM
    {
        public IEnumerable<Task> Tasks { get; set; }
        public int UserId { get; set; }
    }
}