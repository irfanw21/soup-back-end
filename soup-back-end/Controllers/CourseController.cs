using soup_back_end.Data;
using soup_back_end.Models;
using soup_back_end.DTOs.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;


namespace soup_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseData _courseData;

        public CourseController(Data.CourseData courseData)
        {
            _courseData = courseData;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() 
        {
            try
            {
                List<Course> courses = _courseData.GetAll();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            Course? course = _courseData.GetById(id);

            if (course == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(course);
        }

        [HttpGet("GetByCategoryId")]
        public IActionResult GetByCategoryId(string categoryId)
        {
            Course? course = _courseData.GetByCategoryId(categoryId);

            if (course == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(course);
        }

        [HttpGet("GetByCourse")]
        public IActionResult GetByName(string course_Name)
        {
            Course? course = _courseData.GetByName(course_Name);

            if (course == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(course);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CourseDTO courseDto)
        {

            if (courseDto == null)
                return BadRequest("Data Should be Inputed");

            Course course = new Course
            {
                Id = courseDto.Id,
                categoryId = courseDto.categoryId,
                course_Name = courseDto.course_Name,
                Description = courseDto.Description,
                img = courseDto.img,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };

            bool result = _courseData.Insert(course);

            if (result)
            {
                return StatusCode(201, course.Id);
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }

        [HttpPut]
        public IActionResult Put(string id, [FromBody] CourseDTO courseDto)
        {

            if (courseDto == null)
                return BadRequest("Data Should be Inputed");

            Course course = new Course
            {
                Id = courseDto.Id,
                categoryId = courseDto.categoryId,
                course_Name = courseDto.course_Name,
                Description = courseDto.Description,
                img = courseDto.img,
                Updated = DateTime.Now,
            };

            bool result = _courseData.Update(id, course);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            bool result = _courseData.Delete(id);

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
