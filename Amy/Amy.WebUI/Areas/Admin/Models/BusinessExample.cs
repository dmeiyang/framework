using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amy.WebUI.Areas.Admin.Models
{
    public class BusinessExample
    {
        public string Id { get; set; }

        public string Pictrue { get; set; }

        public string Tilte { get; set; }

        public string Description { get; set; }

        public static List<BusinessExample> GetList()
        {
            var list = new List<BusinessExample>();

            list.Add(new BusinessExample()
            {
                Id=IDHelper.Id32,
                Pictrue= "/content/mgr/images/gallery/business/秒杀抢购_list.png",
                Tilte = "商品秒杀业务设计",
                Description= "来自百度的服务器架构师吕毅，为大家介绍抢购业务的设计与实现。主要包含如下内容： 1. 抢购业务介绍； 2. 具体抢购项目中的设计： 2.1. 如何解耦前后端压力、 2.2. 如何保证商品库存可靠、 2.3. 如何在业务中多放对账； 3. 项目总结； 4. Q & A"
            });

            return list;
        }
    }
}