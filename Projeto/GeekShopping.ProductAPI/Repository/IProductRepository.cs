using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Pagination;
using X.PagedList;

namespace GeekShopping.ProductAPI.Repository;
public interface IProductRepository
{
    Task<IPagedList<Product>> FindAll(ProdutosParameters produtosParameters);
    Task<ProductVO> FindById(long id);
    Task<ProductVO> Create(ProductVO vo);
    Task<ProductVO> Update(ProductVO vo);
    Task<bool> Delete(long id);
}
