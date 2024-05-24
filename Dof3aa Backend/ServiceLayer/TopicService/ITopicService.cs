using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.TopicService
{
    public interface ITopicService
    {
        Task<Topic> GetTopicByIdAsync(int id);
        Task<IEnumerable<Topic>> GetTopicsAsync();
        Task<IEnumerable<Topic>> GetTopicsByCourseIdAsync(int courseId);
        Task<int> CreateTopicAsync(Topic topic);
        Task<int> UpdateTopicAsync(Topic topic);
        Task<int> DeleteTopicAsync(int id);

        Task<int> DeleteMaterialAsync(int id);
        Task<int> AddMaterialAsync(Material material);

        Task<Material> GetMaterialAsync(int id);

        Task<int> UpdateMaterialAsync(Material material);

        Task<IEnumerable<Material>> GetMaterialsByTopicIdAsync(int topicId);

        Task<int> DeleteMateralByTopicAsync(int topicId);

    }
}
