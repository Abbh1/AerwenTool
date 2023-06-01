using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace ARW.Common
{
    public class Tools
    {
        /// <summary>
        /// 要分割的字符串 eg: 1,3,10,00
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split">分割的字符串</param>
        /// <returns></returns>
        public static long[] SpitLongArrary(string str, char split = ',')
        {
            if (string.IsNullOrEmpty(str)) { return Array.Empty<long>(); }
            str = str.TrimStart(split).TrimEnd(split);
            string[] strIds = str.Split(split, (char)StringSplitOptions.RemoveEmptyEntries);
            long[] infoIdss = Array.ConvertAll(strIds, s => long.Parse(s));
            return infoIdss;
        }

        public static int[] SpitIntArrary(string str, char split = ',')
        {
            if (string.IsNullOrEmpty(str)) { return Array.Empty<int>(); }
            string[] strIds = str.Split(split, (char)StringSplitOptions.RemoveEmptyEntries);
            int[] infoIdss = Array.ConvertAll(strIds, s => int.Parse(s));
            return infoIdss;
        }

        public static string[] SpitStringArrary(string str, char split = ',')
        {
            if (string.IsNullOrEmpty(str)) { return Array.Empty<string>(); }
            string[] strIds = str.Split(split, (char)StringSplitOptions.RemoveEmptyEntries);
            return strIds;
        }

        /// <summary>
        /// 根据日期获取星期几
        /// </summary>
        public static string GetWeekByDate(DateTime dt)
        {
            var day = new[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return day[Convert.ToInt32(dt.DayOfWeek.ToString("d"))];
        }

        /// <summary>
        /// 得到这个月的第几周
        /// </summary>
        /// <param name="daytime">年月日</param>
        /// <returns>传递过来的时间是第几周</returns>
        public static int GetWeekNumInMonth(DateTime daytime)
        {
            int dayInMonth = daytime.Day;
            //本月第一天
            DateTime firstDay = daytime.AddDays(1 - daytime.Day);
            //本月第一天是周几
            int weekday = (int)firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;
            //本月第一周有几天
            int firstWeekEndDay = 7 - (weekday - 1);
            //当前日期和第一周之差
            int diffday = dayInMonth - firstWeekEndDay;
            diffday = diffday > 0 ? diffday : 1;
            //当前是第几周,如果整除7就减一天
            int weekNumInMonth = ((diffday % 7) == 0
                ? (diffday / 7 - 1)
                : (diffday / 7)) + 1 + (dayInMonth > firstWeekEndDay ? 1 : 0);
            return weekNumInMonth;
        }
        /// <summary>
        /// 判断一个字符串是否为url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(string str)
        {
            try
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                return Regex.IsMatch(str, Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        public static bool PasswordStrength(string password)
        {
            //空字符串强度值为0
            if (string.IsNullOrEmpty(password)) return false;

            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }

            if (iLtt == 0 && iSym == 0) return false; //纯数字密码
            if (iNum == 0 && iLtt == 0) return false; //纯符号密码
            if (iNum == 0 && iSym == 0) return false; //纯字母密码

            if (password.Length >= 6 && password.Length < 16) return true;//长度不大于6的密码

            if (iLtt == 0) return true; //数字和符号构成的密码
            if (iSym == 0) return true; //数字和字母构成的密码
            if (iNum == 0) return true; //字母和符号构成的密码

            return true; //由数字、字母、符号构成的密码
        }


        public static string GetNumCode(int lenght)
        {
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < lenght; i++)
            {
                stringBuilder.Append(random.Next(1, 10));
            }
            return stringBuilder.ToString();
        }

        // 通过地址获取经纬度
        public static (double? lng, double? lat, string country, string province, string city, string area, string addr) GetXY(string address, System.Net.Http.HttpClient client)
        {
            string gdkey = "ec7484f53ba7b557eac7efc6687e5c0d";//高德key
            string bdkey = "bnPWCosxo6QwUC7DsfkGiunf4zBZ3ogy";//百度key
            //高德API会把地址拆分成省市区和坐标返回，百度只返回坐标
            string url = String.Format("https://restapi.amap.com/v3/geocode/geo?address={0}&output=json&key={1}", address, gdkey);
            //结果
            string result = client.GetStringAsync(url).Result;
            var locationResult = (JObject)JsonConvert.DeserializeObject(result);
            if (locationResult["status"].ToString() == "1" && locationResult["geocodes"].Count() > 0)
            {
                var coordinate = locationResult["geocodes"][0]["location"].ToString().Split(",");
                var country = locationResult["geocodes"][0]["country"].ToString().Replace("[]", "");
                var province = locationResult["geocodes"][0]["province"].ToString().Replace("[]", "");
                var city = locationResult["geocodes"][0]["city"].ToString().Replace("[]", "");
                var district = locationResult["geocodes"][0]["district"].ToString().Replace("[]", "");
                var street = locationResult["geocodes"][0]["street"].ToString().Replace("[]", "");
                var number = locationResult["geocodes"][0]["number"].ToString().Replace("[]", "");
                double? lng = null, lat = null;
                if (coordinate.Length == 2)
                {
                    string lngStr = coordinate[0];
                    string latStr = coordinate[1];
                    lng = double.Parse(lngStr);
                    lat = double.Parse(latStr);

                }
                return (lng, lat, country, province, city, district, street + number);
            }
            else
            {
                //百度API
                url = String.Format("http://api.map.baidu.com/geocoding/v3/?address={0}&output=json&ak={1}", address, bdkey);
                result = client.GetStringAsync(url).Result;
                locationResult = (JObject)JsonConvert.DeserializeObject(result);
                if (locationResult["status"].ToString() == "0")
                {
                    string lngStr = locationResult["result"]["location"]["lng"].ToString();
                    string latStr = locationResult["result"]["location"]["lat"].ToString();
                    double lng = double.Parse(lngStr);
                    double lat = double.Parse(latStr);
                    return (lng, lat, "", "", "", "", "");
                }
                else
                {
                    return (null, null, "", "", "", "", "");
                }
            }
        }

    }
}
