using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CyanDesignDemo.Model
{
    public class BaseModel: INotifyPropertyChanged
    {
        public string CreateBy { get; set; }

        public DateTime? CreateTime { get; set; } = DateTime.Now;

        public string UpdateBy { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool IsDelete { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 获取属性注释
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyDescription(PropertyInfo property)
        {
            var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            return string.Empty;
        }

        //StringBuilder sb = new StringBuilder();

        //// 使用反射获取ProjectForm对象的所有属性及其值
        //foreach (PropertyInfo property in projectFormValue.GetType().GetProperties())
        //{
        //    string propertyName = property.Name;
        //    var propertyType = property.PropertyType.Name;
        //    object propertyValue = property.GetValue(projectFormValue);
        //    var propertyDescription = GetPropertyDescription(property);

        //    sb.AppendLine($"{propertyName}({propertyDescription}) {propertyType}: {propertyValue}");
        //}

        //string result = sb.ToString();

    }

}
