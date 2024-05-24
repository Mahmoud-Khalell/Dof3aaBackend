using Core.entities;
using InfraStructure_Layer.baseSpecification;
using InfraStructure_Layer.Interfaces;
using ServiceLayer.DocumentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.TopicService
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork unit;

        public TopicService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public async Task<int> CreateTopicAsync(Topic topic)
        {
           await unit.Repository<Topic>().AddAsync(topic);
            return await unit.Complete();



        }

        public async Task<int> DeleteTopicAsync(int id)
        {
            var topic = await GetTopicByIdAsync(id);
            if (topic == null)
            {
                return 0;
            }
            await DeleteMateralByTopicAsync(id);
            unit.Repository<Topic>().Delete(topic);
            return await unit.Complete();



        }

        public async Task<Topic> GetTopicByIdAsync(int id)
        {
            var spec=new TopicSpecification(e=> e.Id == id);
            return await unit.Repository<Topic>().Find(spec);

        }

        public async Task<IEnumerable<Topic>> GetTopicsAsync()
        {
            var spec = new TopicSpecification();
            return await unit.Repository<Topic>().FindAll(spec);
        }

        public async Task<IEnumerable<Topic>> GetTopicsByCourseIdAsync(int courseId)
        {
            var spec = new TopicSpecification(e => e.CourseId == courseId);
            return await unit.Repository<Topic>().FindAll(spec);
        }

        public async Task<int> UpdateTopicAsync(Topic topic)
        {
            unit.Repository<Topic>().Update(topic);
            return await unit.Complete();

        }


        public async Task<int> DeleteMaterialAsync(int id)
        {
            var material = await unit.Repository<Material>().Find(new MaterialSpecification(e => e.Id == id));
            if (material == null)
            {
                return 0;
            }
            if(material.FileUrl != null)
            {
                DocumentService.DeleteFile(material.FileUrl);
            }
            unit.Repository<Material>().Delete(material);
            return await unit.Complete();

        }
        public async Task<int> AddMaterialAsync(Material material)
        {
            await unit.Repository<Material>().AddAsync(material);
            return await unit.Complete();

        }


        public async Task<Material> GetMaterialAsync(int id)
        {
            var spec= new MaterialSpecification(e => e.Id == id);
            return await unit.Repository<Material>().Find(spec);

        }


        public async Task<int> UpdateMaterialAsync(Material material)
        {
            unit.Repository<Material>().Update(material);
            return await unit.Complete();

        }


        public async Task<IEnumerable<Material>> GetMaterialsByTopicIdAsync(int topicId)
        {
            var spec = new MaterialSpecification(e => e.TopicId == topicId);
            return await unit.Repository<Material>().FindAll(spec);

        }


        public async Task<int> DeleteMateralByTopicAsync(int topicId)
        {
            var materials = await GetMaterialsByTopicIdAsync(topicId);
            if (materials == null)
            {
                return 0;
            }
            foreach (var material in materials)
            {
                if (material.FileUrl != null)
                {
                    DocumentService.DeleteFile(material.FileUrl);
                }
                unit.Repository<Material>().Delete(material);
            }
            return await unit.Complete();

        }
    }
}
