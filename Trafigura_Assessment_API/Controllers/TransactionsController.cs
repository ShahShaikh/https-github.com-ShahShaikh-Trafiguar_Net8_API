using Microsoft.AspNetCore.Mvc;
using TrafiguraAssessment.Application.DTO;
using TrafiguraAssessment.Application.Interface;

namespace Trafigura_Assessment_API.Controllers
{
    [Route("transactions")]
    public class TransactionsController : Controller
    {
        private readonly IPositionService _positionService;
        public TransactionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] TransactionDto dto)
        {
            await _positionService.AddTransactionAsync(dto);
            return Ok();
        }

        [HttpGet("positions")]
        public async Task<IActionResult> GetPositions()
        {
            var positions = await _positionService.GetPositionsAsync();
            return Ok(positions);
        }

       
    }
}
