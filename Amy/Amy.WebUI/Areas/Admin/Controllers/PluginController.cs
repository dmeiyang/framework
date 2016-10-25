using Amy.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class PluginController : BasicController
    {
        public ActionResult WebUploader()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadSliverFile()
        {
            try
            {
                string fileName = Request["name"];

                int index = Convert.ToInt32(Request["chunk"]);//当前分块序号

                var guid = Request["guid"];//前端传来的GUID号

                var dir = Server.MapPath("~/upload/file");//文件上传目录

                dir = System.IO.Path.Combine(dir, guid);//临时保存分块的目录

                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                string filePath = System.IO.Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突

                var data = Request.Files["file"];//表单中取得分块文件

                if (data != null)//为null可能是暂停的那一瞬间
                {
                    data.SaveAs(filePath);//报错
                }
                return Json(new { error = 0 });//Demo，随便返回了个值，请勿参考
            }
            catch
            {
                return Json(new { error = 1 });//Demo，随便返回了个值，请勿参考
            }
        }

        [HttpPost]
        public ActionResult MergeSliverFile()
        {
            try
            {
                var guid = Request["guid"];//GUID

                var uploadDir = Server.MapPath("~/upload/file/");//Upload 文件夹

                var dir = System.IO.Path.Combine(uploadDir, guid);//临时文件夹

                var fileName = Request["fileName"];//文件名

                var nameArray = fileName.ToSplit('.');

                var tempName = IDHelper.Id32 + "." + nameArray[1];

                var files = System.IO.Directory.GetFiles(dir);//获得下面的所有文件

                var finalPath = System.IO.Path.Combine(uploadDir, tempName);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）

                var fs = new System.IO.FileStream(finalPath, System.IO.FileMode.Create);

                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                {
                    var bytes = System.IO.File.ReadAllBytes(part);
                    fs.Write(bytes, 0, bytes.Length);
                    bytes = null;
                    System.IO.File.Delete(part);//删除分块
                }

                fs.Close();

                System.IO.Directory.Delete(dir);//删除文件夹

                return Json(new { error = 0, path = "/upload/file/" + tempName });//随便返回个值，实际中根据需要返回
            }
            catch
            {
                return Json(new { error = 1, path = "系统合并文件时出现异常，请联系管理员或者重新上传！" });//随便返回个值，实际中根据需要返回
            }
        }
    }
}