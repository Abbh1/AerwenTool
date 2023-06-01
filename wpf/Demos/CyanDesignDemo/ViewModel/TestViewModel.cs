using CyanDesignDemo.Model;
using DMSkin.Core;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;

namespace DMSkinDemo.ViewModel
{
    public class TestViewModel : ViewModelBase
    {

        // 实例化模型层
        private TestModel testModel = new TestModel();


        /// <summary>
        /// 测试请求内容
        /// </summary>
        public string Content
        {
            get
            {
                //testModel.Content = GetNewsList();
                return testModel.Content;
            }
            set
            {
                testModel.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        /// <summary>
        /// 测试请求列表
        /// </summary>
        public List<InfoArticle> InfoArticleList
        {
            get
            {
                testModel.InfoArticleList = GetNewsList();
                return testModel.InfoArticleList;
            }
            set
            {
                testModel.InfoArticleList = value;
                OnPropertyChanged(nameof(InfoArticleList));
            }
        }

        /// <summary>
        /// 保存的文字
        /// </summary>
        public string Name
        {
            get
            {
                return testModel.Name;
            }
            set
            {
                testModel.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// 新闻列表
        /// </summary>
        public List<News> NewsList
        {
            get
            {
                var list = new List<News>();
                News news1 = new News
                {
                    Id = 1,
                    Title = "超级大新闻",
                    Intro = "史前巨兽！！！",
                    CreateTime = DateTime.Now
                };
                News news2 = new News
                {
                    Id = 2,
                    Title = "超级大新闻2",
                    Intro = "史前巨兽2！！！",
                    CreateTime = DateTime.Now.AddDays(1)
                };
                list.Add(news1);
                list.Add(news2);
                list.Add(news2);
                list.Add(news2);
                list.Add(news2);

                testModel.NewsList = list;

                return testModel.NewsList;
            }
            set
            {
                testModel.NewsList = value;
                OnPropertyChanged(nameof(NewsList));
            }
        }


        /// <summary>
        /// 保存
        /// </summary>    
        public ICommand SaveCommand => new DelegateCommand(obj =>
        {
            Name = "你好";
        });


        /// <summary>
        /// 清除
        /// </summary>    
        public ICommand ClearCommand => new DelegateCommand(obj =>
        {
            Name = "";
        });


        /// <summary>
        /// 读取
        /// </summary>    
        public ICommand ReadCommand => new DelegateCommand(obj =>
        {
            Name = Storage.GetData<string>("save");
        });



        public List<InfoArticle> GetNewsList()
        {
            HttpItem httpItem = new HttpItem
            {
                URL = "https://www.houde.aerwen.net/api/news/getinfoArticleList?page=1&idx=0&limit=6"
            };
            var res = HTTP.New.GetHtml(httpItem).Html;
            

            var resData = HTTP.New.ToListJson<InfoArticle>(res);

            foreach (var item in resData.data)
            {
                item.info_article_cover = "https://www.houde.aerwen.net" + item.info_article_cover;
                item.create_time = item.info_article_create_time.ToString("yyyy-MM-dd");
            }

            //return res;
            return resData.data;
        }


    }
}
