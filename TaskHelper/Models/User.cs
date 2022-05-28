using System;
using System.Collections.Generic;

namespace TaskHelper.Models
{
    public partial class User
    {
        public User()
        {
            TaskAcceptors = new HashSet<Task>();
            TaskCreators = new HashSet<Task>();
        }

        public int UserId { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Task> TaskAcceptors { get; set; }
        public virtual ICollection<Task> TaskCreators { get; set; }
    }
}
