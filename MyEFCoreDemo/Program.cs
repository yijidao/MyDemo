using System;
using System.Linq;

namespace MyEFCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new BloggingContext();

            Console.WriteLine("新增");
            db.Add(new Blog {Url = "http://www.baidu.com"});
            db.SaveChanges();

            Console.WriteLine("查询");
            var blog = db.Blogs.OrderBy(b => b.BlogId).First();

            Console.WriteLine("更新");
            blog.Url = "http://www.google.com";
            blog.Posts.Add(
                new  Post
                {
                    Title = "Hello World",
                    Content = "EfCore 练习"
                });
            db.SaveChanges();

            Console.WriteLine("删除");
            db.Remove(blog);
            db.SaveChanges();
        }
    }
}
