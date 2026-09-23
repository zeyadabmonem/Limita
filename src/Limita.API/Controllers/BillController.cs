using Limita.API.Helper;
using Limita.Business.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Limita.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/bills")]
    public class BillController : Controller
    {
        private readonly IBillService billService;
        public BillController(IBillService billService)
        {
            this.billService = billService;
        }

        [HttpGet]
        public IActionResult GetAllBills()
        {
            var result = billService.GetAll(User.GetUserId());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("{billId}")]
        public IActionResult GetBillById(int billId)
        {
            var result = billService.GetById(User.GetUserId(), billId);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


    }
}
