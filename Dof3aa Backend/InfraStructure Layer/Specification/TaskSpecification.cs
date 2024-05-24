using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.baseSpecification
{
    public class TaskSpecification: BaseSpecification<task>
    {
        public TaskSpecification(Expression<Func<task, bool>> Criteria): base(Criteria)
        {
            AddInclude(x => x.Cource);
            AddInclude(x => x.PublisherUser);

        }
        public TaskSpecification()
        {
            AddInclude(x => x.PublisherUser);
            AddInclude(x => x.Cource);
        }
    }
}
