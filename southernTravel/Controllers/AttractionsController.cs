using Microsoft.AspNetCore.Mvc;
using southernTravel.DTOs;
using southernTravel.Services;

[ApiController]
[Route("api/[controller]")]
public class AttractionsController : ControllerBase
{
    private readonly IAttractionService _service;

    public AttractionsController(IAttractionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        if (result == null || !result.Any())
            return NotFound("目前尚無景點資料，請新增景點資料。");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound($"找不到 ID 為 {id} 的景點資料，請確認後重試。");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAttractionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatedAttractionDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);

        if (!success)
            return NotFound($"找不到 ID 為 {id} 的景點資料，無法進行更新。");

        return Ok("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);

        if (!success)
            return NotFound($"找不到 ID 為 {id} 的景點資料，無法進行刪除。");

        return Ok("刪除成功");
    }
}