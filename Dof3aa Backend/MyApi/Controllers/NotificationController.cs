using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Authservice;
using ServiceLayer.NotificationService;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService iNotificationService;
        private readonly IauthService iauthService;

        public NotificationController(INotificationService INotificationService,IauthService IauthService)
        {
            iNotificationService = INotificationService;
            iauthService = IauthService;
        }

        [HttpGet("GetNotifications")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetNotifications()
        {
            var user = await iauthService.GetCurentUser();
            if (user == null)
                return Unauthorized();

            var notifications =await iNotificationService.GetByUserName(user.UserName);
            var result = notifications.Select(x => new
            {
                NotificationId = x.NotificationId,
                NotificationDescription = x.Notification.description,
                NotificationCreationDate = x.Notification.CreationDate,
                IsRead = x.IsRead,
                NotificationPublisher = x.ReceiverUserName

            }) ;
            return Ok(result);


        }

        [HttpGet("MarkAsRead")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> MarkAsRead(int NotifiationId)
        {
            var user = await iauthService.GetCurentUser();
            if (user == null)
                return Unauthorized();
            
            var status= await iNotificationService.MarkAsRead(NotifiationId, user.UserName);
            if (status == 0)
                return NotFound();
            return Ok();

        }
        
    }
}
