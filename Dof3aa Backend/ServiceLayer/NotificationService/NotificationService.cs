using Core.entities;
using InfraStructure_Layer.Interfaces;
using InfraStructure_Layer.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork unit;

        public NotificationService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public async Task<int> CreateNotification(Notification notification)
        {
            await unit.Repository<Notification>().AddAsync(notification);
            return await unit.Complete();


        }

        public async Task<IReadOnlyList<UserNotification>> GetByUserName(string userName)
        {
            var spec=new UserNotificationSpecification(x=>x.ReceiverUserName==userName);
            var UN=await unit.Repository<UserNotification>().FindAll(spec);
            return UN;
        }

        public async Task<int> MarkAsRead(int NotificationId,string Username)
        {
            var spec = new UserNotificationSpecification(x=>x.Id==NotificationId && x.ReceiverUserName==Username);
            var UN =await unit.Repository<UserNotification>().Find(spec);
            if (UN == null)
                return 0;
            UN.IsRead = true;
            unit.Repository<UserNotification>().Update(UN);
            return await unit.Complete();


        }

        public async Task<int> SendNotification(Notification notification, List<string> Users)
        {
            foreach(var username in Users)
            {
                var UserNotification = new UserNotification()
                {
                    NotificationId = notification.Id,
                    ReceiverUserName = username
                };

                await unit.Repository<UserNotification>().AddAsync(UserNotification);

            }
            return await unit.Complete();

        }
    }
}
