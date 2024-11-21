using GProject.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GProject.Controllers
{
    public class SearchController : Controller
    {
        private gpdbEntities db = new gpdbEntities();

        // GET: SearchUser  根据用户名搜索
        public ActionResult SearchUser(int? page,string SearchTerm)
        {
            //检查searchTerm是否为空
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // 如果搜索关键字为空，显示全部用户
                var allUsers = db.usertb.ToList();
                int pageSize2 = 20;
                int pageNumber2 = (page ?? 1);
                return View("SearchUser", allUsers.ToPagedList(pageNumber2, pageSize2));
            }
            // 在数据库中执行搜索（关键字搜索）
            var results = db.usertb
                .Where(u => u.username.Contains(SearchTerm))
                .ToList();
            // 将搜索结果和关键字传递给视图
            ViewBag.SearchTerm = SearchTerm;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View("SearchUser", results.ToPagedList(pageNumber, pageSize));
        }

        // GET: SearchVideo  根据视频名称title搜索
        public ActionResult SearchVideo(int? page, string SearchTerm)
        {
            //检查searchTerm是否为空
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // 如果搜索关键字为空，显示全部视频
                var allVideos = db.videotb.ToList();
                int pageSize2 = 20;
                int pageNumber2 = (page ?? 1);
                return View("SearchVideo",allVideos.ToPagedList(pageNumber2,pageSize2));
            }
            // 在数据库中执行搜索（关键字-包含搜索）
            var results = db.videotb
                .Where(v => v.title.Contains(SearchTerm) || v.tvid.Contains(SearchTerm))
                .ToList();
            ViewBag.SearchTerm = SearchTerm;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View("SearchVideo", results.ToPagedList(pageNumber, pageSize));
        }

        // GET: SearchComment  根据用户名搜索
        public ActionResult SearchComment(int? page, string SearchTerm)
        {
            //检查searchTerm是否为空
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // 如果搜索关键字为空，显示全部评论
                var allComments = db.comtb.ToList();
                int pageSize2 = 20;
                int pageNumber2 = (page ?? 1);
                return View("SearchComment", allComments.ToPagedList(pageNumber2, pageSize2));
            }
            // 在数据库中执行搜索（关键字搜索）
            var results = db.comtb
                .Where(v => v.username.Contains(SearchTerm))
                .ToList();
            ViewBag.SearchTerm = SearchTerm;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View("SearchComment", results.ToPagedList(pageNumber, pageSize));
        }
        // GET: SearchAdvertiser  根据广告商名搜索
        public ActionResult SearchAdvertiser(int? page, string SearchTerm)
        {
            //检查searchTerm是否为空
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // 如果搜索关键字为空，显示全部用户
                var allAdvertisers = db.advertisertb.ToList();
                int pageSize2 = 20;
                int pageNumber2 = (page ?? 1);
                return View("SearchAdvertiser", allAdvertisers.ToPagedList(pageNumber2, pageSize2));
            }
            // 在数据库中执行搜索（关键字搜索）
            var results = db.advertisertb
                .Where(u => u.ad_name.Contains(SearchTerm))
                .ToList();
            // 将搜索结果和关键字传递给视图
            ViewBag.SearchTerm = SearchTerm;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View("SearchAdvertiser", results.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchOrder(int? page, string searchTerm, string transactionType, string tradeStatus, string startDateFilter, string endDateFilter)
        {
            var query = db.transaction_recordtb.AsQueryable();

            // 关键字搜索
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => u.ad_name.Contains(searchTerm));
            }

            // 交易类型筛选
            if (!string.IsNullOrWhiteSpace(transactionType))
            {
                query = query.Where(u => u.transaction_type == transactionType);
            }

            // 交易状态筛选
            if (!string.IsNullOrWhiteSpace(tradeStatus))
            {
                query = query.Where(u => u.trade_status == tradeStatus);
            }

            // 时间区间筛选
            if (!string.IsNullOrWhiteSpace(startDateFilter) && DateTime.TryParse(startDateFilter, out DateTime startDate))
            {
                query = query.Where(u => u.transaction_date >= startDate);
            }

            if (!string.IsNullOrWhiteSpace(endDateFilter) && DateTime.TryParse(endDateFilter, out DateTime endDate))
            {
                // 注意：为了包含结束日期当天的数据，将结束日期加一天
                endDate = endDate.AddDays(1);
                query = query.Where(u => u.transaction_date < endDate);
            }
            // 将搜索结果和关键字传递给视图
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TransactionType = transactionType;
            ViewBag.TradeStatus = tradeStatus;
            ViewBag.StartDate = startDateFilter;
            ViewBag.EndDate = endDateFilter;
            decimal totalAmount = 0;
            try
            {
                // 计算总和
                totalAmount = query.Sum(order => order.amount);
            }
            catch
            {
                totalAmount = 0;
            }
            
            ViewBag.TotalAmount = totalAmount;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View("SearchOrder", query.ToList().ToPagedList(pageNumber, pageSize));
        }

    }
}
