using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.baseSpecification
{
    public class CourceSpecification:BaseSpecification<Cource>
    {
        public CourceSpecification(Expression<Func<Cource, bool>> Criteria): base(Criteria)
        {
            AddInclude(x => x.Topics);
            AddInclude(x => x.UserGroups);
            AddInclude(x => x.Announcements);
            AddInclude(x => x.Tasks);
            
        }
        public CourceSpecification() : base()
        {
            AddInclude(x => x.Topics);
            AddInclude(x => x.UserGroups);
            AddInclude(x => x.Announcements);
            AddInclude(x => x.Tasks);
        }
    }
}
