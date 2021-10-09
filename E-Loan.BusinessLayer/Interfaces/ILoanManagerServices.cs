using E_Loan.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Loan.BusinessLayer.Interfaces
{
    public interface ILoanManagerServices
    {
        Task<IEnumerable<LoanMaster>> AllLoanApplication();
        Task<bool> AcceptLoanApplication(string loanId, string remark);
        Task<bool> RejectLoanApplication(string loanId, string remark);
        Task<LoanApprovaltrans> SanctionedLoan(string loanId, LoanApprovaltrans loanApprovaltrans);
        Task<LoanMaster> CheckLoanStatus(string loanId);
    }
}
