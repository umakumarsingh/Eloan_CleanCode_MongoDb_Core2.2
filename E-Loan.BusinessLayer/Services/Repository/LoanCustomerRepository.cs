using E_Loan.DataLayer;
using E_Loan.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace E_Loan.BusinessLayer.Services.Repository
{
    public class LoanCustomerRepository : ILoanCustomerRepository
    {
        /// <summary>
        /// Creating referance Variable of MongoContext and MongoCollection
        /// </summary>
        private readonly IMongoDBContext _mongoContext;
        private IMongoCollection<LoanMaster> _dbloanCollection;
        public LoanCustomerRepository(IMongoDBContext context)
        {
            _mongoContext = context;
            _dbloanCollection = _mongoContext.GetCollection<LoanMaster>(typeof(LoanMaster).Name);
        }
        /// <summary>
        /// Apply mortage and save all data in mongo collection.
        /// </summary>
        /// <param name="loanMaster"></param>
        /// <returns></returns>
        public async Task<LoanMaster> ApplyMortgage(LoanMaster loanMaster)
        {
            if (loanMaster == null)
            {
                throw new ArgumentNullException(typeof(LoanMaster).Name + "Object is Null");
            }
            try
            {
                _dbloanCollection = _mongoContext.GetCollection<LoanMaster>(typeof(LoanMaster).Name);
                await _dbloanCollection.InsertOneAsync(loanMaster);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return loanMaster;
        }
        /// <summary>
        /// Get the loan status by id applied by customer
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        public async Task<LoanMaster> AppliedLoanStatus(string loanId)
        {
            try
            {
                var objectId = new ObjectId(loanId);
                FilterDefinition<LoanMaster> filter = Builders<LoanMaster>.Filter.Eq("LoanId", objectId);
                _dbloanCollection = _mongoContext.GetCollection<LoanMaster>(typeof(LoanMaster).Name);
                return await _dbloanCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Update an existing loan application before sent to loan clerk if loan status is recived update not possible
        /// </summary>
        /// <param name="loanMaster"></param>
        /// <returns></returns>
        public async Task<LoanMaster> UpdateMortgage(string loanId, LoanMaster loanMaster)
        {
            if (loanMaster == null && loanId == null)
            {
                throw new ArgumentNullException(typeof(LoanMaster).Name + "Object is Null");
            }
            var objectId = new ObjectId(loanId);
            FilterDefinition<LoanMaster> filter = Builders<LoanMaster>.Filter.Eq("LoanId", objectId);
            _dbloanCollection = _mongoContext.GetCollection<LoanMaster>(typeof(LoanMaster).Name);
            var loanStstus = await _dbloanCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
            if(loanStstus.LStatus == LoanStatus.Received)
            {
                throw new ArgumentNullException(typeof(LoanMaster).Name + "Loan is Recive not able to update..");
            }
            try
            {
                var update = await _dbloanCollection.FindOneAndUpdateAsync(Builders<LoanMaster>.
                Filter.Eq("LoanId", loanMaster.LoanId), Builders<LoanMaster>.
                Update.Set("LoanName", loanMaster.LoanName).Set("LoanAmount", loanMaster.LoanAmount)
                .Set("Date", loanMaster.Date).Set("BusinessStructure", loanMaster.BusinessStructure)
                .Set("Billing_Indicator", loanMaster.Billing_Indicator).Set("Tax_Indicator", loanMaster.Tax_Indicator)
                .Set("ContactAddress", loanMaster.ContactAddress).Set("Phone", loanMaster.Phone)
                .Set("Email", loanMaster.Email).Set("AppliedBy", loanMaster.AppliedBy)
                .Set("CreatedOn", loanMaster.CreatedOn).Set("ManagerRemark", loanMaster.ManagerRemark)
                .Set("LStatus", loanMaster.LStatus));
                return update;
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }
    }
}
