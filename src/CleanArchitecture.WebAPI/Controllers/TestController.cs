using CleanArchitecture.WebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers;

[ApiController]
[Route("api/product")]
public class TestController : ControllerBase
{
    [HttpGet("products")]
    [Authorize]
    public async Task<ApiResult<List<string>>> GetProducts()
    {
        var results = new List<string>()
        {
            "product 1",
            "product 2",
            "product 3"
        };
        return ApiResult<List<string>>.Ok(results);
    }
}