using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Tracker_API.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionAPIController : ControllerBase
    {
        private readonly ITransaction _repository;

        public TransactionAPIController(ITransaction repository)
        {
            _repository = repository;
        }

        [NonAction]
        private int UserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return 0;
            }
            return int.Parse(userId);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _repository.GetTransactionsAsync(UserId());
            return Ok(transactions);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _repository.GetTransactionAsync(id, UserId());
            return Ok(transaction);
        }
        
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var status = await _repository.DeleteTransactionAsync(id, UserId());
            if (status == false) {
                return StatusCode(404, "Transaction Not Found");
            }
            return NoContent();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionReq newTransaction)
        {
            var isSuccess = await _repository.AddTransactionAsync(newTransaction, UserId());
            return StatusCode(201, "Create Successfully");
        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionReq modifiedTransaction)
        {
            var updatedTransaction = await _repository.UpdateTransactionAsync(id, modifiedTransaction, UserId());

            return Ok(updatedTransaction);
        }
    }
}
