using Core.entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.DTO.Task;
using PresentationLayer.Services;
using ServiceLayer.Authservice;
using ServiceLayer.CourceService;
using ServiceLayer.DocumentServices;
using ServiceLayer.NotificationService;
using ServiceLayer.TaskService;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService iTaskService;
        private readonly ICourceService iCourceService;
        private readonly IauthService iauthService;
        private readonly INotificationService iNotificationService;
       

        public TaskController(ITaskService ITsakService,ICourceService ICourceService,IauthService IauthService,INotificationService INotificationService)
        {

            iTaskService = ITsakService;
            iCourceService = ICourceService;
            iauthService = IauthService;
            iNotificationService = INotificationService;
            
        }

        #region Create Task
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromForm] TaskDTO taskDTO)
        {
            
            var user =await iauthService.GetCurentUser();
            if (user == null)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Role=await iCourceService.GetRole( taskDTO.CourceId,user.UserName);
            if (Role == 0 || Role ==3)
            {
                return Unauthorized();
            }

            if (taskDTO.DeadLine.Date < System.DateTime.Now.Date)
            {
                return BadRequest("DeadLine Must Be In The Future");
            }

            

            var MyTask = new task()
            {
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                DeadLine = taskDTO.DeadLine,
                CourceId = taskDTO.CourceId,
                
                SaurceUrl =(taskDTO.Saurce!=null)? await DocumentService.UploadFile(taskDTO.Saurce):null,
                PublisherUserName = user.UserName

            };

            await iTaskService.CreateTask(MyTask);

            var crs= await iCourceService.GetCource(taskDTO.CourceId);

            var notification = new Notification()
            {
                publiserUsername = user.UserName,
                CreationDate = System.DateTime.Now,
                description = $"{user.FirstName} has added a new announcement in {crs.Title} cource",
            };
            
            await iNotificationService.CreateNotification(notification); 
            var users = await iCourceService.GetCourceMenmbers(crs.Id);
            var userNames = users.Select(x => x.UserName).ToList();
            await iNotificationService.SendNotification(notification, userNames);
            return Ok();
            

        }
        #endregion

        #region Edit Task
        [HttpPost("Edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit([FromForm] TaskUpdateDTO taskDTO)
        {
            var user = await iauthService.GetCurentUser();
            if (user == null)
            {
                return Unauthorized();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var task = await iTaskService.GetTask(taskDTO.Id);
            
            if (task == null)
            {
                return NotFound();
            }

            var Role= await iCourceService.GetRole(task.CourceId, user.UserName);

            if (Role == 0 || Role == 3)
            {
                return Unauthorized();
            }

            if (taskDTO.DeadLine.Date < System.DateTime.Now.Date)
            {
                return BadRequest("DeadLine Must Be In The Future");
            }


            task.Title = taskDTO.Title;
            task.Description = taskDTO.Description;
            task.DeadLine = taskDTO.DeadLine;
            await iTaskService.UpdateTask(task);
            return Ok();



        }
        #endregion

        #region Delete Task
        [HttpDelete("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var user=await iauthService.GetCurentUser();

            if (user == null)
            {
                return Unauthorized();
            }
            var task = await iTaskService.GetTask(id);
            
            if (task == null)
            {
                return NotFound();
            }

            var Role = await iCourceService.GetRole(task.CourceId, user.UserName);

            if (Role == 0 || Role == 3)
            {
                return Unauthorized();
            }


            
            await iTaskService.DeleteTask(id);
            return Ok();
        }
        #endregion

        #region Get All Tasks

        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll(int CourceId)
        {
            var user=await iauthService.GetCurentUser();
            if (user == null)
            {
                return Unauthorized();
            }

            var Role = await iCourceService.GetRole(CourceId, user.UserName);
            
            if(Role == 0)
            {
                return Unauthorized();
            }

            var tasks= await iTaskService.GetAllCourceTask(CourceId);
;           var taskDTO = tasks.Select(x => Mapper.Task2TaskInfo(x));
            return Ok(taskDTO);

        }
        #endregion




    }
}
