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
    public class UserSpecification: BaseSpecification<AppUser>
    {
        public UserSpecification(Expression<Func<AppUser,bool>>Criteria): base(Criteria)
        {
            AddInclude(x => x.UserGroups);
            AddInclude(x => x.UserNotifications);
        }
        public UserSpecification()
        {
            AddInclude(x => x.UserGroups);
            AddInclude(x => x.UserNotifications);
        }
    }
}
