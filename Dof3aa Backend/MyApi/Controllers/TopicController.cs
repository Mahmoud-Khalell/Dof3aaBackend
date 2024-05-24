using Core.entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.DTO.Material;
using PresentationLayer.DTO.Topic;
using PresentationLayer.Services;
using ServiceLayer.Authservice;
using ServiceLayer.CourceService;
using ServiceLayer.DocumentServices;
using ServiceLayer.NotificationService;
using ServiceLayer.TopicService;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService iTopicService;
        private readonly IauthService iauthService;
        private readonly ICourceService iCourceService;
        private readonly INotificationService iNotificationService;

        public TopicController(ITopicService ITopicService,IauthService IauthService,ICourceService ICourceService,INotificationService INotificationService)
        {
            iTopicService = ITopicService;
            iauthService = IauthService;
            iCourceService = ICourceService;
            iNotificationService = INotificationService;
        }
        
        #region Create Topic
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromForm] NewTopicDTO newTopicDTO)
        {
            var user = await iauthService.GetCurentUser();

            if (user == null)
            {
                return Unauthorized();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var Role=await iCourceService.GetRole( newTopicDTO.CourceId,user.UserName);
            if (Role ==0 || Role==3)
            {
                return Unauthorized();
            }
            
            var topic =await Mapper.TopicDTO2Topic(newTopicDTO);
            await iTopicService.CreateTopicAsync(topic);
            

            return Ok();
        }
        #endregion

        #region Get Topic Info
        [HttpGet("GetInfo")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async  Task<IActionResult> GetInfo(int id)
        {
            var user = await iauthService.GetCurentUser();

            if (user == null)
            {
                return Unauthorized();
            }

            var topic = await iTopicService.GetTopicByIdAsync(id);

            if(topic == null)
            {
                return NotFound("not found");
            }

            var Role = await iCourceService.GetRole(topic.CourseId, user.UserName);
            if (Role == 0)
            {
                return Unauthorized();
            }

            
            
            var topicInfo = Mapper.Topic2TopicInfo(topic);
            return Ok(topicInfo);
        }
        #endregion

        #region Delete Topic
        [HttpDelete("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteTopic(int TopicId)
        {
            var user = await iauthService.GetCurentUser();
            if (user == null)
            {
                return Unauthorized();
            }
            var topic = await iTopicService.GetTopicByIdAsync(TopicId);

            if (topic == null)
            {
                return NotFound();
            }

            var Role = await iCourceService.GetRole(topic.CourseId, user.UserName);

            if (Role == 0 || Role == 3)
            {
                return Unauthorized();
            }

            await iTopicService.DeleteTopicAsync(TopicId);
            return Ok();
        }
        #endregion

        #region Edit Topic
        [HttpPut("Edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit([FromForm] TopicEditDTO topicEditDTO)
        {
            var user = await iauthService.GetCurentUser();

            if (user == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var topic = await iTopicService.GetTopicByIdAsync(topicEditDTO.Id);
            
            if (topic == null)
            {
                return NotFound();
            }

            var Role= await iCourceService.GetRole(topic.CourseId, user.UserName);

            if (Role == 0 || Role == 3)
            {
                return Unauthorized();
            }

            

            if (topicEditDTO.Image != null)
            {
                if (topic.ImageUrl != null)
                {
                    DocumentService.DeleteFile(topic.ImageUrl);
                }
                topic.ImageUrl =await DocumentService.UploadFile(topicEditDTO.Image);
            }
            topic.Title = topicEditDTO.Title;
            topic.Description = topicEditDTO.Description;
            await iTopicService.UpdateTopicAsync(topic);
            
            return Ok();
        }

        #endregion


        #region Get All Topics
        [HttpPost("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll(int CourceId)
        {
            var user = await iauthService.GetCurentUser();

            if (user == null)
            {
                return Unauthorized();
            }

            var Role = await iCourceService.GetRole(CourceId, user.UserName);

            if (Role == 0)
            {
                return Unauthorized();
            }

            var topics = await iTopicService.GetTopicsByCourseIdAsync(CourceId);
            var ans = topics.Select(x => Mapper.Topic2TopicInfo(x)).ToList();
            return Ok(ans);
        }

        #endregion


        #region Add Material

        [HttpPost("AddMaterial")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddMaterial([FromForm] NewMaterialDTO newMaterialDTO)
        {
            var user = await iauthService.GetCurentUser();
            if (user == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var topic =await iTopicService.GetTopicByIdAsync(newMaterialDTO.TopicId);
            if (topic == null)
            {
                return NotFound("This topic isn't found");
            }

            var Role = await iCourceService.GetRole(topic.CourseId, user.UserName);

            if (Role == 0 || Role == 3)
            {
                return Unauthorized();
            }

            var material = await Mapper.NewMaterialDTO2Material(newMaterialDTO);
            await iTopicService.AddMaterialAsync(material);
            var notification = new Notification()
            {
                CreationDate = DateTime.Now,
                publiserUsername = user.UserName,
                description = $"{user.FirstName} has added a new announcement in {topic.Cource.Title} cource"
            };
            await iNotificationService.CreateNotification(notification);
            var users = await iCourceService.GetCourceMenmbers(topic.CourseId);
            var usernames = users.Select(x => x.UserName).ToList();
            iNotificationService.SendNotification( notification,usernames);
            return Ok();





        }

        #endregion

        #region Delete Material

        [HttpPost("DeleteMaterial")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var user =await iauthService.GetCurentUser();
            if (user == null)
            {
                return Unauthorized();
            }
            var material = await iTopicService.GetMaterialAsync(id);
            
            if (material == null)
            {
                return NotFound();
            }
            
            var Role = await iCourceService.GetRole(material.Topic.CourseId, user.UserName);
            if (Role == 0 || Role==3)
            {
                return Unauthorized();
            }

            await iTopicService.DeleteMaterialAsync(id);
            
            return Ok();
        }

        #endregion

        #region Edit Material
        [HttpPut("EditMaterial")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async  Task<IActionResult> EditMaterial([FromForm] EditMaterialDTO materialEditDTO)
        {
            var user = await iauthService.GetCurentUser();
            if (user == null)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var material =await iTopicService.GetMaterialAsync(materialEditDTO.Id);
            if (material == null)
            {
                return NotFound();
            }

            var Role = await iCourceService.GetRole(material.Topic.CourseId, user.UserName);
            if (Role == 0 || Role == 3)
            {
                return Unauthorized();
            }

            

            if (materialEditDTO.Saurce != null)
            {
                DocumentService.DeleteFile(material.FileUrl);
                material.FileUrl =await DocumentService.UploadFile(materialEditDTO.Saurce);
            }
            if(materialEditDTO.Title != null)
            material.Title = materialEditDTO.Title;
            if (materialEditDTO.Description != null)
                material.Description = materialEditDTO.Description;
            await iTopicService.UpdateMaterialAsync(material);
            return Ok();
        }
        #endregion


    }
}
