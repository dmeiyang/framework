using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace System
{
    public class HGLMember
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 登录帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// 登陆状态
        /// </summary>
        public bool Enduring { get; set; }
    }

    public class HGLAuthorize
    {
        public const string SPLIT = "@amy#";

        /// <summary>
        /// 会员Id
        /// </summary>
        public static string Id
        {
            get
            {
                return Serialize().Id;
            }
        }

        /// <summary>
        /// 登录帐号
        /// </summary>
        public static string Account
        {
            get
            {
                return Serialize().Account;
            }
        }

        /// <summary>
        /// 账号角色
        /// </summary>
        public static int Role
        {
            get
            {
                return Serialize().Role;
            }
        }

        /// <summary>
        /// 登陆状态
        /// </summary>
        public static bool Enduring
        {
            get
            {
                return Serialize().Enduring;
            }
        }

        /// <summary>
        /// 存放cookie
        /// </summary>
        /// <param name="model"></param>
        public static void SetCookie(HGLMember model)
        {
            //System.Web.HttpContext.Current.Request.Cookies.Clear();

            var expires = DateTime.Now.AddMinutes(30);

            if (model.Enduring)
                expires = DateTime.Now.AddDays(7);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    model.Id,
                    DateTime.Now,
                    expires,
                    false,
                    string.Format("{1}{0}{2}{0}{3}{0}{4}", SPLIT, model.Id, model.Account, model.Role, model.Enduring)
                    );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.Domain = FormsAuthentication.CookieDomain;
            authCookie.Expires = expires;
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// 清除cookie
        /// </summary>
        public static void Logoff()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        private static HGLMember Serialize()
        {
            FormsIdentity identity = System.Web.HttpContext.Current.User.Identity as FormsIdentity;
            if (identity == null)
                return new HGLMember();
            if (identity.Ticket == null)
                return new HGLMember();
            if (string.IsNullOrEmpty(identity.Ticket.UserData))
                return new HGLMember();

            try
            {
                var array = identity.Ticket.UserData.Split(new string[] { SPLIT }, StringSplitOptions.None);
                if (array.Length != 4)
                {
                    return new HGLMember();
                }

                return new HGLMember
                {
                    Id = array[0],
                    Account = array[1],
                    Role = array[2].ToInt32(),
                    Enduring = array[3].ToBoolean(),
                };
            }
            catch (Exception)
            {
                return new HGLMember();
            }
        }
    }
}
