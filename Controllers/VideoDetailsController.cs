using GProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GProject.Controllers
{
    public class VideoDetailsController : Controller
    {
        // 链接数据库
        private gpdbEntities db = new gpdbEntities();
        // GET: VideoDetails 关于前端传值给后端(Prompt9)
        public ActionResult Details(string tvid,string username)
        {
            var viewModel = new CommentModel
            {
                Comment = db.comtb.ToList(),
                Video = db.videotb.FirstOrDefault(v => v.tvid == tvid),
                User = db.usertb.ToList()
            };

            if (viewModel.Video == null)
            {
                TempData["notice"] = "未找到该视频，请重试！";
                return RedirectToAction("Index", "FrontStage");
            }

            return View(viewModel);
        }

        // 从api获取评论
        public ActionResult GetComments(string tvid,string commentId)
        {
            // videoId 是非主键字段 
            var comment = db.videotb.FirstOrDefault(v => v.tvid == tvid);
            // 电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6
            var channel = comment.type;
            var channel_id = 1;
            if (channel == "电影")
            {
                channel_id = 1;
            }
            else if(channel == "电视剧")
            {
                channel_id = 2;
            }
            else if (channel == "纪录片")
            {
                channel_id = 3;
            }
            else if (channel == "动漫")
            {
                channel_id = 4;
            }
            else if (channel == "综艺")
            {
                channel_id = 6;
            }
            var last_id = commentId;
            // 构建带有参数的 apiUrl
            var apiUrl = $"https://sns-comment.iqiyi.com/v3/comment/get_baseline_comments.action" +
                          $"?agent_type=118" +
                          $"&agent_version=9.11.5" +
                          $"&authcookie=null" +
                          $"&business_type=17" +
                          $"&channel_id={channel_id}" +
                          $"&content_id={tvid}" +
                          $"&last_id={last_id}" +
                          $"&need_vote=1" +
                          $"&page_size=40" +
                          $"&qyid=781a214eb3398301b866a5c83cd0bb5a" +
                          $"&sort=HOT" +
                          $"&tail_num=1";

            using (var client = new WebClient())
            {
                // 显式指定编码方式为 UTF-8
                client.Encoding = Encoding.UTF8;

                var json = client.DownloadString(apiUrl);
                return Content(json, "application/json");
            }
        }
        // 发表评论
        [HttpPost]
        public ActionResult ReleaseComments(string username, string content,string tvid)
        {
            // 获取表中最大的 ID，如果不存在则设置为 0（主键ID不能为空）
            int comID = db.comtb.Any() ? db.comtb.Max(u => u.comID) : 0;
            // 创建新评论并添加到数据库
            var newComment = new comtb
            {
                comID = comID+1,
                username = username,
                content = content,
                comtime = DateTime.Now,
                tvid = tvid,
                status = 0
            };
            // 添加并保存
            db.comtb.Add(newComment);
            db.SaveChanges();
            // 你可能想返回一个响应，比如一个成功的消息
            return Json(new { success = true, message = "评论发布成功" });
        }

    }
}