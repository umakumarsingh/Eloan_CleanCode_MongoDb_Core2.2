﻿using E_Loan.BusinessLayer.Interfaces;
using E_Loan.BusinessLayer.Services.Repository;
using E_Loan.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace E_Loan.BusinessLayer.Services
{
    public class LoanClerkServices : ILoanClerkServices
    {
        /// <summary>
        /// Creating the ILoanClerkRepository field/instance and injecting in LoanClerkServices constuctor
        /// </summary>
        private readonly ILoanClerkRepository _clerkRepository;
        public LoanClerkServices(ILoanClerkRepository loanClerkRepository)
        {
            _clerkRepository = loanClerkRepository;
        }
        /// <summary>
        /// Get/Show all loan application
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoanMaster>> AllLoanApplication()
        {
            //Do code here
            throw new NotImplementedException();
        }
        /// <summary>
        /// Show not recived loan application
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoanMaster>> NotReceivedLoanApplication()
        {
            //Do code here
            throw new NotImplementedException();
        }
        /// <summary>
        /// Start the loan process after all status verifaction by loan clerk
        /// </summary>
        /// <param name="loanProcesstrans"></param>
        /// <returns></returns>
        public async Task<LoanProcesstrans> ProcessLoan(LoanProcesstrans loanProcesstrans)
        {
            //Do code here
            throw new NotImplementedException();
        }
        /// <summary>
        /// Check the status of recived loan application before start the loan process
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        public async Task<bool> ReceivedLoan(string loanId)
        {
            //Do code here
            throw new NotImplementedException();
        }
        /// <summary>
        /// Sho/get all loan application thta is in recived form
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoanMaster>> ReceivedLoanApplication()
        {
            //Do code here
            throw new NotImplementedException();
        }
    }
}
