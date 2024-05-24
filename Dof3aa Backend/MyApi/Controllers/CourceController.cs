using Core.entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.DTO;
using PresentationLayer.DTO.Cource;
using PresentationLayer.Services;
using ServiceLayer.Authservice;
using ServiceLayer.CourceService;
using System.Reflection.Metadata;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CourceController : ControllerBase
    {
        private readonly ICourceService courceService;
        private readonly IauthService authService;

        public CourceController(ICourceService courceService,IauthService AuthService)
        {
            this.courceService = courceService;
            authService = AuthService;
        }


        #region Create Cource
        [HttpPost("CreateCource")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCource([FromForm] CourceDTO cource)
        {
            var user =await  authService.GetCurentUser();
            if (user == null)
                return Unauthorized();

            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            
            var crs=courceService.GetCource(cource.Id);
            if (crs != null)
                return BadRequest("Group Id already exists");

            var NewCource =await Mapper.CourceDTO2Cource(cource);

            await courceService.AddCource(NewCource);
            await courceService.JoinCource(NewCource.Id, user.UserName, 1);
            return Ok();
        }
        #endregion


        #region Join to Cource
        [HttpGet("Join")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async  Task<IActionResult> Join(int? CourceId)
        {

            var user =await  authService.GetCurentUser();
            if (user == null)
                return Unauthorized();

            if (CourceId == null)
                return BadRequest();


            if (await courceService.IsExist(CourceId.Value)==false )
                return BadRequest("Cource does not exist");

            if (await courceService.IsJoined(CourceId.Value,user.UserName)==false)
                return BadRequest("You are already in this group");

             await courceService.JoinCource(CourceId.Value, user.UserName, 3);
            return Ok();


        }
        #endregion

        #region Leave Cource
        [HttpGet("LeaveCource")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LeaveCource(int? CourceId)
        {

            if (CourceId == null)
                return BadRequest();

            var user=await authService.GetCurentUser();

            if (user == null)
                return Unauthorized();
            if(await courceService.IsExist(CourceId.Value) == false)
                return BadRequest("You are not in this group");
            var role = await courceService.GetRole(CourceId.Value, user.UserName);
            if (role == 1)
                return BadRequest("You can't leave this group");
            var state=await courceService.LeaveCource(CourceId.Value, user.UserName);
            if (state !=0)
                return Ok();
            else
                return BadRequest();
            


        }
        #endregion

        #region Delete Cource

        [HttpDelete("DeleteCource")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteCource(int? CourceId)
        {
            if (CourceId == null)
                return BadRequest();

            var user=await authService.GetCurentUser();
            if (user == null)
                return Unauthorized();
            if (await courceService.IsJoined(CourceId.Value, user.UserName)) 
                return BadRequest("You are not in this group");

            var role = await courceService.GetRole(CourceId.Value, user.UserName);
            if (role != 1)
                return BadRequest("You can't delete this group");

            var crs =await courceService.GetCource(CourceId.Value);
            
            
            var state = await courceService.DeleteCource(crs);
            if (state != 0)
                return Ok();
            else
                return BadRequest();

        }
        #endregion

        

        #region Promote User
        [HttpPost("Promote")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Promote(int? CourceId, string UserName)
        {
            if (CourceId == null || UserName == null)
                return BadRequest();

            var user = await authService.GetCurentUser();
            if(user==null)
                return Unauthorized();

            if (await courceService.IsJoined(CourceId.Value, user.UserName) == false)
                return BadRequest("You are not joined in this cource");

            var role = await courceService.GetRole(CourceId.Value, user.UserName);
            if (role != 1)
                return BadRequest("You can't promote user");

            var role2 = await courceService.GetRole(CourceId.Value, UserName);
            if (role2 == 1)
                return BadRequest("User is already The creator");
            else if (role2 == 2)
                return BadRequest("User is already an admin");

            var status= await courceService.PromoteUser(CourceId.Value, UserName);
            if (status != 0)
                return Ok();
            else
                return BadRequest();

        }

        #endregion

        #region Demote User
        [HttpPost("Demote")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Demote(int? CourceId, string UserName)
        {
            if (CourceId == null || UserName == null)
                return BadRequest();

            var user = await authService.GetCurentUser();
            if (user == null)
                return Unauthorized();

            if (await courceService.IsJoined(CourceId.Value, user.UserName) == false)
                return BadRequest("You are not joined in this cource");

            var role = await courceService.GetRole(CourceId.Value, user.UserName);
            if (role != 1)
                return BadRequest("You can't promote user");

            var role2 = await courceService.GetRole(CourceId.Value, UserName);
            if (role2 == 1)
                return BadRequest("Creator can't be demoted");
            else if (role2 == 3)
                return BadRequest("User is already an usual user");

            var status = await courceService.DemoteUser(CourceId.Value, UserName);
            if (status != 0)
                return Ok();
            else
                return BadRequest();

        }
        #endregion

        #region Get Cource Info
        [HttpGet("GetInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetInfo(int? CourceId)
        {

            var user =await authService.GetCurentUser();
            if (user == null || CourceId==null)
                return Unauthorized();

            if(await courceService.IsExist(CourceId.Value) == false)
                return BadRequest("Cource not found");

            if (await courceService.IsJoined(CourceId.Value, user.UserName) == false)
                return BadRequest("You are not in this group");

            
            var Cource =await courceService.GetCource(CourceId.Value);
            if (Cource == null)
                return BadRequest("Cource not found");
            var res = new
            {
                CourceInfo = Mapper.Cource2CourceInfoDTO(Cource),
                Users = Cource.UserGroups.Select(x=>new
                {
                    username=x.Username,
                    name=x.User.FirstName + " " + x.User.LastName,
                    rule = x.rule,
                    ImageUrl= x.User.ImageUrl

                })
            };
            return Ok(res);
        }
        #endregion

        #region Edit Cource
        [HttpPost("EditCource")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EditCource([FromForm] CourceUpdateDTO cource)
        {
            var user= await authService.GetCurentUser();
            if (user == null)
                return Unauthorized();

            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            if (await courceService.IsExist(cource.Id) == false)
                return BadRequest("This group does not exist");

            if(await courceService.IsJoined(cource.Id, user.UserName) == false)
                return BadRequest("You are not in this group");

            var Role = await courceService.GetRole(cource.Id, user.UserName);
            if (Role != 1)
                return BadRequest("You can't edit this group");

            var crs= await courceService.GetCource(cource.Id);
            
            crs=await Mapper.CourceUpdateDTO2Cource(cource, crs);
            var status = await courceService.UpdateCource(crs);
            if (status != 0)
                return Ok();
            else
                return BadRequest("edit was not colmpleted successfully");

        }
        #endregion


    }
}
