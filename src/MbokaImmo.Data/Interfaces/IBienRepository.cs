using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;

namespace MBOKA_IMMO.src.MbokaImmo.Data.Interfaces;

public interface IBienRepository
{
    Task<(List<Bien> Items, int Total)> GetAllAsync(int page, int pageSize, string? ville, string? type, decimal? loyerMax);
    Task<Bien?> GetByIdAsync(int idBien);
    Task<List<Bien>> GetByProprietaireAsync(int idProprietaire);
    Task<Bien> AddAsync(Bien bien);
    Task UpdateAsync(Bien bien);
    Task DeleteAsync(Bien bien);
    Task<int> SaveChangesAsync();
}
