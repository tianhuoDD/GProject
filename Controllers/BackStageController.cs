using GProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace GraduationProject.Controllers
{
    public class BackStageController : Controller
    {
        // 链接数据库
        private gpdbEntities db = new gpdbEntities();

        // 检查用户是否为管理员(backstage.js中调用)
        [HttpGet]
        public JsonResult CheckAdminStatus(string username)
        {
            // 从数据库中查询与给定username匹配的第一个用户
            var user = db.usertb.FirstOrDefault(p => p.username == username);
            // 如果user不为null，则检查其admin属性是否为1
            var isAdmin = user?.admin == 1;
            //用户为空,则不是管理员
            if (user == null)
            {
                isAdmin = false;
            }
            // 返回一个JSON响应，其中包含一个名为isAdmin的属性，表示用户是否是管理员。
            // JsonRequestBehavior.AllowGet允许客户端使用HTTP GET请求获取此JSON数据。
            return Json(new { isAdmin = isAdmin }, JsonRequestBehavior.AllowGet);
        }

        /* 用户管理 */
        // GET: /BackStage/UserMan （用户列表）
        public ActionResult UserMan(int? page)
        {
            //展示用户信息
            var userList = db.usertb.ToList();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(userList.ToPagedList(pageNumber,pageSize));
        }
        //注意这里的参数只能使用id,需与路由配置一致（Prompt2）
        // GET:BackStage/DeleteUser/5  （删除用户）
        public ActionResult DeleteUser(int id)
        {
            string referrerUrl = Request.UrlReferrer.ToString();
            //删除用户信息
            usertb user = db.usertb.Find(id);
            //找不到该用户
            if (user == null)
            {
                TempData["notice"] = "未找到该用户，请重试！";
                //使用重定向到UserMan，因为该方法名不在BackStage文件夹中
                return RedirectToAction("UserMan");
            }
            //删除用户，并保存数据库
            db.usertb.Remove(user);
            db.SaveChanges();
            return Redirect(referrerUrl);
        }

        /* 视频管理 */
        // GET: /BackStage/VideoMan （视频列表）
        public ActionResult VideoMan(int? page)//可接收页数
        {
            //展示视频信息
            var videoList = db.videotb.ToList();
            int pageSize = 20;//每页显示行数
            int pageNumber = (page ?? 1);//当前页码
            return View(videoList.ToPagedList(pageNumber, pageSize));
        }
        // GET:BackStage/DeleteVideo/5  （删除视频）
        public ActionResult DeleteVideo(int id)
        {
            // 记录当前页面，以便返回上一个页面(Prompt8)
            string referrerUrl = Request.UrlReferrer.ToString();
            //删除视频信息
            videotb video = db.videotb.Find(id);
            //找不到该视频
            if (video == null)
            {
                TempData["notice"] = "未找到该视频，请重试！";
                return Redirect(referrerUrl);
            }
            //删除视频，并保存数据库
            db.videotb.Remove(video);
            db.SaveChanges();
            return Redirect(referrerUrl);
        }
        /* 评论管理 */
        // GET: /BackStage/CommentMan （评论列表）
        public ActionResult CommentMan(int? page)
        {
            // 展示评论信息
            var commentList = db.comtb.ToList();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(commentList.ToPagedList(pageNumber,pageSize));
        }
        // GET:BackStage/DeleteComment/5  （删除评论）
        public ActionResult DeleteComment(int id)
        {
            string referrerUrl = Request.UrlReferrer.ToString();
            // Find(id)是查找主键id
            comtb comment = db.comtb.Find(id);
            // 找不到该评论
            if (comment == null)
            {
                TempData["notice"] = "未找到该评论，请重试！";
                return RedirectToAction("CommentMan");
            }
            // 删除评论，并保存数据库
            db.comtb.Remove(comment);
            db.SaveChanges();
            return Redirect(referrerUrl);
        }
        // 视频详情
        public ActionResult VideoDetails(int? id, int? page, string SearchTerm)
        {
            // 找到该视频
            videotb video = db.videotb.Find(id);
            //找到该视频
            if (video != null)
            {
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    // 记录页数，以便返回使用
                    ViewBag.CurrentPage = page;
                    ViewBag.SearchTerm = SearchTerm;
                    return View(video);
                }
                return View(video);
            }
            // 搜索界面返回
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                TempData["notice"] = "未找到该视频，请重试！";
                return RedirectToAction("SearchVideo", "Search", new { page = page, SearchTerm = SearchTerm });
            }
            // 视频管理界面返回
            TempData["notice"] = "未找到该视频，请重试！";
            return RedirectToAction("VideoMan", new { page = page });
        }
        /* 广告管理 */
        // GET: /BackStage/AdvertiserMan （广告列表）
        public ActionResult AdvertiserMan(int? page)
        {
            // 展示广告信息
            var advertiserList = db.advertisertb.ToList();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(advertiserList.ToPagedList(pageNumber, pageSize));
        }
        // GET:BackStage/DeleteAdvertiser/5  （删除广告）
        public ActionResult DeleteAdvertiser(int id)
        {
            string referrerUrl = Request.UrlReferrer.ToString();
            //删除用户信息
            advertisertb advertiser = db.advertisertb.Find(id);
            //找不到该用户
            if (advertiser == null)
            {
                TempData["notice"] = "未找到该广告商，请重试！";
                //使用重定向到UserMan，因为该方法名不在BackStage文件夹中
                return RedirectToAction("AdvertiserMan");
            }
            //删除用户，并保存数据库
            db.advertisertb.Remove(advertiser);
            db.SaveChanges();
            return Redirect(referrerUrl);
        }
        /* 账单信息界面 */
        public ActionResult OrderMan(int? page)
        {
            var orderList = db.transaction_recordtb.ToList();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            // 计算总和
            var totalAmount = orderList.Sum(order => order.amount);
            ViewBag.TotalAmount = totalAmount;
            return View(orderList.ToPagedList(pageNumber, pageSize));
        }
        // 删除账单
        public ActionResult DeleteOrder(int id)
        {
            string referrerUrl = Request.UrlReferrer.ToString();
            //删除用户信息
            transaction_recordtb order = db.transaction_recordtb.Find(id);
            //找不到该用户
            if (order == null)
            {
                TempData["notice"] = "未找到该订单，请重试！";
                return RedirectToAction("OrderMan");
            }
            //删除用户，并保存数据库
            db.transaction_recordtb.Remove(order);
            db.SaveChanges();
            return Redirect(referrerUrl);
        }
    }
}
