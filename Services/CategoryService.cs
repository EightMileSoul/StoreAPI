using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Services;

public class CategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(category => category.Id == id);
    }
    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }
    public async Task<DeleteCategoryResult> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return DeleteCategoryResult.CategoryNotFound;

        var categoryInUse = await _context.Products
            .AnyAsync(product => product.CategoryId == id);

        if (categoryInUse)
            return DeleteCategoryResult.CategoryInUse;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return DeleteCategoryResult.Success;
    }

    public async Task<bool> UpdateAsync(int id, Category updatedCategory)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return false;

        category.Name = updatedCategory.Name;

        await _context.SaveChangesAsync();

        return true;
    }
}