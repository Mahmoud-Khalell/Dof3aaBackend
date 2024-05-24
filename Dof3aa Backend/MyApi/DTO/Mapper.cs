using Core.entities;
using PresentationLayer.DTO;
using PresentationLayer.DTO.Announcment;
using PresentationLayer.DTO.Cource;
using PresentationLayer.DTO.Material;
using PresentationLayer.DTO.Task;
using PresentationLayer.DTO.Topic;
using ServiceLayer.DocumentServices;
using System.Data;

namespace PresentationLayer.Services
{
    public class Mapper
    {
        
        #region RegisterationDTO2User
        public static async Task<AppUser> RegisDTO2User(RegisterationDTO registerationDTO)
        {
            var user = new AppUser();
            user.Email = registerationDTO.Email;
            user.UserName = registerationDTO.Username;
            user.FirstName = registerationDTO.FirstName;
            user.LastName = registerationDTO.LastName;
            user.Department = registerationDTO.Department;
            user.University = registerationDTO.University;
            user.faculty = registerationDTO.Faculty;
          
            user.ImageUrl =await DocumentService.UploadFile(registerationDTO.Image);
            
            return user;

        }
        #endregion

        #region courceDTO2Cource
        public static async Task<Cource> CourceDTO2Cource(CourceDTO courceDTO)
        {
            var cource = new Cource();
            cource.Id = courceDTO.Id;
            cource.Title = courceDTO.Title ;
            cource.SubTitle = courceDTO.SubTitle;
            cource.Description = courceDTO.Description;
            cource.ImageUrl =await DocumentService.UploadFile(courceDTO.Image);
            cource.LogoUrl =await DocumentService.UploadFile(courceDTO.Logo);
            cource.type = courceDTO.type;
            return cource;
        }
        #endregion


        #region cource2CourceDTO
        public static CourceInfoDTO Cource2CourceInfoDTO(Cource cource)
        {
            var CourceInfo = new CourceInfoDTO();
            CourceInfo.Id = cource.Id;
            
            CourceInfo.Title = cource.Title;
            CourceInfo.SubTitle = cource.SubTitle;
            CourceInfo.Description = cource.Description;
            CourceInfo.Image = cource.ImageUrl;
            CourceInfo.Logo = cource.LogoUrl;
            CourceInfo.type = cource.type;
            return CourceInfo;
        }
        #endregion

        
        #region user2UserDTO
        public static UserDTO User2UserDTO(AppUser user)
        {
            var userDTO = new UserDTO();
            userDTO.UserName = user.UserName;
            userDTO.Email = user.Email;
            userDTO.firstName = user.FirstName;
            userDTO.lastName = user.LastName;
            userDTO.ImageUrl = user.ImageUrl;

            userDTO.Groups = user.UserGroups.Select(e => new
            {
                Rule = e.rule,
                Cource = Cource2CourceInfoDTO(e.Cource)
            }).ToList();


            
            
            return userDTO;
        }
        #endregion

        #region Ann2AnnInfoDTO
        public static Announcement_Info_DTO Ann2AnnInfoDTO(Announcement ann)
        {
            var annInfo = new Announcement_Info_DTO();
            annInfo.Id = ann.Id;
            annInfo.Title = ann.Title;
            annInfo.Description = ann.Description;
            annInfo.CourceId = ann.CourceId;
            annInfo.CreationDate = ann.CreateDate;
            annInfo.PublisherUserName = ann.PublisherUserName;
            
            return annInfo;
        }
        #endregion


        #region Task2TaskInfo
        public static TaskInfo Task2TaskInfo(task task)
        {
            var taskInfo = new TaskInfo();
            taskInfo.Id = task.Id;
            taskInfo.Title = task.Title;
            taskInfo.Description = task.Description;
            taskInfo.DeadLine = task.DeadLine;
            taskInfo.CourceId = task.CourceId;
            taskInfo.PublisherUserName = task.PublisherUserName;
            taskInfo.SaurceUrl = task.SaurceUrl;
            taskInfo.PublishDate = task.CreateDate;
            return taskInfo;
        }
        #endregion

        #region TaskDTO2Task
        public static async Task<Topic> TopicDTO2Topic(NewTopicDTO topicDTO)
        {
            var topic = new Topic();
            topic.Title = topicDTO.Title;
            topic.Description = topicDTO.Description;
            topic.ImageUrl =await DocumentService.UploadFile(topicDTO.Image);
            topic.CourseId = topicDTO.CourceId;
            topic.CretaedAt = System.DateTime.Now;
            
            return topic;
        }
        #endregion

        #region Topic2TopicInfo
        public static TopicInfo Topic2TopicInfo(Topic topic)
        {
            var topicInfo = new TopicInfo();
            topicInfo.Id = topic.Id;
            topicInfo.Title = topic.Title;
            topicInfo.Description = topic.Description;
            topicInfo.ImageUrl = topic.ImageUrl;
            topicInfo.CourceId = topic.CourseId;
            topicInfo.LastUpdate = topic.CretaedAt;
            var LastUpdate=topic.Materials.Select(e=>e.PublishDate   ).Max();
            if(LastUpdate != null && LastUpdate >topicInfo.LastUpdate)
            {
                topicInfo.LastUpdate = LastUpdate;
            }
            topicInfo.Materials = topic.Materials.Select(e => Material2MaterialInfo(e)).ToList();
            return topicInfo;
        }
        #endregion

        #region NewMaterialDTO2Material
        internal static async Task<Material> NewMaterialDTO2Material(NewMaterialDTO newMaterialDTO)
        {
            var material = new Material();
            material.Title = newMaterialDTO.Title;
            material.Description = newMaterialDTO.Description;
            material.FileUrl =await DocumentService.UploadFile(newMaterialDTO.Saurce);
            material.Type = newMaterialDTO.Type;
            material.TopicId = newMaterialDTO.TopicId;
            material.PublishDate = System.DateTime.Now;

            return material;

        }
        #endregion

        #region Material2MaterialInfo
        public static Material_Info Material2MaterialInfo(Material material)
        {
            var materialInfo = new Material_Info();
            materialInfo.Id = material.Id;
            materialInfo.Title = material.Title;
            materialInfo.Description = material.Description;
            materialInfo.SaurceUrl = material.FileUrl;
            materialInfo.Type = material.Type;
            materialInfo.TopicId = material.TopicId;
            materialInfo.CreatedAt = material.PublishDate;
            
            return materialInfo;
        }
        #endregion

        public static async Task<Cource> CourceUpdateDTO2Cource(CourceUpdateDTO cource,Cource crs)
        {
            if (cource.Title != null)
                crs.Title = cource.Title;
            if (cource.SubTitle != null)
                crs.SubTitle = cource.SubTitle;
            if (cource.Description != null)
                crs.Description = cource.Description;
            if (cource.Image != null)
            {
                DocumentService.DeleteFile(crs.ImageUrl);
                crs.ImageUrl =await DocumentService.UploadFile(cource.Image);

            }
            if (cource.Logo != null)
            {
                DocumentService.DeleteFile(crs.LogoUrl);
                crs.LogoUrl =await DocumentService.UploadFile(cource.Logo);
            }
            if (cource.type != null)
                crs.type = cource.type.Value;
            return crs;

        }

    }
}
