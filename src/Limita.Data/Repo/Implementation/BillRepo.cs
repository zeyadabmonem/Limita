using Limita.Data.Entities;
using Limita.Data.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace Limita.Data.Repo.Implementation
{
    public class BillRepo : IBillRepo
    {
        private readonly LimitaDbContext _context;
        public BillRepo(LimitaDbContext context)
        {
            _context = context;
        }

        public List<Bill> GetAllBills(int userId)
        {
            var result = _context.Bills.Include(x => x.User).Where(x => x.UserId == userId).ToList();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public Bill GetBillById(int id)
        {
            var result = _context.Bills.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result;
            }
            return null;
        }
    }
}
