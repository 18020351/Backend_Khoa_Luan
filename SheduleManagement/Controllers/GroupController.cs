using Microsoft.AspNetCore.Mvc;
using SheduleManagement.Data;
using SheduleManagement.Data.Services;
using SheduleManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SheduleManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly ScheduleManagementDbContext _dbContext;
        public GroupController(ScheduleManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost("Update")]
        public IActionResult Update(GroupInfos data)
        {
            try
            {
                var groupService = new GroupService(_dbContext);
                var (msg, groupId) = groupService.Add(data.Creator.Id, data.Name);
                if (msg.Length == 0) return Ok(groupId);
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete/{groupId}")]
        public IActionResult Delete(int groupId)
        {
            try
            {
                string msg = (new GroupService(_dbContext)).Delete(groupId);
                if (msg.Length > 0) BadRequest(msg);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ChangeName")]
        public IActionResult ChangeName(ChangeNameModel model)
        {
            try
            {
                string msg = (new GroupService(_dbContext)).ChangeName(model.Id, model.Name);
                if (msg.Length == 0) return Ok(model.Id);
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var groupService = new GroupService(_dbContext);
                var (msg, groups) = groupService.GetAll();
                if (msg.Length > 0) return BadRequest(msg);
                return Ok(groups.Select(x => new
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    CreatorId = x.CreatorId,
                    Creator = x.Creator,
                    CreatedTime = x.CreatedTime,

                }).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetById/{groupId}")]
        public IActionResult GetById(int groupId)
        {
            try
            {
                var groupService = new GroupService(_dbContext);
                var (msg, group) = groupService.GetById(groupId);
                if (msg.Length > 0) return BadRequest(msg);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
