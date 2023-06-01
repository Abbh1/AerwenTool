namespace Infrastructure.WeChat.SubScribe
{
    /// <summary>
    /// 微信订阅推送Dto
    /// </summary>
    public class SubscribeDto
    {
        /// <summary>
        /// 小程序用户openId
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 模板消息Id
        /// </summary>
        public string templateId { get; set; }

    }
}
