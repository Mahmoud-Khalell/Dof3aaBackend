using Core.entities;
using InfraStructure_Layer.baseSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.Specification
{
    public class UserNotificationSpecification:BaseSpecification<Core.entities.UserNotification>
    {
        public UserNotificationSpecification(Expression<Func<UserNotification,bool>> Crateria): base(Crateria)
        {
            AddInclude(x => x.Notification);
            AddInclude(x => x.ReceiverUserName);
            
        }
        public UserNotificationSpecification()
        {
            AddInclude(x => x.Notification);
            AddInclude(x => x.ReceiverUserName);
        }
    }
}
