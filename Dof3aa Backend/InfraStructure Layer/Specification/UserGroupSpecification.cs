using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.baseSpecification
{
    public class UserGroupSpecification: BaseSpecification<UserGroup>
    {
        public UserGroupSpecification(Expression<Func<UserGroup, bool>> Criteria): base(Criteria)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Cource);

        }
        public UserGroupSpecification()
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Cource);
        }

    }
}
