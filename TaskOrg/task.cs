using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOrg
{
    class task
    {
        string title;
        string description;
        public task()
        {
            title = "Title";
            description = "Enter a Discription";
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
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }
    }
}
