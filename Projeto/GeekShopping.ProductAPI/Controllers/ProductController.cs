using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Pagination;
using GeekShopping.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace GeekShopping.ProductAPI.Controllers;

[Route("api/v1")]
[ApiController]
public class ProductController(IProductRepository _repository, IMapper _mapper) : ControllerBase
{

    [HttpGet("product/{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductVO>>FindById(long id)
    {
        var product = await _repository.FindById(id);

        if (product is null) return NotFound();

        return Ok(product);
    }

    [HttpGet("product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll([FromQuery] ProdutosParameters produtosParameters)
    {
        var products = await _repository.FindAll(produtosParameters);

        var metaData = new
        {
            products.Count,
            products.PageSize,
            products.PageCount,
            products.TotalItemCount,
            products.HasNextPage,
            products.HasPreviousPage,
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

        var productsDto = _mapper.Map<IEnumerable<ProductVO>>(products);

        return Ok(productsDto);
    }

    [HttpPost("product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductVO>> Create([FromBody] ProductVO vo)
    {
        if (vo is null)
            return BadRequest();

        var product = await _repository.Create(vo);

        return Ok(product);
    }

    [HttpPut("product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductVO>> Update([FromBody] ProductVO vo)
    {
        if (vo is null)
            return BadRequest();

        var product = await _repository.Update(vo);

        return Ok(product);
    }

    [HttpDelete("product/{id:long}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _repository.Delete(id);

        return (result) ? Ok(result) : BadRequest();
    }
}
