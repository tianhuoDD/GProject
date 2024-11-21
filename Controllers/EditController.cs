
using GProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraduationProject.Controllers
{
    public class EditController : Controller
    {
        private gpdbEntities db = new gpdbEntities();
        //默认用户权限为null
        private int admin = 0;
        private int status = 0;
        private int adstatus = 0;
        private int adpay = 0;
        public static usertb temp;
        public static videotb temp2;
        public static comtb temp3;
        public static advertisertb temp4;

        /* 修改用户 */
        // GET: EditUser 
        public ActionResult EditUser()
        {
            // 获取url中的 id（/Edit/EditUser?id=@user.id），由UserMan传值
            string userId = Request.QueryString["id"];
            //检验是否为空
            if (!string.IsNullOrEmpty(userId))
            {
                // 将 userId 转换为整数
                if (int.TryParse(userId, out int id))
                {
                    // 使用 id 从数据库中获取用户信息，这里假设你有一个名为 db 的数据库上下文
                    var user = db.usertb.FirstOrDefault(u => u.id == id);
                    //记录user的值，便于后续返回此页面
                    temp = user;
                    if (user != null)
                    {
                        // 将用户信息传递给视图
                        return View(user);
                    }
                }
            }
            //如果未能成功获取用户信息，执行相应的错误处理(用于跨页面提示)
            TempData["notice"] = "未找到该用户，请重试！";
            //返回其他控制器的方法return RedirectToAction("Index", "OtherController");
            return RedirectToAction("UserMan", "BackStage");
        }

        /* 处理上传的文件为二进制格式 */
        private static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        //使用Post请求修改  （提交修改后的表单）
        [HttpPost]
        //编辑功能实现(id为主键，不可修改)
        public ActionResult EditUser(string inUser, string inPwd, string inNickname,DateTime regTime, HttpPostedFileBase inIcon ,string ifAdmin)
        {
            //用于后续错误情况，还保留用户信息
            usertb user2 = temp;
            // 从数据库中获取要修改的用户
            var user = db.usertb.FirstOrDefault(u => u.id == user2.id);
            // 获取数据库中具有相同 id 或 username 的用户，但排除当前用户
            var conflictingUser = db.usertb.FirstOrDefault(u => (u.id == user2.id || u.username == inUser) && u.id!= user2.id);
            // 检查是否找到冲突的用户
            if (conflictingUser != null)
            {
                // 处理冲突的情况
                ViewBag.notice = "新的用户名与现有用户冲突，请选择其他值。";
                return View(user2); 
            }
            //string类型，转换为int类型
            if (ifAdmin == "1")
            {
                admin = 1;
            }
            else
            {
                admin = 0;
            }
            // 更新用户信息
            user.username = inUser;
            user.password = inPwd;
            user.nickname = inNickname;
            user.regtime = regTime;
            // 检查是否上传了新文件
            if (inIcon != null && inIcon.ContentLength > 0)
            {
                // 读取并更新新文件
                user.icon = ReadFully(inIcon.InputStream);
            }
            user.admin = (sbyte?)admin;
            // 保存更改
            db.SaveChanges();
            TempData["notice"] = "修改成功！";
            // 重定向到用户管理页面或其他适当的页面
            return RedirectToAction("UserMan", "BackStage");
        }
        
        /* 修改视频 */
        // GET: EditVideo
        public ActionResult EditVideo()
        {
            string videoID = Request.QueryString["videoID"];
            if (!string.IsNullOrEmpty(videoID))
            {
                // 将 videoID 转换为整数,赋值给id
                if (int.TryParse(videoID, out int id))
                {
                    // 使用 id 从数据库中获取视频信息，这里假设你有一个名为 db 的数据库上下文
                    var video = db.videotb.FirstOrDefault(u => u.videoID == id);
                    //记录 video 的值，便于后续返回此页面
                    temp2 = video;
                    if (video != null)
                    {
                        // 将视频信息传递给视图
                        return View(video);
                    }
                }
            }
            //如果未能成功获取视频信息，执行相应的错误处理(用于跨页面提示)
            TempData["notice"] = "未找到该视频，请重试！";
            //返回其他控制器的方法return RedirectToAction("Index", "OtherController");
            return RedirectToAction("VideoMan", "BackStage");
        }
        // 提交修改后的表单
        [HttpPost]
        // 参数应与表单中name属性相同
        public ActionResult EditVideo(string inTitle, string inLink, string inCover, string inTag,
            string inYear,float inRating,string inStatus, int inHotScore,string inType,
            string intvID, string inDesc, string inBigCover, string inDate, int inTime,
            string inDirector, string inActor)
        {
            //用于后续错误情况，还保留视频信息
            videotb video2 = temp2;
            // 从数据库中获取要修改的视频
            var video = db.videotb.FirstOrDefault(u => u.videoID == video2.videoID);
            // 获取数据库中具有相同 videoID 或 title 或 tvid 的视频，但排除当前视频
            var conflictingVideo = db.videotb.FirstOrDefault(u =>
                                (u.videoID == video2.videoID || u.title == inTitle || u.tvid == intvID) &&
                                u.videoID != video2.videoID);
            // 检查是否找到冲突的视频
            if (conflictingVideo != null)
            {
                // 处理冲突的情况
                ViewBag.notice = "新的‘视频名称’或‘唯一标识’与现有冲突，请选择其他值。";
                return View(video2);
            }
            // 更新用户信息
            video.title = inTitle;
            video.link = inLink;
            video.cover = inCover;
            video.tag = inTag;
            video.year = inYear;
            video.rating = inRating;
            video.status = inStatus;
            video.hotscore = inHotScore;
            video.type = inType;
            video.tvid = intvID;
            video.desc = inDesc;
            video.bigcover = inBigCover;
            video.date = inDate;
            video.time = inTime;
            video.director = inDirector;
            video.actor = inActor;

            // 保存更改
            db.SaveChanges();
            TempData["notice"] = "修改成功！";
            // 重定向到用户管理页面或其他适当的页面
            return RedirectToAction("VideoMan", "BackStage");
        }
        
        /* 修改评论 */
        // GET: EditComment 用于点击编辑按钮返回
        public ActionResult EditComment()
        {
            string comID = Request.QueryString["comID"];
            if (!string.IsNullOrEmpty(comID))
            {
                if (int.TryParse(comID, out int id))
                {
                    var comment = db.comtb.FirstOrDefault(u => u.comID == id);
                    temp3 = comment;
                    if (comment != null)
                    {
                        return View(comment);
                    }
                }
            }
            //如果未能成功获取评论信息，执行相应的错误处理(用于跨页面提示)
            TempData["notice"] = "未找到该评论，请重试！";
            return RedirectToAction("CommentMan", "BackStage");
        }
        //使用Post请求修改 用于点击修改按钮的返回（表单提交）
        [HttpPost]
        public ActionResult EditComment(string username, string content, DateTime comtime, string tvid,string ifStatus)
        {
            //用于后续错误情况，还保留视频信息
            comtb commnet2 = temp3;
            //string类型，转换为int类型
            if (ifStatus == "1")
            {
                status = 1;
            }
            else
            {
                status = 0;
            }
            // 从数据库中获取要修改的视频
            var commnet = db.comtb.FirstOrDefault(u => u.comID == commnet2.comID);
            // 更新评论信息
            commnet.username = username;
            commnet.content = content;
            commnet.comtime = comtime;
            commnet.tvid = tvid;
            commnet.status = (sbyte?)status;
            // 保存更改
            db.SaveChanges();
            TempData["notice"] = "修改成功！";
            // 重定向到用户管理页面或其他适当的页面
            return RedirectToAction("CommentMan", "BackStage");
        }
        /* 修改广告商 */
        // GET: EditAdvertiser
        public ActionResult EditAdvertiser()
        {
            // 获取url中的 id，由AdvertiserMan传值
            string advertiserId = Request.QueryString["id"];
            //检验是否为空
            if (!string.IsNullOrEmpty(advertiserId))
            {
                // 将 userId 转换为整数
                if (int.TryParse(advertiserId, out int id))
                {
                    // 使用 id 从数据库中获取用户信息，这里假设你有一个名为 db 的数据库上下文
                    var advertiser = db.advertisertb.FirstOrDefault(u => u.advertiserID == id);
                    //记录user的值，便于后续返回此页面
                    temp4 = advertiser;
                    if (advertiser != null)
                    {
                        // 将用户信息传递给视图
                        return View(advertiser);
                    }
                }
            }
            //如果未能成功获取用户信息，执行相应的错误处理(用于跨页面提示)
            TempData["notice"] = "未找到该广告商，请重试！";
            //返回其他控制器的方法return RedirectToAction("Index", "OtherController");
            return RedirectToAction("AdvertiserMan", "BackStage");
        }
        //使用Post请求修改  （提交修改后的表单）
        [HttpPost]
        [ValidateInput(false)]
        //编辑功能实现(id为主键，不可修改)
        public ActionResult EditAdvertiser(string username,string inAdName, decimal balance, string inAdContent, DateTime startDate, DateTime endDate, string adStatus, float cost, string payStatus)
        {
            //用于后续错误情况，还保留用户信息
            advertisertb advertiser2 = temp4;
            // 从数据库中获取要修改的用户
            var advertiser = db.advertisertb.FirstOrDefault(u => u.advertiserID == advertiser2.advertiserID);
            // 获取数据库中具有相同 id 或 username 的用户，但排除当前用户
            var conflictingAdName = db.advertisertb.FirstOrDefault(u => (u.advertiserID == advertiser2.advertiserID || u.ad_name == inAdName) && u.advertiserID != advertiser2.advertiserID);
            var conflictingUser = db.advertisertb.FirstOrDefault(u => (u.advertiserID == advertiser2.advertiserID || u.username == username) && u.advertiserID != advertiser2.advertiserID);
            // 检查是否找到冲突的用户
            if (conflictingAdName != null )
            {
                // 处理冲突的情况
                ViewBag.notice = "新的广告商名与现有广告商名冲突，请选择其他值。";
                return View(advertiser2);
            }else if(conflictingUser != null)
            {
                // 处理冲突的情况
                ViewBag.notice = "新的用户名与现有用户名冲突，请选择其他值。";
                return View(advertiser2);
            }
            //string类型，转换为int类型
            switch (adStatus){
                case "0":
                    adstatus = 0; // 未审核
                    break;
                case "1":
                    adstatus = 1; // 通过
                    break;
                case "2":
                    adstatus = 2; // 播放中
                    break;
                case "3": 
                    adstatus = 3; // 已结束
                    break;
                default:
                    adstatus = 0;
                    break;
            }
            switch (payStatus)
            {
                case "0":
                    adpay = 0; // 未支付
                    break;
                case "1":
                    adpay = 1; // 已支付
                    break;
                default:
                    adpay = 0;
                    break;
            }

            // 更新用户信息
            advertiser.username = username;
            advertiser.ad_name = inAdName;
            advertiser.balance = balance;
            advertiser.ad_content = inAdContent;
            advertiser.start_date = startDate;
            advertiser.end_date = endDate;
            advertiser.status = (sbyte?)adstatus;
            advertiser.cost = (decimal?)cost;
            advertiser.payment_status = (sbyte?)adpay;
            // 保存更改
            db.SaveChanges();
            TempData["notice"] = "修改成功！";
            // 重定向到用户管理页面或其他适当的页面
            return RedirectToAction("AdvertiserMan", "BackStage");
        }
    }
}