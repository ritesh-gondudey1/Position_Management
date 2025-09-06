using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Position_Management.DataRepository
{
    public class TransactionRepository
    {
        private readonly PositionManagementDbContext _context;

        public TransactionRepository(PositionManagementDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        // Read (Get by ID)
        public async Task<Transaction?> GetByIdAsync(int transactionId)
        {
            return await _context.Transactions.FindAsync(transactionId);
        }

        // Read (Get TradeID by SecurityCode)
        public async Task<int?> GetTradeIdAsync(Transaction transaction)
        {
            int? tradeId = 0;
            var temp = await _context.Transactions.Where(x => x.SecurityCode.Equals(transaction.SecurityCode)).OrderBy(x => x.TransactionID).FirstOrDefaultAsync();
            if (temp == null)
            {
                tradeId = await _context.Transactions.Select(x => x.TradeID).Distinct().CountAsync() + 1;
            }
            else if (transaction.InsertUpdateCancel.ToUpper() == "INSERT" && transaction.Version == 1) {
                tradeId = await _context.Transactions.Select(x => x.TradeID).Distinct().CountAsync() + 1;
            }
            else if (transaction.InsertUpdateCancel.ToUpper() == "UPDATE" && transaction.Version == 1)
            {
                tradeId = temp.TradeID;
            }
            else
            {
                tradeId = temp.TradeID;
            }
            return tradeId;
        }

        // Read (Get Version by SecurityCode)
        public async Task<int?> GetVersionByInsertUpdateCancelAsync(Transaction transaction)
        {
            var transactions = await _context.Transactions
                                .Where(x => x.SecurityCode == transaction.SecurityCode)
                                .OrderBy(x => x.TransactionID)
                                .ToListAsync();

            if (transaction.InsertUpdateCancel.ToUpper() == "INSERT")
            {
                return 1;
            }
            else if (transaction.InsertUpdateCancel.ToUpper() == "UPDATE")
            {
                var temp = transactions.OrderBy(x => x.TransactionID).LastOrDefault();
                if (temp == null)
                {
                    return 1;
                }
                else
                {
                    return temp.Version + 1;
                }
            }
            else
            {
                var temp = transactions.LastOrDefault();
                if (temp == null)
                    return 1;
                else
                    return temp.Version + 1;
            }
        }

        // Read (Get all)
        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        // Update
        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}