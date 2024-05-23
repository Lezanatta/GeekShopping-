namespace GeekShopping.ProductAPI.Pagination;
public class ProdutosParameters
{
    const int MaxPageSize = 5;

    public int PageNumber { get; set; } = 1;
    private int _pageSize;
    public int PageSize
    {
        get 
        { 
            return _pageSize; 
        }
        set 
        { 
            _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
