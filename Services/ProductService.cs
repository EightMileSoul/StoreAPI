using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Models;


namespace StoreApi.Services;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
        .Include(product => product.Category)
        .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
        .Include(product => product.Category)
        .FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<bool> UpdateAsync(int id, Product updatedProduct)
    {
        var product = await GetByIdAsync(id);

        if (product == null)
            return false;

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.CategoryId = updatedProduct.CategoryId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);

        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}