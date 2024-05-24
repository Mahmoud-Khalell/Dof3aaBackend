using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.AnnouncementService
{
    public interface IAnnouncementService
    {
        Task<int> CreateAnnouncement(Announcement announcement);
        Task<int> DeleteAnnouncement(int id);
        Task<int> UpdateAnnouncement(Announcement announcement);
        Task<Announcement> GetAnnouncement(int id);
        Task<IReadOnlyList<Announcement>> GetAllAnnouncements();
        Task<bool> IsExist(int id);

        Task<IReadOnlyList<Announcement>> GetAllCourceAnnouncement(int CourceId);


    }
}
