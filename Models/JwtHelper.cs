using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Newtonsoft.Json;

namespace webapijwttest.Models
{
    public static class JwtHelper
    {

        private static readonly string JwtKey = "mysecret";
        /// <summary>
        /// 获取加密解密
        /// </summary>
        /// <returns></returns>
        private static IJwtEncoder GetEncoder()
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();//加密方式
            IJsonSerializer serializer = new JsonNetSerializer();//序列化Json
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();//base64加解密
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);//JWT编码
            return encoder;
        }

        /// <summary>
        /// 获取解密密钥
        /// </summary>
        /// <returns></returns>
        private static IJwtDecoder GetDecoder()
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            return decoder;
        }

        /// <summary>
        /// 加密
        /// </summary>
        public static string Encode(object payload)
        {
            var encoder = GetEncoder();
            var token = encoder.Encode(payload, JwtKey);
            return token;
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static T Decode<T>(string token)
        {
            var decoder = GetDecoder();
            var data = decoder.Decode(token, JwtKey);
            var res = JsonConvert.DeserializeObject<T>(data);
            return res;
        }

        /// <summary>
        /// 解密，只返回Json文本
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Decode(string token)
        {

            var decoder = GetDecoder();
            var data = decoder.Decode(token, JwtKey);
            return data;
        }

    }

}
