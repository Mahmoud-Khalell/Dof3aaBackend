using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CourceService
{
    public interface ICourceService
    {
        Task<IEnumerable<Cource>> GetCources();
        Task<Cource> GetCource(int id);
        Task<int> AddCource(Cource cource);
        Task<int> UpdateCource(Cource cource);
        Task<int> DeleteCource(Cource cource);
        Task<int> JoinCource(int courceId, string userId,int rule);
        Task<bool> IsJoined(int courceId, string userId);
        Task<bool> IsExist(int courceId);

        Task<int> LeaveCource(int courceId, string userId);
        Task<int> GetRole(int courceId, string userId);

        Task<int> PromoteUser(int courceId, string userId);
        Task<int> DemoteUser(int courceId, string userId);

        Task<IReadOnlyList<AppUser>> GetCourceMenmbers(int courceId);

    }
}
