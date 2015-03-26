using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOrg
{
    class TaskList
    {
        string title;
        List<task> listArray;
        public TaskList()
        {
            title = "Title";
            for(int i = 0; i < 2; i++)
            {
                listArray.Add(new task());
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
    }
}
