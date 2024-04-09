using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using webapijwttest.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapijwttest.Controllers
{
    /// <summary>
    /// testjwt token test
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestJwtController : ControllerBase
    {
      

        // POST api/<ValuesController>
        [HttpPost]
        public string Login([FromBody] LoginRs loginRequest)
        {
            if (loginRequest == null) return JsonConvert.SerializeObject(new RsModel() { code = 0, isOk = false, msg = "登录信息为空！" });

            #region  判断userid pwd

            if (loginRequest.UserId != "admin" || loginRequest.PasswordMD5 != "admin")
            {
                return JsonConvert.SerializeObject(new RsModel() { code = 0, isOk = false, msg = "用户名和密码不正确！" });
            }
            #endregion
            LoginInfo Info = new LoginInfo()
            {
                UserId = loginRequest.UserId,
                Pwd = loginRequest.PasswordMD5,
                Expires = DateTime.Now.AddDays(1)
            };
            const string secretKey = "myseckey";//口令加密秘钥
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();//加密方式
            IJsonSerializer serializer = new JsonNetSerializer();//序列化Json
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();//base64加解密
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);//JWT编码
            var token = encoder.Encode(Info, key);//生成令牌

            return JsonConvert.SerializeObject(new RsModel() { code = 1, isOk = true, rsData = token, msg = "登录成功！" });
        }

        [HttpPost]
        public string Login2([FromBody] LoginRs loginRequest)
        {
            if (loginRequest == null) return JsonConvert.SerializeObject(new RsModel() { code = 0, isOk = false, msg = "登录信息为空！" });

            #region  判断userid pwd

            if (loginRequest.UserId != "admin" || loginRequest.PasswordMD5 != "admin")
            {
                return JsonConvert.SerializeObject(new RsModel() { code = 0, isOk = false, msg = "用户名和密码不正确！" });
            }
            #endregion
            LoginInfo Info = new LoginInfo()
            {
                UserId = loginRequest.UserId,
                Expires = DateTime.Now.AddDays(1)
            };
            var token = JwtHelper.Encode(Info);

            return JsonConvert.SerializeObject(new RsModel() { code = 1, isOk = true, rsData = token, msg = "登录成功！" });
        }


        /// <summary>
        /// Jwt解密
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public string TokenDecode(string token)
        {
            string strjs = "";
            RsModel rsm = new RsModel();
            rsm.code = 0;
            rsm.isOk = false; 
            try
            {
                strjs = JwtHelper.Decode(token);
                LoginInfo lgs =  JsonConvert.DeserializeObject<LoginInfo>(strjs);
                int rsint = 0;
                if (lgs != null)
                {
                    if (DateTime.Now < lgs.Expires)
                    {
                        rsint = 1;
                    }
                }
                rsm.isOk=true;
                rsm.code = rsint;// 1;
                rsm.msg = strjs; 
                return JsonConvert.SerializeObject(rsm);
            }
            catch (Exception ex)
            {
              rsm.msg = ex.Message;
              
            }
            return JsonConvert.SerializeObject(rsm);
        }


        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
