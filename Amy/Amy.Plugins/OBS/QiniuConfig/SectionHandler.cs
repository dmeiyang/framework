/* ==================================================================
* 类 名 称：SectionHandler
* 版 本 号：v1.0.0
* 作 者: Administrator
* 创建时间：2016/5/24 14:26:38
* 类 说 明：
* 备 注：
*
* ==================================================================*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace Amy.Plugins.OBS.QiniuConfig
{
    public class SectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Config config = new Config();

            //解析配置文件信息，返回对象
            if (section != null)
            {
                foreach (XmlNode item in section.ChildNodes)
                {
                    switch (item.Attributes["key"].InnerText)
                    {
                        case "AccessKey":
                            config.AccessKey = item.Attributes["value"].InnerText;
                            break;
                        case "SecretKey":
                            config.SecretKey = item.Attributes["value"].InnerText;
                            break;
                        case "Bucket":
                            config.Bucket = item.Attributes["value"].InnerText;
                            break;
                        case "Domain":
                            config.Domain = item.Attributes["value"].InnerText;
                            break;
                        default:
                            break;
                    }

                }
            }
            return config;
        }
    }
}
