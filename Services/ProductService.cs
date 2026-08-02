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

    public async Task<Product?> CreateAsync(Product product)
    {
        if (product.CategoryId.HasValue)
        {
            var category = await _context.Categories
                .FindAsync(product.CategoryId.Value);

            if (category == null)
                return null;

            product.Category = category;
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }
    public async Task<UpdateProductResult> UpdateAsync(int id, Product updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return UpdateProductResult.ProductNotFound;

        if (updatedProduct.CategoryId.HasValue)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(category =>
                    category.Id == updatedProduct.CategoryId.Value);

            if (!categoryExists)
                return UpdateProductResult.CategoryNotFound;
        }

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.CategoryId = updatedProduct.CategoryId;

        await _context.SaveChangesAsync();

        return UpdateProductResult.Success;
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