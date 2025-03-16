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

public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAuthService _service;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IAuthService service, IConfiguration configuration)
    {
        _context = context;
        _service = service;
        _configuration = configuration;
    }

    /// <summary>
    /// 登入
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">登入資訊</param>
    /// <returns>Token</returns>       
    [HttpPost("Login")]
    public async Task<ActionResult<ApiRes<string>>> Login([FromBody] AuthRequest request)
    {
        var res = new ApiRes<Auth>();
        try
        {
            res = await _service.GetUser(request);
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }

    /// <summary>
    /// 註冊
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">註冊資訊</param>
    /// <returns>AuthRequest</returns> 
    [HttpPost("Register")]
    public async Task<ActionResult<ApiRes<string>>> Register([FromBody] AuthRequest request)
    {

        var res = new ApiRes<dynamic>();
        try
        {
            res.Result = true;
            res.Data = await _service.ApplyUser(request);
            res.Msg = "註冊成功";
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }

        return Ok(res);
    }

    /// <summary>
    /// 修改帳號
    /// </summary>
    /// <remarks></remarks>
    /// <param name="request">修改帳號</param>
    /// <returns>AuthRequest</returns> 
    [HttpPut()]
    public async Task<ActionResult<ApiRes<string>>> UpdateAccount([FromBody] AuthChangeRequest request)
    {

        var res = new ApiRes<Auth>();
        try
        {
            res = await _service.UpdateAccount(request);    
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }

        return Ok(res);
    }


    [HttpPost("ValidateToken")]
    public async Task<ActionResult<ApiRes<bool>>> ValidateToken([FromBody] Auth request)
    {
        var res = new ApiRes<bool>();
        try
        {
            res = await _service.ValidateToken(request.Token);
            res.Result = true;
        }
        catch (Exception ex)
        {
            res.HandleError(ex);
        }
        return Ok(res);
    }

}