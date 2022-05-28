using System;
using System.Collections.Generic;

namespace TaskHelper.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string Name { get; set; } = null!;
        public string Describtion { get; set; } = null!;
        public DateTime PublicDate { get; set; }
        public int CreatorId { get; set; }
        public int AcceptorId { get; set; }
        public int StatusTaskId { get; set; }

        public virtual User Acceptor { get; set; } = null!;
        public virtual User Creator { get; set; } = null!;
        public virtual SatusTask StatusTask { get; set; } = null!;
    }
}
