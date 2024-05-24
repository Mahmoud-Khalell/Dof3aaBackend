using Core.entities;
using InfraStructure_Layer.baseSpecification;
using InfraStructure_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CourceService
{
    public class CourceService : ICourceService
    {
        private readonly IUnitOfWork unit;

        public CourceService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public async Task<int> AddCource(Cource cource)
        {
            await unit.Repository<Cource>().AddAsync(cource);
            return await unit.Complete();
        }


        public async Task<int> DeleteCource(Cource cource)
        {
            unit.Repository<Cource>().Delete(cource);
            return await unit.Complete();

        }

        public async Task<int> DemoteUser(int courceId, string userId)
        {
            var spec = new UserGroupSpecification(x => x.CourceId == courceId && x.Username == userId);
            var usergroup = await unit.Repository<UserGroup>().Find(spec);
            usergroup.rule = 3;
            unit.Repository<UserGroup>().Update(usergroup);
            return await unit.Complete();
        }

        public async Task<Cource> GetCource(int id)=>
            await unit.Repository<Cource>().Find(new CourceSpecification(x=>x.Id==id));
        

        public async Task<IEnumerable<Cource>> GetCources()
        {
            var spec= new CourceSpecification();
            return await unit.Repository<Cource>().FindAll(spec);
        }

        public async Task<int> GetRole(int courceId, string userId)
        {
            var spec = new UserGroupSpecification(x => x.CourceId == courceId && x.Username == userId);
            var usergroup =await unit.Repository<UserGroup>().Find(spec);
            if(usergroup == null)
            {
                return 0;
            }

            return usergroup.rule;
        }

        public async Task<bool> IsExist(int courceId)
        {
            var cource=await unit.Repository<Cource>().GetByIdAsync(courceId);
            return cource != null;
        }

        public async Task<bool> IsJoined(int courceId, string userId)
        {
            var spec = new UserGroupSpecification(x => x.CourceId == courceId && x.Username == userId);
            var usergroup = await unit.Repository<UserGroup>().Find(spec);
            return usergroup != null;

        }

        public async Task<int> JoinCource(int courceId, string userId, int rule)
        {
            var usergroup = new UserGroup
            {
                CourceId = courceId,
                Username = userId,
                rule = rule
            };
            await unit.Repository<UserGroup>().AddAsync(usergroup);
            return await unit.Complete();
        }

        public async Task<int> LeaveCource(int courceId, string userId)
        {
            var usergroup = new UserGroup
            {
                CourceId = courceId,
                Username = userId
            };
            unit.Repository<UserGroup>().Delete(usergroup);
            return await unit.Complete();
        }

        public async Task<int> PromoteUser(int courceId, string userId)
        {
            var spec = new UserGroupSpecification(x => x.CourceId == courceId && x.Username == userId);
            var usergroup = await unit.Repository<UserGroup>().Find(spec);
            usergroup.rule = 2;
            unit.Repository<UserGroup>().Update(usergroup);
            return await unit.Complete();
        }

        public async Task<int> UpdateCource(Cource cource)
        {
            unit.Repository<Cource>().Update(cource);
            return await unit.Complete();
        }


        public async Task<IReadOnlyList<AppUser>> GetCourceMenmbers(int courceId)
        {
            var spec = new UserGroupSpecification(x => x.CourceId == courceId);
            var usergroups = await unit.Repository<UserGroup>().FindAll(spec);
            var users = usergroups.Select(x => x.User).ToList();
            return users;
        }


    }
}
