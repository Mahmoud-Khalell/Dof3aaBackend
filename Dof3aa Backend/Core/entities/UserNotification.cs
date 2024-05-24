using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public class UserNotification: BaseEntity
    {
        [ForeignKey("Notification")]
        public int NotificationId { get; set; }
        [ForeignKey("ReceiverUser")]
        public string ReceiverUserName { get; set; }
        public bool IsRead { get; set; }
        public Notification Notification { get; set; }
        public AppUser ReceiverUser { get; set; }

        public UserNotification()
        {
            IsRead = false;
        }

    }
}
