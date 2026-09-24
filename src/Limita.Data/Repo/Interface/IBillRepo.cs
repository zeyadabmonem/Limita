namespace Limita.Data.Repo.Interface;

public interface IBillRepo
{
    /// <summary>Read-only projection for listing. Not tracked.</summary>
    Task<List<Bill>> GetByUserIdAsync(int userId);

    /// <summary>
    /// Tracked lookup scoped to the owning user. Used both for "view details"
    /// and as the entity mutated during Pay (status/PaidAt update).
    /// </summary>
    Task<Bill?> GetByIdAndUserIdAsync(int billId, int userId);
}