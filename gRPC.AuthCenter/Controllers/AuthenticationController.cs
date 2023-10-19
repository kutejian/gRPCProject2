using gRPC.AuthCenter.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace gRPC.AuthCenter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IJWTService _iJWTService;
        public AuthenticationController(ILogger<AuthenticationController> logger, IJWTService iJWTService)
        {
            _logger = logger;
            _iJWTService = iJWTService;
        }
        [HttpPost]
        public string Login(string name, string password)
        {
            if ("Steven".Equals(name) && "1234".Equals(password))//应该数据库
            {
                CurrentUserModel currentUser = new CurrentUserModel()
                {
                    Id = 123,
                    Account = "Steven@123123.com",
                    EMail = "11111@qq.com",
                    Mobile = "13312311231",
                    Sex = 1,
                    Age = 18,
                    Name = "Steven",
                    Role = "Admin"
                };

                string token = _iJWTService.GetToken(currentUser);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            return JsonConvert.SerializeObject(new
            {
                result = false,
                token = ""
            });
        }


    }
}