using GProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GProject.Controllers
{
    public class AddController : Controller
    {
        private gpdbEntities db = new gpdbEntities();
        //默认用户权限为false
        private int admin = 0;
        private int status = 0;
        private int adstatus = 0;
        private int adpay = 0;
        /* 添加用户 */
        // GET: AddUser
        public ActionResult AddUser()
        {
            return View("AddUser");
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
        //添加功能实现(提交表单)
        [HttpPost]
        public ActionResult AddUser(int id,string inUser, string inPwd, string inNickname,DateTime regTime, HttpPostedFileBase inIcon,string ifAdmin)
        {
            // 检查id是否已存在
            var existingId = db.usertb.FirstOrDefault(p => p.id == id);
            if (existingId != null)
            {
                ViewBag.notice = "该id已存在，请重新添加!";
                return View("AddUser"); // 返回添加视图
            }
            // 检查username是否已存在
            var existingUser = db.usertb.FirstOrDefault(p => p.username == inUser);
            if (existingUser != null)
            {
                ViewBag.notice = "该账号已存在，请重新添加!";
                return View("AddUser"); // 返回添加视图
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
            // 创建新用户并添加到数据库
            var newUser = new usertb
            {
                id = id,
                username = inUser,
                password = inPwd,
                nickname = inNickname,
                regtime = regTime,
                icon = ReadFully(inIcon.InputStream),
                admin = (sbyte?)admin
            };
            //添加并保存
            db.usertb.Add(newUser);
            db.SaveChanges();
            //提示信息
            ViewBag.notice = "添加成功!";
            return View("AddUser");
        }

        /* 添加视频 */
        // GET: AddVideo
        public ActionResult AddVideo()
        {
            return View("AddVideo");
        }
        // 添加视频功能实现(提交表单)
        [HttpPost]
        public ActionResult AddVideo(int videoID, string inTitle, string inLink, string inCover, string inTag, 
            string inYear, float inRating, string inStatus, int inHotScore,string inType,
            string intvID, string inDesc, string inBigCover, string inDate, int inTime,
            string inDirector, string inActor)
        {
            // 检查videoID是否已存在
            var existingVideoID = db.videotb.FirstOrDefault(p => p.videoID == videoID);
            if (existingVideoID != null)
            {
                ViewBag.notice = "该视频ID已存在，请重新添加!";
                return View("AddVideo"); // 返回添加视图
            }
            // 检查 title 是否已存在
            var existingTitle = db.videotb.FirstOrDefault(p => p.title == inTitle);
            if (existingTitle != null)
            {
                ViewBag.notice = "该视频名称已存在，请重新添加!";
                return View("AddVideo"); // 返回添加视图
            }
            // 检查 tvID 是否已存在
            var existingtvID = db.videotb.FirstOrDefault(p => p.tvid == intvID);
            if (existingtvID != null)
            {
                ViewBag.notice = "该唯一标识已存在，请重新添加!";
                return View("AddVideo"); // 返回添加视图
            }
            // 创建新视频并添加到数据库
            var newVideo = new videotb
            {
                videoID = videoID,
                title = inTitle,
                link = inLink,
                cover=inCover,
                tag = inTag,
                year = inYear,
                rating = inRating,
                status = inStatus,
                hotscore = inHotScore,
                type = inType,
                tvid = intvID,
                desc = inDesc,
                bigcover = inBigCover,
                date = inDate,
                time = inTime,
                director = inDirector,
                actor = inActor
            };
            // 添加并保存
            db.videotb.Add(newVideo);
            db.SaveChanges();
            // 提示信息
            ViewBag.notice = "视频添加成功!";
            return View("AddVideo");
        }
        
        /* 添加评论 */
        // GET: AddComment
        public ActionResult AddComment()
        {
            return View("AddComment");
        }
        // 添加评论功能实现(提交表单)
        [HttpPost]
        public ActionResult AddComment(int comID, string username, string content, DateTime comtime, string tvid,string ifStatus)
        {
            // 检查comID是否已存在
            var existingComID = db.comtb.FirstOrDefault(p => p.comID == comID);
            if (existingComID != null)
            {
                ViewBag.notice = "该评论ID已存在，请重新添加!";
                return View("AddComment"); // 返回添加视图
            }
            //string类型，转换为int类型
            if (ifStatus == "1")
            {
                status = 1;
            }
            else
            {
                status = 0;
            }
            // 创建新评论并添加到数据库
            var newComment = new comtb
            {
                comID = comID,
                username = username,
                content = content,
                comtime = comtime,
                tvid = tvid,
                status = (sbyte?)status
            };
            // 添加并保存
            db.comtb.Add(newComment);
            db.SaveChanges();
            // 提示信息
            ViewBag.notice = "评论添加成功!";
            return View("AddComment");
        }
        
        /* 添加广告 */
        // GET: AddAdvertiser
        public ActionResult AddAdvertiser()
        {
            return View("AddAdvertiser");
        }
        //添加功能实现(提交表单)
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddAdvertiser(int id, string username, string inAdName, decimal balance,string inAdContent, DateTime startDate, DateTime endDate, string adStatus, float cost, string payStatus)
        {
            // 检查id是否已存在
            var existingId = db.advertisertb.FirstOrDefault(p => p.advertiserID == id);
            if (existingId != null)
            {
                ViewBag.notice = "该id已存在，请重新添加!";
                return View("AddAdvertiser"); // 返回添加视图
            }
            // 检查username是否已存在
            var existingUser = db.advertisertb.FirstOrDefault(p => p.username == username);
            if (existingUser != null)
            {
                ViewBag.notice = "该用户名已存在，请重新添加!";
                return View("AddAdvertiser"); // 返回添加视图
            }
            //string类型，转换为int类型
            switch (adStatus)
            {
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
            // 创建新用户并添加到数据库
            var newAdvertiser = new advertisertb
            {
                advertiserID = id,
                username = username,
                ad_name = inAdName,
                balance=balance,
                ad_content = inAdContent,
                start_date = startDate,
                end_date = endDate,
                status = (sbyte?)adstatus,
                cost = (decimal?)cost,
                payment_status = (sbyte?)adpay
        };
            //添加并保存
            db.advertisertb.Add(newAdvertiser);
            db.SaveChanges();
            //提示信息
            ViewBag.notice = "添加成功!";
            return View("AddAdvertiser");
        }
    }
}