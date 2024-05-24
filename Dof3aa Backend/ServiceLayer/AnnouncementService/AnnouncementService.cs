using Core.entities;
using InfraStructure_Layer.baseSpecification;
using InfraStructure_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.AnnouncementService
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IUnitOfWork unit;

        public AnnouncementService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public async Task<int> CreateAnnouncement(Announcement announcement)
        {
            await unit.Repository<Announcement>().AddAsync(announcement);
            return await unit.Complete();
        }

        public async Task<int> DeleteAnnouncement(int id)
        {
            var announcement =await unit.Repository<Announcement>().GetByIdAsync(id);
            unit.Repository<Announcement>().Delete(announcement);
            return await unit.Complete();
        }

        public async Task<IReadOnlyList<Announcement>> GetAllAnnouncements()
        {
            var spec = new AnnouncementSpecification();
            return await unit.Repository<Announcement>().FindAll(spec);

        }

        public async Task<Announcement> GetAnnouncement(int id)
        {
            var spec= new AnnouncementSpecification(x=>x.Id==id);
            return await unit.Repository<Announcement>().Find(spec);
        }

        public async Task<bool> IsExist(int id)
        {
            var entity= await unit.Repository<Announcement>().GetByIdAsync(id);
            return entity != null;
        }

        public async Task<int> UpdateAnnouncement(Announcement announcement)
        {
            unit.Repository<Announcement>().Update(announcement);
            return await unit.Complete();

        }


        public async Task<IReadOnlyList<Announcement>> GetAllCourceAnnouncement(int CourceId)
        {
            var spec = new AnnouncementSpecification(x => x.CourceId == CourceId);
            return await unit.Repository<Announcement>().FindAll(spec);
        }
    }
}
