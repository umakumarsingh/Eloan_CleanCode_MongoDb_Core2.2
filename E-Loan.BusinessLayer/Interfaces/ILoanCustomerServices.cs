using E_Loan.Entities;
using System.Threading.Tasks;

namespace E_Loan.BusinessLayer.Interfaces
{
    public interface ILoanCustomerServices
    {
        Task<LoanMaster> ApplyMortgage(LoanMaster loanMaster);
        Task<LoanMaster> UpdateMortgage(string loanId, LoanMaster loanMaster);
        Task<LoanMaster> AppliedLoanStatus(string loanId);
    }
}
