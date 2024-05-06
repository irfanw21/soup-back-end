using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using soup_back_end.Data;
using soup_back_end.DTOs.Invoice;
using soup_back_end.Models;

namespace soup_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly InvoiceData _invoiceData;

        public InvoiceController(Data.InvoiceData invoiceData)
        {
            _invoiceData = invoiceData;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            try
            {
                List<Invoice> invoice = _invoiceData.GetAll();
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByUserId")]
        [Authorize]
        public IActionResult GetByUserId(Guid userid)
        {
            Invoice? invoice = _invoiceData.GetByUserId(userid);

            if (invoice == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(invoice);
        }

        [HttpPost]
        public IActionResult Post([FromBody] InvoiceDTO invoiceDto)
        {

            if (invoiceDto == null)
                return BadRequest("Data Should be Inputed");

            Invoice invoice = new Invoice
            {
                invoiceId = Guid.NewGuid(),
                paymentId = invoiceDto.paymentId,
                userId = invoiceDto.userId,
                invoiceDate = DateTime.Now,
                itemCount = invoiceDto.itemCount,
                totalPaid = invoiceDto.totalPaid
            };

            bool result = _invoiceData.Insert(invoice);

            if (result)
            {
                return StatusCode(201, invoice.invoiceId);
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            bool result = _invoiceData.Delete(id);

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
