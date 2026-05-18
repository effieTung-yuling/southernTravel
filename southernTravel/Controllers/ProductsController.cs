using Microsoft.AspNetCore.Mvc;
using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Services;

namespace southernTravel.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllProductsAsync();
            if (result == null || !result.Any())
                return NotFound("目前尚無產品資料，請新增產品資料。");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetProductByIdAsync(id);
            if (result == null)
                return NotFound($"找不到 ID 為 {id} 的產品資料，請確認後重試。");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.ProductId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
        {
            var success = await _service.UpdateProductAsync(id, dto);

            if (!success)
                return NotFound($"找不到 ID 為 {id} 的產品資料，請確認後重試。");

            return Ok("更新成功");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _service.DeleteProductAsync(id);

            if (!success)
                return NotFound($"找不到 ID 為 {id} 的產品資料，請確認後重試。");

            return Ok("刪除成功");
        }
    }
}