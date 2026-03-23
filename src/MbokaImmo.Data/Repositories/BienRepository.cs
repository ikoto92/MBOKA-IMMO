using MBOKA_IMMO.src.MbokaImmo.Data.Interfaces;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Data.Repositories;

public class BienRepository(AppDbContext context) : IBienRepository
{
    public async Task<(List<Bien> Items, int Total)> GetAllAsync(
        int page, int pageSize, string? ville, string? type, decimal? loyerMax)
    {
        var query = context.Biens
            .Include(b => b.Proprietaire).ThenInclude(p => p.Utilisateur)
            .Include(b => b.Photos)
            .Where(b => b.Valide)
            .AsQueryable();

        if (!string.IsNullOrEmpty(ville))
            query = query.Where(b => b.Ville.ToLower().Contains(ville.ToLower()));

        if (!string.IsNullOrEmpty(type) && Enum.TryParse<Domain.Enums.TypeBienEnum>(type, true, out var typeEnum))
            query = query.Where(b => b.Type == typeEnum);

        if (loyerMax.HasValue)
            query = query.Where(b => b.LoyerMensuel <= loyerMax.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.DateCreation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Bien?> GetByIdAsync(int idBien)
        => await context.Biens
            .Include(b => b.Proprietaire).ThenInclude(p => p.Utilisateur)
            .Include(b => b.Photos)
            .FirstOrDefaultAsync(b => b.IdBien == idBien);

    public async Task<List<Bien>> GetByProprietaireAsync(int idProprietaire)
        => await context.Biens
            .Include(b => b.Photos)
            .Where(b => b.IdProprietaire == idProprietaire)
            .OrderByDescending(b => b.DateCreation)
            .ToListAsync();

    public async Task<Bien> AddAsync(Bien bien)
    {
        await context.Biens.AddAsync(bien);
        return bien;
    }

    public Task UpdateAsync(Bien bien)
    {
        context.Biens.Update(bien);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Bien bien)
    {
        context.Biens.Remove(bien);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();
}
