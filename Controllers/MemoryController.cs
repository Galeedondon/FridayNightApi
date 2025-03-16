using System;
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

public class MemoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMemoryService _service;
    private readonly IConfiguration _configuration;

    public MemoryController(AppDbContext context, IMemoryService service, IConfiguration configuration)
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
    /// <returns>ApiRes<Memorys></returns>       
    [HttpPost()]
    public async Task<ActionResult<ApiRes<Memorys>>> Create([FromBody] Memorys request)
    {
        var res = new ApiRes<Memorys>();
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
    /// 取得貼文
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">貼文ID</param>
    /// <returns>ApiRes<Memorys></returns>       
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiRes<Memorys>>> Get(int Id)
    {
        var res = new ApiRes<Memorys>();
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
    /// <param name="request">日期</param>
    /// <returns>ApiRes<Memorys></returns>       
    [HttpGet("GetByDate/{date}")]
    public async Task<ActionResult<ApiRes<List<Memorys>>>> GetByDate(DateTime date)
    {
        var res = new ApiRes<List<Memorys>>();
        try
        {
            res.Data = await _service.GetByDate(date);
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
    /// <param name="request">日期</param>
    /// <returns>ApiRes<Memorys></returns>       
    [HttpGet("GetByMonth/{date}")]
    public async Task<ActionResult<ApiRes<List<Memorys>>>> GetByMonth(DateTime date)
    {
        var res = new ApiRes<List<Memorys>>();
        try
        {
            res.Data = await _service.GetByMonth(date);
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
    /// <returns>ApiRes<Memorys></returns>       
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiRes<string>>> Delete(int Id, string deleteAt)
    {
        var res = new ApiRes<Memorys>();
        try
        {
            res.Data = await _service.Delete(Id, deleteAt);
            res.Result = true;
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }

    /// <summary>
    /// 創建貼文圖片
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">圖片</param>
    /// <returns>ApiRes<string></returns>       
    [HttpPost("Picture")]
    public async Task<ActionResult<ApiRes<string>>> CreatePicture(IFormFile avatar)
    {
        var res = new ApiRes<string>();
        try
        {
            if (avatar == null || avatar.Length == 0)
            {
                res.Result = false;
                res.Msg = "No file uploaded.";
                return Ok(res);
            }

            res.Data = await _service.CreatePicture(avatar);
            res.Result = true;

        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }


    /// <summary>
    /// 取得貼文圖片
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">圖片</param>
    /// <returns>ApiRes<FileContentResult></returns>
    [HttpGet("Picture/{Id}")]
    public async Task<IActionResult> GetPicture(string Id)
    {
        var res = new ApiRes<string>();

        try
        {
            var query = await _service.GetPicture(Id);

            if (query == null)
            {
                NotFound();
            }
            string base64String = Convert.ToBase64String(query.FileData);
            res.Data = $"data:image/jpeg;base64,{base64String}";
            res.Result = true;
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }

}