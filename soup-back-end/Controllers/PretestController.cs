using Microsoft.AspNetCore.Mvc;
using soup_back_end.DTOs.Pretest;
using soup_back_end.Data;
using soup_back_end.Models;


namespace soup_back_end.Controllers
{
    public class PretestController : ControllerBase
    {
    
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = new PretestDTO
                {
                    name = "irfan",
                    date = "18/04/2024"
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}
