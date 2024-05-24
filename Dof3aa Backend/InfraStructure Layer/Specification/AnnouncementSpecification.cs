using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.baseSpecification
{
    public class AnnouncementSpecification: BaseSpecification<Announcement>
    {
        public AnnouncementSpecification(Expression<Func<Announcement, bool>> Criteria): base(Criteria)
        {
            AddInclude(x => x.Cource);
            AddInclude(x => x.PublisherUser);

        }
        public AnnouncementSpecification()
        {
            AddInclude(x => x.Cource);
            AddInclude(x => x.PublisherUser);
        }
    }
}
