using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace CyanDesignDemo.Model
{
    public class TestModel
    {

        public string Name { get; set; }
        public string Content { get; set; }

        public List<News> NewsList;

        public List<InfoArticle> InfoArticleList;

    }


    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public DateTime CreateTime  { get; set; }
    }

    public class InfoArticle
    {
        public int info_article_id { get; set; }
        public string info_article_type_name { get; set; }
        public string info_article_title { get; set; }
        public string info_article_link { get; set; }
        public DateTime info_article_create_time { get; set; }
        public string create_time { get; set; }
        public string info_article_cover { get; set; }
    }

}
