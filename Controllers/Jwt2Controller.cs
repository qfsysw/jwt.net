using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapijwttest.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapijwttest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Jwt2Controller : ControllerBase
    {
        private readonly JwtHelper2 _jwt;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="jwtHelper"></param>
        public Jwt2Controller(JwtHelper2 jwtHelper)
        {
            _jwt = jwtHelper;
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetToken(LoginRs user)
        {
            //参数验证等等....
            if (string.IsNullOrEmpty(user.UserId))
            {
                return Ok("参数异常！");
            }
            //这里可以连接mysql数据库做账号密码验证
            //这里可以做Redis缓存验证等等
            //这里获取Token，当然，这里也可以选择传结构体过去
            var token = _jwt.CreateToken(user.UserId, user.PasswordMD5);
            //解密后的Token
            var PWToken = _jwt.JwtDecrypt(token);
            return Ok(token + "解密后：" + PWToken);
        }
        /// <summary>
        /// 获取自己的详细信息，其中 [Authorize] 就表示要带Token才行
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult GetSelfInfo()
        {
            string strtk = "";


            //执行到这里，就表示已经验证授权通过了
            /*
             * 这里返回个人信息有两种方式
             * 第一种：从Header中的Token信息反向解析出用户账号，再从数据库中查找返回
             * 第二种：从Header中的Token信息反向解析出用户账号信息直接返回，当然，在前面创建        Token时，要保存进使用到的Claims中。
            */
            return Ok("授权通过了！");
        }


        [Route("Get")]
        [HttpGet]
        public IEnumerable<int> Get()
        {//未加授权认证
            return new List<int>() { 1, 3, 5, 7, 9 };
        }

        [Route("GetData")]
        [HttpGet]
        [Authorize]
        public List<object> GetData()
        {
            //添加了授权认证，需要使用token
            return new List<object>() { new { userName = "11", remark = "123" } };
        }

    }
}
