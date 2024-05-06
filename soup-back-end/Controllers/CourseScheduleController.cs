using Microsoft.AspNetCore.Mvc;
using soup_back_end.Data;
using soup_back_end.Models;
using soup_back_end.DTOs.CourseSchedule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace soup_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseScheduleController : Controller
    {
        private readonly CourseScheduleData _courseScheduleData;

        public CourseScheduleController(Data.CourseScheduleData courseScheduleData)
        {
            _courseScheduleData = courseScheduleData;
        }
        [HttpGet("GetAll")]
        
        public IActionResult GetAll()
        {
            try
            {
                List<CourseSchedule> courseSchedule = _courseScheduleData.GetAll();
                return Ok(courseSchedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CourseScheduleDTO courseScheduleDto)
        {

            if (courseScheduleDto == null)
                return BadRequest("Data Should be Inputed");
            
            CourseSchedule courseSchedule = new CourseSchedule
            {
                scheduleId = courseScheduleDto.scheduleId,
                scheduleDate = courseScheduleDto.scheduleDate,
                courseId = courseScheduleDto.courseId,
            };

            bool result = _courseScheduleData.CreateSchedule(courseSchedule);

            if (result)
            {
                return StatusCode(201, courseSchedule.scheduleId);
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            bool result = _courseScheduleData.Delete(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "error occur");
            }
        }
    }
}
