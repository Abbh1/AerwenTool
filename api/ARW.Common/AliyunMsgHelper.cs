using Aliyun.OSS.Common;
using Aliyun.OSS;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using ClientException = Aliyun.Acs.Core.Exceptions.ClientException;
using Infrastructure;
using System.Collections.Generic;

namespace ARW.Common
{
    public class AliyunMsgHelper
    {
        
        public static void SendMsgCode(string phone)
        {
            var accessKeyId = AppSettings.GetConfig("AARWYUN_MSG:accessKeyId");
            var accessSecret = AppSettings.GetConfig("AARWYUN_MSG:accessSecret");
            var signName = AppSettings.GetConfig("AARWYUN_MSG:signName");
            var templateCode = AppSettings.GetConfig("AARWYUN_MSG:templateCode");

            /*这里的*"cn-hangzhou"：不需要更改, "<accessKeyId>"：就是上面要求记录到本地的accessKeyId, "<accessSecret>:：就是上面要求记录到本地的accessSecret"*/
            IClientProfile profile = DefaultProfile.GetProfile("cn-hongkong", accessKeyId, accessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;1

            request.AddQueryParameters("PhoneNumbers", phone);
            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("TemplateCode", templateCode);

            Dictionary<string, object> pairs = new Dictionary<string, object>();
            pairs.Add("code", new Random().Next(100000, 1000000).ToString());
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(pairs);
            request.AddQueryParameters("TemplateParam", json);

            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                var status = response.HttpResponse;
            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
            }

        }
    }

}

