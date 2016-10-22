using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amy.WebUI.Controllers;

namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class OBSController : BasicController
    {
        /// <summary>
        /// 七牛OBS服务
        /// </summary>
        public ActionResult Qiniu()
        {
            return View();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public string PutByQiniu(string id)
        {
            var url = "七牛图片访问路径";

            var files = Request.Files;

            if (files.Count > 0)
            {
                Amy.Plugins.OBS.IObjectStorageServcieFactory factory = new Amy.Plugins.OBS.QiniuFactory();

                url = factory.Put(files[0].InputStream, id: id);
            }

            return url;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public ActionResult DelByQiniu(string id)
        {
            Amy.Plugins.OBS.IObjectStorageServcieFactory factory = new Amy.Plugins.OBS.QiniuFactory();

            return JRCommonHandleResult(factory.Remove(id));
        }
    }
}