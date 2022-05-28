using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHelper.Models;

namespace TaskHelper
{
    public class Helper
    {
            public static TaskHelperContext db = new TaskHelperContext();
            public static User userSession;
            public static Models.Task task;
    }
}
