using ARW.Admin.WebApi.Controllers;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Framework;
using ARW.Model.System;
using Infrastructure.WeChat.Login;
using Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp.Business.JsonResult;
using Senparc.Weixin.WxOpen;
using Senparc.Weixin;
using Senparc.Weixin.CommonAPIs;
using ARW.Model.Dto.Business.ToolCustomers;
using ARW.Model.Models.Business.ToolCustomers;
using ARW.Service.Business.IBusinessService.ToolCustomers;
using System.Security.Cryptography;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Crypto.Prng;

namespace ARW.WebApi.Controllers.Api
{
    /// <summary>
    /// 工具箱登录控制器
    /// </summary>
    //[Verify]
    [Route("api/[controller]")]
    public class LoginController : BaseController
    {

        private readonly OptionsSetting _jwtSettings;
        private readonly IToolCustomerService _ToolCustomerService;

        public LoginController(IOptions<OptionsSetting> jwtSettings, IToolCustomerService toolCustomerService)
        {
            _jwtSettings = jwtSettings.Value;
            _ToolCustomerService = toolCustomerService;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("getUserInfo")]
        public async Task<IActionResult> GetUserInfo([FromBody] ToolCustomerDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var user = await _ToolCustomerService.GetFirstAsync(s => s.ToolCustomerName == parm.ToolCustomerName);
            return SUCCESSApi(user, "登录成功");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ToolCustomerDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            parm.ToolCustomerPassword = NETCore.Encrypt.EncryptProvider.Md5(parm.ToolCustomerPassword);

            var user = await _ToolCustomerService.GetFirstAsync(s => s.ToolCustomerName == parm.ToolCustomerName && s.ToolCustomerPassword == parm.ToolCustomerPassword);

            if (user == null)
            {
                return SUCCESSApi("用户名或者密码错误");
            }
            else
            {
                LoginUser loginUser = new LoginUser
                {
                    UserId = user.ToolCustomerGuid,
                    UserName = user.ToolCustomerName,
                    IsApi = true,
                };
                var jwt = JwtUtil.GenerateJwtToken(JwtUtil.AddClaims(loginUser), _jwtSettings.JwtSettings);
                var jwtvalue = Convert.ChangeType(jwt, typeof(object));
                return SUCCESSApi(jwtvalue, "登录成功");
            }

        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ToolCustomerDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = new ToolCustomer();
            modal = parm.Adapt<ToolCustomer>().ToCreate(HttpContext);

            var user = await _ToolCustomerService.GetFirstAsync(s => s.ToolCustomerName == parm.ToolCustomerName);

            if (user == null)
            {
                modal.ToolCustomerPassword = NETCore.Encrypt.EncryptProvider.Md5(parm.ToolCustomerPassword);
                var response = await _ToolCustomerService.InsertReturnSnowflakeIdAsync(modal);
                if (response == 0)
                    throw new CustomException("添加失败！");

                var newUser = await _ToolCustomerService.GetFirstAsync(s => s.ToolCustomerGuid == response);
                if (newUser != null)
                {
                    LoginUser loginUser = new LoginUser
                    {
                        UserId = newUser.ToolCustomerGuid,
                        UserName = newUser.ToolCustomerName,
                        IsApi = true,
                    };
                    var jwt = JwtUtil.GenerateJwtToken(JwtUtil.AddClaims(loginUser), _jwtSettings.JwtSettings);
                    var jwtvalue = Convert.ChangeType(jwt, typeof(object));
                    return SUCCESSApi(jwtvalue, "注册成功");
                }
            }
            else
            {
                return SUCCESSApi("用户已存在");
            }

            return SUCCESSApi("");
        }

    }
}
