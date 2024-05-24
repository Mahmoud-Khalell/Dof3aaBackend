using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.baseSpecification
{
    public class TopicSpecification: BaseSpecification<Topic>
    {
        public TopicSpecification(Expression<Func<Topic, bool>> Criteria): base(Criteria)
        {
            AddInclude(x => x.Cource);
            AddInclude(x => x.Materials);
            

        }
        public TopicSpecification()
        {
            AddInclude(x => x.Cource);
            AddInclude(x => x.Materials);
        }
    }
}
