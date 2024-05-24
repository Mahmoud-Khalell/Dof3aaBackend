using Core.entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.DTO.Announcment;
using PresentationLayer.Services;
using ServiceLayer.AnnouncementService;
using ServiceLayer.Authservice;
using ServiceLayer.CourceService;
using ServiceLayer.NotificationService;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly INotificationService iNotService;
        private readonly ICourceService courceService;
        private readonly IauthService iAuthService;
        private readonly IAnnouncementService announcementService;

        public AnnouncementController(INotificationService INotService,ICourceService courceService,IauthService IAuthService, IAnnouncementService announcementService)
        {
            iNotService = INotService;
            this.courceService = courceService;
            iAuthService = IAuthService;
            this.announcementService = announcementService;
        }
        #region Create Announcement
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromForm] AnnouncementDTO ann)
        {
            var user =await  iAuthService.GetCurentUser();
            if(user == null)
                return Unauthorized();
            if(ModelState.IsValid==false)
                return BadRequest();

            if(await courceService.IsExist(ann.CourceId) == false)
                return NotFound("Cource is not found");
            if(await courceService.IsJoined(ann.CourceId,user.UserName)==false)
                BadRequest("You are not joined this cource");

            var Role= await courceService.GetRole(ann.CourceId, user.UserName);
            if (Role==3)
                return Unauthorized("You are not allowed to add announcement in this cource");

            var announcement = new Announcement()
            {
                Title = ann.Title,
                Description = ann.Description,
                CourceId = ann.CourceId,
                PublisherUserName = user.UserName,

            };

            await announcementService.CreateAnnouncement(announcement);
            var cource = await courceService.GetCource(ann.CourceId);
            var notification = new Notification()
            {
                CreationDate = DateTime.Now,
                publiserUsername = user.UserName,
                description=$"{user.FirstName} has added a new announcement in {cource.Title} course",
            };
            await iNotService.CreateNotification(notification);

            var CourceMembers = await courceService.GetCourceMenmbers(ann.CourceId);

            var usernames = CourceMembers.Select(x => x.UserName).ToList();

            await iNotService.SendNotification(notification, usernames);
            

            return Ok();
        }
        #endregion

        #region Get All Announcements
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll(int CourceId)
        {

            var user =await iAuthService.GetCurentUser();
            if (user == null)
                return Unauthorized();
            if (await courceService.IsExist(CourceId) == false)
                return NotFound("Cource is not found");
            if (await courceService.IsJoined(CourceId, user.UserName) == false)
                BadRequest("You are not joined this cource");

          var results=await announcementService.GetAllCourceAnnouncement(CourceId);
            var announcements=results.Select(x => Mapper.Ann2AnnInfoDTO(x));
            return Ok(announcements);
        }
        #endregion

        #region delete Announcement
        [HttpPost("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async  Task<IActionResult> Delete(int id)
        {
            var user =await iAuthService.GetCurentUser();
            if (user == null)
                return Unauthorized();
           
            var announcement =await announcementService.GetAnnouncement(id);

            if (announcement == null) return NotFound();

            if (await courceService.IsJoined(announcement.CourceId, user.UserName) == false)
                return Unauthorized("You are not joined this cource");

            var Role = await courceService.GetRole(announcement.CourceId, user.UserName);
            if (Role == 3)
                return Unauthorized("You are not allowed to delete announcement in this cource");

            var status = await announcementService.DeleteAnnouncement(announcement.Id);

            return Ok();
            
        }
        #endregion

        #region Edit Announcement
        [HttpPost("Edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit([FromForm] AnnouncementEditDTO ann)
        {
            if(ModelState.IsValid == false)
                return BadRequest();
            var user = await iAuthService.GetCurentUser();
            if (user == null)
                return Unauthorized();
            
            if(await announcementService.IsExist(ann.Id) == false)
                return NotFound();

            var announcement = await announcementService.GetAnnouncement(ann.Id);

            var Role = await courceService.GetRole(announcement.CourceId, user.UserName);

            if (Role == 0 ||Role==3)
                return Unauthorized();


            announcement.Title = ann.Title;
            announcement.Description = ann.Description;
            await announcementService.UpdateAnnouncement(announcement);

            return Ok();
        }
        #endregion

    }
}
