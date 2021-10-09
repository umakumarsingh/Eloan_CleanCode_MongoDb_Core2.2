using E_Loan.DataLayer;
using E_Loan.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Loan.BusinessLayer.Services.Repository
{
    public class LoanClerkRepository : ILoanClerkRepository
    {
        /// <summary>
        /// Creating referance Variable of MongoContext and MongoCollection
        /// </summary>
        private readonly IMongoDBContext _mongoContext;
        private IMongoCollection<LoanMaster> _dbloanMaster;
        private IMongoCollection<LoanProcesstrans> _dbloanprocessCollection;
        public LoanClerkRepository(IMongoDBContext context)
        {
            _mongoContext = context;
            _dbloanMaster = _mongoContext.GetCollection<LoanMaster>(typeof(LoanMaster).Name);
            _dbloanprocessCollection = _mongoContext.GetCollection<LoanProcesstrans>(typeof(LoanProcesstrans).Name);
        }
        /// <summary>
        /// Show/Get all loan application
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoanMaster>> AllLoanApplication()
        {
            try
            {
                return await _dbloanMaster.Find(loanapp => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Show/Get all loan application that status is Not Recived
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoanMaster>> NotReceivedLoanApplication()
        {
            try
            {
                return await _dbloanMaster.Find<LoanMaster>(m => m.LStatus == LoanStatus.NotReceived).ToListAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Start the loan process and add the remening data by loan clerk
        /// </summary>
        /// <param name="loanProcesstrans"></param>
        /// <returns></returns>
        public async Task<LoanProcesstrans> ProcessLoan(LoanProcesstrans loanProcesstrans)
        {
            try
            {
                if (loanProcesstrans == null)
                {
                    throw new ArgumentNullException(typeof(LoanProcesstrans).Name + "Object is Null");
                }
                _dbloanprocessCollection = _mongoContext.GetCollection<LoanProcesstrans>(typeof(LoanProcesstrans).Name);
                await _dbloanprocessCollection.InsertOneAsync(loanProcesstrans);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return loanProcesstrans;
        }
        /// <summary>
        /// Make the loan application as "Recived" before starting loan process using this method
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        public async Task<bool> ReceivedLoan(string loanId)
        {
            var filter = Builders<LoanMaster>.Filter.Eq(s => s.LoanId, loanId);
            var update = Builders<LoanMaster>.Update
                            .Set(s => s.LStatus, LoanStatus.Received);
            try
            {
                UpdateResult actionResult = await _dbloanMaster.UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Find and get all loan application that is recived for loan clerk
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoanMaster>> ReceivedLoanApplication()
        {
            try
            {
                return await _dbloanMaster.Find<LoanMaster>(m => m.LStatus == LoanStatus.Received).ToListAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
