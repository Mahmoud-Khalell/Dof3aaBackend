using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.baseSpecification
{
    public class MaterialSpecification: BaseSpecification<Material>
    {
        public MaterialSpecification(Expression<Func<Material,bool>>Criteria): base(Criteria)
        {
            AddInclude(x => x.Topic);
            

        }
        public MaterialSpecification()
        {
            AddInclude(x => x.Topic);

        }
    }
}
