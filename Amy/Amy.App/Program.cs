using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace Amy.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var prcm = new PooledRedisClientManager(new string[] { "192.168.2.31:6379" }, new string[] { "192.168.2.31:6379" }, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5, // “写”链接池链接数  
                MaxReadPoolSize = 5, // “读”链接池链接数  
                AutoStart = true,
            });

            var mclient = prcm.GetClient();

            var sclient = prcm.GetReadOnlyClient();

            #region string缓存
            //var result = mclient.Set<string>("test", "我是一个好孩子！！！", DateTime.Now.AddDays(1));

            //if(result)
            //    Console.WriteLine("设置test缓存成功！！！");

            //var result = sclient.Remove("test");

            //if (result)
            //    Console.WriteLine("删除test缓存成功！！！");

            //Console.WriteLine("当前test缓存值：{0}", sclient.Get<string>("test"));

            //var dic = new Dictionary<string, string>();

            //dic.Add("test", "我是一个好孩子！！！");

            //dic.Add("test2", "我是一个好孩子！！！");

            //mclient.SetAll(dic);

            //mclient.RemoveAll(new string[] { "test", "test2" });

            //var list = sclient.GetAllKeys();

            //foreach (var v in list)
            //{
            //    Console.WriteLine("key：{0}  value：{1}", v, sclient.Get<string>(v));
            //}
            #endregion

            #region list缓存
            var mlclient = mclient.GetTypedClient<Student>();

            var slclient = sclient.GetTypedClient<Student>();

            mclient.FlushAll();

            mlclient.AddItemToList(mlclient.Lists["test"], new Student() { Id = "001", Name = "成龙", Age = 18 });

            var list = slclient.Lists["test"];

            mlclient.RemoveAllFromList(list);

            if (list.Where(x => x.Id == "002").Count() <= 0)
            {
                slclient.PushItemToList(mlclient.Lists["test"], new Student() { Id = "002", Name = "刘德华", Age = 20 });
            }

            mlclient.PopItemFromList(list);

            foreach (var v in list)
            {
                Console.WriteLine("key：{0}  value：{1}", "test", v.ToJsonByJsonNet());
            }
            #endregion

            //数据持久化
            //mclient.SaveAsync();

            //清空缓存
            //mclient.FlushAll();
            //mclient.FlushDb();
        }
    }

    public class Student
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
