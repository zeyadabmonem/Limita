using Limita.Data.Entities;

namespace Limita.Data.Repo.Interface
{
    public interface IBillRepo
    {
        public Bill GetBillById(int id);
        public List<Bill> GetAllBills(int userId);
    }
}
