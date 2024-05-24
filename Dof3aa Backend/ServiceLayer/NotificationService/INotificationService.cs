using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.NotificationService
{
    public interface INotificationService
    {
        Task<int> CreateNotification(Notification notification);
        Task<int> SendNotification(Notification notification,List<string>Users);

        Task<int> MarkAsRead(int NotificationId,string Username);
        Task<IReadOnlyList<UserNotification>> GetByUserName(string userName);
    }
}
