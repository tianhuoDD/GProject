using GProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GraduationProject.Controllers
{
    public class LoginRegController : Controller
    {
        //数据库上下文连接对象
        private gpdbEntities db = new gpdbEntities();
        /* 返回登录界面 */
        // GET: Login
        public ActionResult Login()
        {
            return View("Login");
        }
        //实现登录功能
        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            //去数据库
            //是否存在该用户
            usertb user = db.usertb.FirstOrDefault(p => p.username == username);
            //用户不存在
            if (user == null)
            {
                ViewBag.notice="账号不存在！";
            }
            //用户密码错误
            else if (user.password != password)
            {
                ViewBag.notice="密码错误！";
                //密码错误时保留username
                ViewBag.username = username;
            }
            //登录成功
            else
            {
                return RedirectToAction("Index","FrontStage");
            }
            return View("Login");
        }

        /* 返回注册界面 */
        // GET: Register
        public ActionResult Register()
        {
            return View("Register");
        }
        //注册功能实现
        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            // 检查用户是否已存在
            var existingUser = db.usertb.FirstOrDefault(p => p.username == username);
            if (existingUser != null)
            {
                ViewBag.notice = "该账号已被注册，请登录！";
                return View("Login"); // 返回登录视图
            }
            // 获取表中最大的 ID，如果不存在则设置为 0（主键ID不能为空）
            int maxId = db.usertb.Any() ? db.usertb.Max(u => u.id) : 0;
            // 创建新用户并添加到数据库
            var newUser = new usertb
            {
                id = maxId+1,
                username = username,
                password = password,
                nickname = username,
                regtime = DateTime.Now, // 将注册时间设置为当前时间
                admin = 0
            };
             //添加并保存
            db.usertb.Add(newUser);
            db.SaveChanges();
            //注册成功提示
            ViewBag.notice = "注册成功，请登录!";
            return View("Login");
        }
    }
}