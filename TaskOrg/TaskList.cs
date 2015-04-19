using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOrg
{
    public class TaskList
    {
        string title;
        List<task> listArray;
        public TaskList()
        {
            title = "List Title";
            listArray = new List<task>();
            for(int i = 0; i < 2; i++)
            {
                task newTask = new task();
                listArray.Add(newTask);
            }
        }
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }
        public List<task> Tasks
        {
            get
            {
                return listArray;
            }

            set
            {
                listArray = value;
            }
        }
    }
}
