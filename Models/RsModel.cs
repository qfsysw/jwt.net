namespace webapijwttest.Models
{

    /// <summary>
    /// rsmodel
    /// </summary>
    public class RsModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool isOk { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回数据  
        /// </summary>
        public object rsData { get; set; }


    }
}
