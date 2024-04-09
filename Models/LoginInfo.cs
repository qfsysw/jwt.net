namespace webapijwttest.Models
{ 
    /// <summary>
    /// 用户登录信息类
    /// </summary>
    public class LoginInfo
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public string UserId { get; set; }
        public string Pwd { get; set; }
        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime Expires { get; set; }
}
}
