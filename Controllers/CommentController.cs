using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FrightNight.Data;
using FrightNight.Models;
using FrightNight.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static FrightNight.Models.Common;

namespace FrightNight.Controllers;

[ApiController]
[Route("[controller]")]

public class CommentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICommentService _service;
    private readonly IConfiguration _configuration;

    public CommentController(AppDbContext context, ICommentService service, IConfiguration configuration)
    {
        _context = context;
        _service = service;
        _configuration = configuration;
    }

    /// <summary>
    /// 創建貼文
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">貼文資訊</param>
    /// <returns>ApiRes<Comments></returns>       
    [HttpPost()]
    public async Task<ActionResult<ApiRes<Comments>>> Create([FromBody] Comments request)
    {
        var res = new ApiRes<Comments>();
        try
        {
            res.Data = await _service.Create(request);
            res.Result = true;
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }
    /// <summary>
    /// 更新貼文
    /// </summary>
    /// <remarks></remarks>
    /// <param name="id">貼文 ID</param>
    /// <param name="request">更新後的貼文資訊</param>
    /// <returns>ApiRes<Comments></returns>       
    [HttpPut()]
    public async Task<ActionResult<ApiRes<Comments>>> Update([FromBody] Comments request)
    {
        var res = new ApiRes<Comments>();
        try
        {
            // 更新貼文
            await _service.Update(request);
            res.Data = request;
            res.Result = true;
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }

    /// <summary>
    /// 取得貼文
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">Memory ID</param>
    /// <returns>ApiRes<Comments></returns>       
    [HttpGet("{Id}")]
    public async Task<ActionResult<ApiRes<List<Comments>>>> Get(int Id)
    {
        var res = new ApiRes<List<Comments>>();
        try
        {
            res.Data = await _service.Get(Id);
            res.Result = true;
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }

    /// <summary>
    /// 取得貼文
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">貼文ID</param>
    /// <returns>ApiRes<Comments></returns>       
    [HttpDelete("{Id}/{DeleteAt}")]
    public async Task<ActionResult<ApiRes<string>>> Delete(int Id, string DeleteAt)
    {
        var res = new ApiRes<Comments>();
        try
        {
            res.Data = await _service.Delete(Id, DeleteAt);
            res.Result = true;
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }
}