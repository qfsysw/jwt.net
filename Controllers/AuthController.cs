using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapijwttest.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapijwttest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILogger<AuthController> _logger = null;
        private JwtHelper2 _iJWTService = null;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, JwtHelper2 jWTService, IConfiguration configuration)
        {
            this._logger = logger;
            _iJWTService = jWTService;
            _configuration = configuration;
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
        {//添加了授权认证，需要使用token
            return new List<object>() { new { userName = "123", remark = "1234" } };
        }

        [Route("Login")]
        [HttpGet]
        public string Login(string name, string password)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
            {
                string token = this._iJWTService.CreateToken(name,password);
                return JsonConvert.SerializeObject(new { result = true, token });
            }
            else
            {
                return JsonConvert.SerializeObject(new { result = false, token = "" });
            }
        }


    }
}
