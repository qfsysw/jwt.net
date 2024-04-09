namespace webapijwttest.Models
{
    /// <summary>
    /// 用户信息类
    /// </summary>
    public class LoginRs
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string PasswordMD5 { get; set; }
    }
}
