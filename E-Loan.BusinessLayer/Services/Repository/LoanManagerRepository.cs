using E_Loan.DataLayer;
using E_Loan.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Loan.BusinessLayer.Services.Repository
{
    public class LoanManagerRepository : ILoanManagerRepository
    {
        /// <summary>
        /// Creating referance Variable of MongoContext and MongoCollection
        /// </summary>
        private readonly IMongoDBContext _mongoContext;
        private IMongoCollection<LoanMaster> _dbloanCollection;
        private IMongoCollection<LoanApprovaltrans> _dbloanapprovalCollection;
        public LoanManagerRepository(IMongoDBContext context)
        {
            _mongoContext = context;
            _dbloanCollection = _mongoContext.GetCollection<LoanMaster>(typeof(LoanMaster).Name);
            _dbloanapprovalCollection = _mongoContext.GetCollection<LoanApprovaltrans>(typeof(LoanApprovaltrans).Name);
        }
        /// <summary>
        /// Accept loan application before start the loan approval process.
        /// </summary>
        /// <param name="loanId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<bool> AcceptLoanApplication(string loanId, string remark)
        {
            var filter = Builders<LoanMaster>.Filter.Eq(s => s.LoanId, loanId);
            var update = Builders<LoanMaster>.Update
                            .Set(s => s.LStatus, LoanStatus.Accept).Set(s => s.ManagerRemark, remark);
            try
            {
                UpdateResult actionResult = await _dbloanCollection.UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Get list of all loan Application baed on status that is belongs to "Recived"
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoanMaster>> AllLoanApplication()
        {
            try
            {
                return await _dbloanCollection.Find<LoanMaster>(m => m.LStatus == LoanStatus.Received).ToListAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Reject loan application after physical review with remark, before start the loan approval process make again as "Accept".
        /// </summary>
        /// <param name="loanId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<bool> RejectLoanApplication(string loanId, string remark)
        {
            var filter = Builders<LoanMaster>.Filter.Eq(s => s.LoanId, loanId);
            var update = Builders<LoanMaster>.Update
                            .Set(s => s.LStatus, LoanStatus.Rejected).Set(s => s.ManagerRemark, remark);
            try
            {
                UpdateResult actionResult = await _dbloanCollection.UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Start the loan Sanction if loan status is "Accept" and add the all pending amout and all terms,
        /// with makeking loan status is Done
        /// </summary>
        /// <param name="loanApprovaltrans"></param>
        /// <returns></returns>
        public async Task<LoanApprovaltrans> SanctionedLoan(string loanId, LoanApprovaltrans loanApprovaltrans)
        {
            if (loanApprovaltrans == null)
            {
                throw new ArgumentNullException(typeof(LoanApprovaltrans).Name + "Object is Null");
            }
            var filter = Builders<LoanMaster>.Filter.Eq(s => s.LoanId, loanId);
            var update = Builders<LoanMaster>.Update.Set(s => s.LStatus, LoanStatus.Done);
            try
            {
                _dbloanapprovalCollection = _mongoContext.GetCollection<LoanApprovaltrans>(typeof(LoanApprovaltrans).Name);
                await _dbloanapprovalCollection.InsertOneAsync(loanApprovaltrans);
                await _dbloanCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return loanApprovaltrans;
        }
        /// <summary>
        /// Using this methos check the loan status is "Accepted" or not before start loan process.
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        public async Task<LoanMaster> CheckLoanStatus(string loanId)
        {
            try
            {
                var objectId = new ObjectId(loanId);
                FilterDefinition<LoanMaster> filter = Builders<LoanMaster>.Filter.Eq("LoanId", objectId);
                _dbloanCollection = _mongoContext.GetCollection<LoanMaster>(typeof(LoanMaster).Name);
                var findLoan = await _dbloanCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
                if (findLoan.LStatus == LoanStatus.Accept)
                {
                    return findLoan;
                }
                return findLoan;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
