using Infrastructure;
using Infrastructure.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ARW.Admin.WebApi.Controllers
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 时间格式化 2020-01-01 09:21:03
        /// </summary>
        public static string TIME_FORMAT_FULL = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 时间格式化 01-01 09:21:03
        /// </summary>
        public static string TIME_FORMAT_FULL_2 = "MM-dd HH:mm:ss";

        /// <summary>
        /// 返回成功封装
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeFormatStr"></param>
        /// <returns></returns>
        protected IActionResult SUCCESS(object data, string timeFormatStr = "yyyy-MM-dd HH:mm:ss")
        {
            string jsonStr = GetJsonStr(GetApiResult(data != null ? ResultCode.SUCCESS : ResultCode.FAIL, data), timeFormatStr);
            return Content(jsonStr, "application/json");
        }

        protected IActionResult SUCCESSApi(object data)
        {
            string jsonStr = GetJsonStr(GetApiResult(data != null ? ResultCode.SUCCESS : ResultCode.FAIL, data), "yyyy-MM-dd HH:mm:ss");
            return Content(jsonStr, "application/json");
        }

        protected IActionResult SUCCESSApi(object data, string msg)
        {
            string jsonStr = GetJsonStr(GetApiResult(data != null ? ResultCode.SUCCESS : ResultCode.FAIL, msg, data), "yyyy-MM-dd HH:mm:ss");
            return Content(jsonStr, "application/json");
        }

        protected IActionResult SUCCESSApi(string msg)
        {
            string jsonStr = GetJsonStr(GetApiResult(ResultCode.SUCCESS, msg), "yyyy-MM-dd HH:mm:ss");
            return Content(jsonStr, "application/json");
        }

        /// <summary>
        /// json输出带时间格式的
        /// </summary>
        /// <param name="apiResult"></param>
        /// <param name="timeFormatStr"></param>
        /// <returns></returns>
        protected IActionResult ToResponse(ApiResult apiResult, string timeFormatStr = "yyyy-MM-dd HH:mm:ss")
        {
            string jsonStr = GetJsonStr(apiResult, timeFormatStr);

            return Content(jsonStr, "application/json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="timeFormatStr"></param>
        /// <returns></returns>
        protected IActionResult ToResponse(long rows, string timeFormatStr = "yyyy-MM-dd HH:mm:ss")
        {
            string jsonStr = GetJsonStr(ToJson(rows), timeFormatStr);

            return Content(jsonStr, "application/json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected IActionResult ToResponse(ResultCode resultCode, string msg = "")
        {
            return ToResponse(GetApiResult(resultCode, msg));
        }

        #region 方法

        /// <summary>
        /// 响应返回结果
        /// </summary>
        /// <param name="rows">受影响行数</param>
        /// <returns></returns>
        protected ApiResult ToJson(long rows)
        {
            return rows > 0 ? GetApiResult(ResultCode.SUCCESS) : GetApiResult(ResultCode.FAIL);
        }
        /// <summary>
        /// 响应返回结果,带消息
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ApiResult ToJson(long rows, object data)
        {
            return rows > 0 ? GetApiResult(ResultCode.SUCCESS, data) : GetApiResult(ResultCode.FAIL);
        }
        /// <summary>
        /// 全局Code使用
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ApiResult GetApiResult(ResultCode resultCode, object? data = null)
        {
            var apiResult = new ApiResult((int)resultCode, resultCode.ToString())
            {
                Data = data
            };

            return apiResult;
        }

        protected ApiResult GetApiResult(ResultCode resultCode, string msg, object? data = null)
        {
            var apiResult = new ApiResult((int)resultCode, msg, resultCode.ToString())
            {
                Data = data
            };

            return apiResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected ApiResult GetApiResult(ResultCode resultCode, string msg)
        {
            return new ApiResult((int)resultCode, msg);
        }
        private static string GetJsonStr(ApiResult apiResult, string timeFormatStr)
        {
            if (string.IsNullOrEmpty(timeFormatStr))
            {
                timeFormatStr = TIME_FORMAT_FULL;
            }
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = timeFormatStr
            };

            return JsonConvert.SerializeObject(apiResult, Formatting.Indented, serializerSettings);
        }
        #endregion

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sheetName"></param>
        /// <param name="fileName"></param>
        protected string ExportExcel<T>(List<T> list, string sheetName, string fileName)
        {
            IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)App.ServiceProvider.GetService(typeof(IWebHostEnvironment));
            string sFileName = $"{fileName}{DateTime.Now:MMddHHmmss}.xlsx";
            string newFileName = Path.Combine(webHostEnvironment.WebRootPath, "export", sFileName);
            //调试模式需要加上
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Directory.CreateDirectory(Path.GetDirectoryName(newFileName));
            using (ExcelPackage package = new(new FileInfo(newFileName)))
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                //单元格自动适应大小
                worksheet.Cells.Style.ShrinkToFit = true;
                //单元格自动换行
                //worksheet.Cells.Style.WrapText = true;
                //全部字段导出
                worksheet.Cells.LoadFromCollection(list, true, OfficeOpenXml.Table.TableStyles.Light13);

                #region 样式设置

                //获得有数据的区域
                //var lastAddress = worksheet.Dimension.Address;
                ////获得有数据的区域最上且最左的单元格
                //var startCell = worksheet.Dimension.Start.Address;
                ////获得有数据的区域最下且最右的单元格
                //var endCell = worksheet.Dimension.End.Address;

                //获取最后的行数
                var endCellRow = worksheet.Dimension.End.Row;
                //获取最后的列数
                var endCellColumn = worksheet.Dimension.End.Column;

                //获得第一行有数据的最后一个单元格
                string rowCell = "null";
                var lastRowCell = worksheet.Cells.LastOrDefault(c => c.Start.Row == 1);
                if (lastRowCell != null) rowCell = lastRowCell.Address;

                var firstTitle = "A1" + ":" + rowCell;

                //字体大小
                worksheet.Cells[firstTitle].Style.Font.Size = 13;
                //字体粗细
                worksheet.Cells[firstTitle].Style.Font.Bold = true;
                //字体颜色
                //worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.Red);

                //左右居中
                worksheet.Cells[firstTitle].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //上下居中
                worksheet.Cells[firstTitle].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                //for (int i = 1; i <= endCellRow; i++)
                //{
                worksheet.Row(1).Height = 30;    //行高
                //}
                for (int i = 1; i <= endCellColumn; i++)
                {
                    worksheet.Column(i).Width = 25;//列寬
                }
                #endregion

                package.Save();
            }

            return sFileName;
        }


        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="stream"></param>
        /// <param name="fileName">下载文件名</param>
        /// <returns></returns>
        protected string DownloadImportTemplate<T>(List<T> list, Stream stream, string fileName, List<string> values)
        {
            IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)App.ServiceProvider.GetService(typeof(IWebHostEnvironment));
            string sFileName = $"{fileName}模板.xlsx";
            string newFileName = Path.Combine(webHostEnvironment.WebRootPath, "importTemplate", sFileName);
            //调试模式需要加上
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (!Directory.Exists(newFileName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newFileName));
            }
            using (ExcelPackage package = new(new FileInfo(newFileName)))
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(fileName);

                //单元格自动适应大小
                worksheet.Cells.Style.ShrinkToFit = true;
                //单元格自动换行
                //worksheet.Cells.Style.WrapText = true;

                //全部字段导出
                worksheet.Cells.LoadFromCollection(list, true, OfficeOpenXml.Table.TableStyles.Light13);


                #region 样式设置

                //获得有数据的区域
                //var lastAddress = worksheet.Dimension.Address;
                ////获得有数据的区域最上且最左的单元格
                //var startCell = worksheet.Dimension.Start.Address;
                ////获得有数据的区域最下且最右的单元格
                //var endCell = worksheet.Dimension.End.Address;

                //获取最后的行数
                var endCellRow = worksheet.Dimension.End.Row;
                //获取最后的列数
                var endCellColumn = worksheet.Dimension.End.Column;

                //获得第一行有数据的最后一个单元格
                string rowCell = "null";
                var lastRowCell = worksheet.Cells.LastOrDefault(c => c.Start.Row == 1);
                if (lastRowCell != null) rowCell = lastRowCell.Address;

                var firstTitle = "A1" + ":" + rowCell;

                //字体大小
                worksheet.Cells[firstTitle].Style.Font.Size = 13;
                //字体粗细
                worksheet.Cells[firstTitle].Style.Font.Bold = true;
                //字体颜色
                //worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.Red);

                //左右居中
                worksheet.Cells[firstTitle].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //上下居中
                worksheet.Cells[firstTitle].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                //for (int i = 1; i <= endCellRow; i++)
                //{
                worksheet.Row(1).Height = 30;    //行高
                //}
                for (int i = 1; i <= endCellColumn; i++)
                {
                    worksheet.Column(i).Width = 25;//列寬
                }
                #endregion


                //示例内容
                //var cloum_i = 2;
                //var cloum_i2 = 0;
                //foreach (var item in values)
                //{
                //    cloum_i++;
                //    foreach (var item2 in item)
                //    {
                //        cloum_i2++;
                //        worksheet.Cells[cloum_i, cloum_i2 + 1].Value = item2[cloum_i2];
                //    }

                //}

                for (int i = 0; i < values.Count; i++)
                {
                    worksheet.Cells[2, i + 1].Value = values[i];
                }

                package.SaveAs(stream);
            }

            return sFileName;
        }


        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="stream"></param>
        /// <param name="fileName">下载文件名</param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected string DownloadImportTemplate<T>(List<T> list, Stream stream, string fileName, List<List<string>> values)
        {
            IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)App.ServiceProvider.GetService(typeof(IWebHostEnvironment));
            string sFileName = $"{fileName}模板.xlsx";
            string newFileName = Path.Combine(webHostEnvironment.WebRootPath, "importTemplate", sFileName);
            //调试模式需要加上
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (!Directory.Exists(newFileName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newFileName));
            }
            using (ExcelPackage package = new(new FileInfo(newFileName)))
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(fileName);

                //单元格自动适应大小
                worksheet.Cells.Style.ShrinkToFit = true;
                //单元格自动换行
                //worksheet.Cells.Style.WrapText = true;

                //全部字段导出
                worksheet.Cells.LoadFromCollection(list, true, OfficeOpenXml.Table.TableStyles.Light13);


                #region 样式设置

                //获得有数据的区域
                //var lastAddress = worksheet.Dimension.Address;
                ////获得有数据的区域最上且最左的单元格
                //var startCell = worksheet.Dimension.Start.Address;
                ////获得有数据的区域最下且最右的单元格
                //var endCell = worksheet.Dimension.End.Address;

                //获取最后的行数
                var endCellRow = worksheet.Dimension.End.Row;
                //获取最后的列数
                var endCellColumn = worksheet.Dimension.End.Column;

                //获得第一行有数据的最后一个单元格
                string rowCell = "null";
                var lastRowCell = worksheet.Cells.LastOrDefault(c => c.Start.Row == 1);
                if (lastRowCell != null) rowCell = lastRowCell.Address;

                var firstTitle = "A1" + ":" + rowCell;

                //字体大小
                worksheet.Cells[firstTitle].Style.Font.Size = 13;
                //字体粗细
                worksheet.Cells[firstTitle].Style.Font.Bold = true;
                //字体颜色
                //worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.Red);

                //左右居中
                worksheet.Cells[firstTitle].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //上下居中
                worksheet.Cells[firstTitle].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                //for (int i = 1; i <= endCellRow; i++)
                //{
                worksheet.Row(1).Height = 30;    //行高
                //}
                for (int i = 1; i <= endCellColumn; i++)
                {
                    worksheet.Column(i).Width = 25;//列寬
                }
                #endregion


                //示例内容
                var cloum_i = 1;
                foreach (var item in values)
                {
                    cloum_i++;
                    var cloum_i2 = 0;
                    foreach (var item2 in item)
                    {
                        worksheet.Cells[cloum_i, cloum_i2 + 1].Value = item2;
                        cloum_i2++;
                    }
                }

                //for (int i = 0; i < values.Count; i++)
                //{
                //    worksheet.Cells[2, i+1].Value = values[i];
                //}

                package.SaveAs(stream);
            }

            return sFileName;
        }
    }
}
