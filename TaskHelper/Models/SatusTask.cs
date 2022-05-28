using System;
using System.Collections.Generic;

namespace TaskHelper.Models
{
    public partial class SatusTask
    {
        public SatusTask()
        {
            Tasks = new HashSet<Task>();
        }

        public int StatusTaskId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
