using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using GeekShopping.ProductAPI.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace GeekShopping.ProductAPI.Repository;
public class ProductRepository(MyContext _context, IMapper _map) : IProductRepository
{
    public async Task<IPagedList<Product>> FindAll(ProdutosParameters produtosParameters)
    {
        var products = await _context.Products.OrderBy(p => p.Name).ToListAsync();

        var produtosFiltrados = await products.ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);

        return produtosFiltrados;
    }

    public async Task<ProductVO> FindById(long id)
    {
        var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

        return _map.Map<ProductVO>(product);
    }

    public async Task<ProductVO> Create(ProductVO vo)
    {
        ArgumentNullException.ThrowIfNull(vo);

        var product = _map.Map<Product>(vo);

        _context.Add(product);

        await _context.SaveChangesAsync();

        return _map.Map<ProductVO>(product);
    }
    public async Task<ProductVO> Update(ProductVO vo)
    {
        ArgumentNullException.ThrowIfNull(vo);

        var product = _map.Map<Product>(vo);

        _context.Update(product);

        await _context.SaveChangesAsync();

        return _map.Map<ProductVO>(product);
    }

    public async Task<bool> Delete(long id)
    {
        var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

        if (product is null)
            return false;

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return true;
    }
}
