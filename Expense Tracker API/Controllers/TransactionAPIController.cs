using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_API.Controllers
{
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


        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _repository.GetTransactionsAsync();
            return Ok(transactions);
        }



        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _repository.GetTransactionAsync(id);
            return Ok(transaction);
        }
        
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var status = await _repository.DeleteTransactionAsync(id);
            if (status == false) {
                return StatusCode(404, "Transaction Not Found");
            }
            return NoContent();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionReq newTransaction)
        {
            var isSuccess = await _repository.AddTransactionAsync(newTransaction);
            return StatusCode(201, "Create Successfully");
        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionReq modifiedTransaction)
        {
            var updatedTransaction = await _repository.UpdateTransactionAsync(id, modifiedTransaction);

            return Ok(updatedTransaction);
        }
    }
}
