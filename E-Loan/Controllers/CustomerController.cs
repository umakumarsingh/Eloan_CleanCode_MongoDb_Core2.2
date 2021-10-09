using E_Loan.BusinessLayer.Interfaces;
using E_Loan.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Loan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// Creating the field of ILoanCustomerServices and injecting in CustomerController constructor
        /// </summary>
        private readonly ILoanCustomerServices _customerServices;
        public CustomerController(ILoanCustomerServices loanCustomerServices)
        {
            _customerServices = loanCustomerServices;
        }
        /// <summary>
        /// Get loan status for customer while or before updating loan application.
        /// Get the loan application status by loanId
        /// Check if loan applcant email id is match with loan applied email id, then show loan record.
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("loan-status/{loanId}")]
        public async Task<ActionResult<LoanMaster>> AppliedLoanStatus(string loanId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Get the loan application status by loanId
            var loanStatus = await _customerServices.AppliedLoanStatus(loanId);
            if (loanStatus == null)
            {
                return NotFound();
            }
            return loanStatus;
        }
        /// <summary>
        /// Applay new loan or mortage supply all value for LoanMaster and return object as result of loanmaster object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-mortage")]
        public async Task<ActionResult<LoanMaster>> ApplayMortage([FromBody] LoanMaster model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _customerServices.ApplyMortgage(model);
            return result;
        }
        /// <summary>
        /// Update loan/mortage if not recived by loan clerk then perforn actual update,
        /// Check loan status is recived return a error message, and finnaly return Ok(Result) or ststus code
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loanId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-mortage/{loanId}")]
        public async Task<ActionResult<LoanMaster>> UpdateMortage(string loanId, [FromBody] LoanMaster model)
        {
            if (!ModelState.IsValid && loanId.Equals(""))
            {
                return BadRequest(ModelState);
            }
            var loanUpdate = await _customerServices.AppliedLoanStatus(loanId);
            //Check if loan status is "Recived" not possible to update.
            if (loanUpdate.LStatus == LoanStatus.Received)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Loan Application Status = {loanUpdate.LStatus} cannot be Updated" });
            }
            //Update loan application if it is "not recived"
            if(loanUpdate != null && loanUpdate.LStatus == LoanStatus.NotReceived)
            {
                await _customerServices.UpdateMortgage(loanId, model);
                return Ok(loanUpdate);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            { Status = "Error", Message = $"Loan Id with = {loanId} cannot be Updated" });
        }
    }
}
