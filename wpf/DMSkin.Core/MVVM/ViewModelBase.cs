using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System;
using System.Windows;
using DMSkin.Core.MVVM;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Net.Http.Headers;

namespace DMSkin.Core
{
    public class ViewModelBase : INotifyPropertyChanged
    {



        #region 网络请求
        private readonly string baseUrl = "http://localhost:8888/api";

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">列表定义的类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns></returns>        
        public List<T> GetListData<T>(string url)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                var jwt = Storage.GetData<string>("jwt");
                if (jwt != null)
                {
                    headers.Add("Authorization", "Bearer " + jwt);
                }

                url = baseUrl + url;
                HttpItem httpItem = new HttpItem { URL = url, Header = headers };
                var res = HTTP.New.GetHtml(httpItem).Html;
                var resData = HTTP.New.ToListJson<T>(res).data;
                return resData;
            }
            catch (Exception)
            {
                MessageBox.Show("接口请求异常");
                throw;
            }
        }
        /// <summary>
        /// 获取data数据
        /// </summary>
        /// <typeparam name="T">列表定义的类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns></returns>        
        public ApiResponseData<T> Get<T>(string url)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                var jwt = Storage.GetData<string>("jwt");
                if (jwt != null)
                {
                    headers.Add("Authorization", "Bearer " + jwt);
                }

                url = baseUrl + url;
                HttpItem httpItem = new HttpItem { URL = url };
                var res = HTTP.New.GetHtml(httpItem).Html;
                var resData = HTTP.New.ToJson<T>(res);
                return resData;
            }
            catch (Exception ex)
            { 
                MessageBox.Show("接口请求异常");
                throw;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="fielUrl">文件地址</param>
        /// <returns></returns>
        public async Task<T> UploadFile<T>(string imagePath, string fileDir)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        var jwt = Storage.GetData<string>("jwt");
                        if (jwt != null)
                        {
                            // 设置 Authorization 请求头
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                        }

                        // 添加 fileDir 参数
                        form.Add(new StringContent(fileDir), "fileDir");
                        // 添加 file 参数
                        byte[] fileData = File.ReadAllBytes(imagePath);
                        var fileContent = new ByteArrayContent(fileData);
                        fileContent.Headers.Add("Content-Type", "application/octet-stream");
                        form.Add(fileContent, "file", Path.GetFileName(imagePath));

                        using (var response = await httpClient.PostAsync(baseUrl + "/Common/UploadFile", form))
                        {
                            response.EnsureSuccessStatusCode();
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var resData = HTTP.New.ToJson<T>(responseContent).data;
                            return resData;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("接口请求异常");
                throw;
            }
        }


        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<ApiResponseData<T>> SendPostRequest<T>(string url, Dictionary<string, object> parameters)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jwt = Storage.GetData<string>("jwt");
                    if (jwt != null)
                    {
                        // 设置 Authorization 请求头
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    }

                    var formContent = new FormUrlEncodedContent(parameters.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString())));

                    using (var response = await httpClient.PostAsync(baseUrl + url, formContent))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = HTTP.New.ToJson<T>(responseContent);
                        return responseData;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("接口请求异常");
                throw;
            }
        }

        public async Task<ApiResponseData<T>> SendPostRequest<T>(string url, object model)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jwt = Storage.GetData<string>("jwt");
                    if (jwt != null)
                    {
                        // 设置 Authorization 请求头
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    }

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(baseUrl + url, jsonContent))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = HTTP.New.ToJson<T>(responseContent);
                        return responseData;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("接口请求异常");
                throw;
            }
        }

        public async Task<ApiResponseDataNoType> SendPostRequest(string url, object model)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jwt = Storage.GetData<string>("jwt");
                    if (jwt != null)
                    {
                        // 设置 Authorization 请求头
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    }

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");


                    using (var response = await httpClient.PostAsync(baseUrl + url, jsonContent))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = HTTP.New.ToNoTypeJson(responseContent);
                        return responseData;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("接口请求异常");
                throw;
            }

        }

        /// <summary>
        /// 删除请求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteData(string url, string ids)
        {
            string msg = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = baseUrl + url + "/{ids}";

                // 替换 {id} 为具体要删除的资源的 ID
                string resourceUrl = apiUrl.Replace("{ids}", ids);
                httpClient.Timeout = TimeSpan.FromSeconds(30); // 设置超时时间为30秒

                var jwt = Storage.GetData<string>("jwt");
                if (jwt != null)
                {
                    // 设置 Authorization 请求头
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                }

                try
                {
                    HttpResponseMessage response = await httpClient.DeleteAsync(resourceUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // 删除成功，执行相应的操作
                        msg = "删除成功";
                    }
                    else
                    {
                        // 删除失败，处理错误信息
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        msg = $"删除失败: {errorMessage}";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    throw;
                }
            }
            return msg;
        }


        #endregion

        #region 是否选中
        private bool isChecked;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        #endregion

        #region UI更新接口
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region 是否正在加载
        private bool isLoad;

        /// <summary>
        /// 是否加载
        /// </summary>
        public bool IsLoad
        {
            get { return isLoad; }
            set
            {
                isLoad = value;
                OnPropertyChanged(nameof(IsLoad));
            }
        }
        #endregion

        #region 是否需要刷新
        private bool update;
        /// <summary>
        /// 刷新
        /// </summary>
        public bool Update
        {
            get { return update; }
            set
            {
                update = value;
                OnPropertyChanged(nameof(Update));
            }
        }
        #endregion


        #region 工具



        #endregion
    }
}
