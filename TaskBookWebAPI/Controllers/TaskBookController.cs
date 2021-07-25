using System;
using System.Collections.Generic;
using TaskBook.Models;
using Microsoft.AspNetCore.Mvc;
using TaskBookWebAPI.Services;

namespace TaskBookWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskBookController : ControllerBase
    {

        private readonly TaskListService _taskListService;

        public TaskBookController(TaskListService service)
        {
            _taskListService = service;
        }

        [HttpGet("test")]
        public string Test()
        {
            return "Hello, World!";
        }

        [HttpGet("TaskLists")]
        public ActionResult<List<TaskList>> Get()
        {
            return Ok(_taskListService.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<TaskList> Get([FromRoute] Guid id)
        {
            var tasklist = _taskListService.Get(id);
            if (tasklist != default(TaskList))
            {
                return Ok(tasklist);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("AddOrUpdate")]
        public ActionResult<TaskList> AddOrUpdate([FromBody] TaskList tasklist)
        {
            if (tasklist == null)
            {
                return BadRequest();
            }
            return Ok(_taskListService.AddOrUpdate(tasklist));
        }

        [HttpPost("Delete")]
        public ActionResult<TaskList> Delete([FromBody] Guid id)
        {
            return Ok(_taskListService.Remove(id));
        }

        [HttpPost("{id}/AddOrUpdateTask")]
        public ActionResult<Item> AddOrUpdateTask([FromRoute] Guid id, [FromBody] Task task)
        {
            if (task == null)
            {
                return BadRequest();
            }
            return Ok(_taskListService.AddOrUpdateTask(id, task));

        }

        [HttpPost("{id}/AddOrUpdateAppt")]
        public ActionResult<Item> AddOrUpdateAppt([FromRoute] Guid id, [FromBody] Appointment appt)
        {
            if (appt == null)
            {
                return BadRequest();
            }
            return Ok(_taskListService.AddOrUpdateAppointment(id, appt));
        }

        [HttpPost("{id}/DeleteTask")]
        public ActionResult<Item> DeleteItem([FromRoute] Guid id, [FromBody] Task task)
        {
            return Ok(_taskListService.RemoveTask(id, task));
        }

        [HttpPost("{id}/DeleteAppt")]
        public ActionResult<Item> DeleteItem([FromRoute] Guid id, [FromBody] Appointment appt)
        {
            return Ok(_taskListService.RemoveAppointment(id, appt));
        }

        [HttpGet("search")]
        public ActionResult<TaskList> SearchForTaskList([FromQuery] string name)
        {
            List<TaskList> searchRes = _taskListService.Search(name);
            if (searchRes != default(List<TaskList>))
            {
                return Ok(searchRes);
            }
            else
            {
                return Ok(new List<TaskList>());
            }
        }
    }
}
