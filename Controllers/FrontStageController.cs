using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using GProject.Models;
using PagedList;
using System.Web.Http;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Domain;
using System.Threading.Tasks;
using Aop.Api.Util;
using System.Collections.Specialized;
using Aop.Api.Response;

namespace GProject.Controllers
{
    // 支付宝沙箱配置
    public class Config
    {
        //沙箱环境，请求支付链接的地址           
        public const string URL = "https://openapi-sandbox.dl.alipaydev.com/gateway.do";
        //支付宝正式环境 
        //https://openapi.alipaydev.com/gateway.do   
        //APPID即创建应用后生成,沙箱环境中的AppId
        public const string APPID = "9021000135616514";
        //这里是我们之前用秘钥工具生成的商户私钥 (.net需转码为 PKCS1 格式)
        public const string APP_PRIVATE_KEY = "MIIEogIBAAKCAQEAhEv9WVkm5ypPLOUMG8DhU0UCnF0kQsqtfuDPXE39UoAcb8XLQxIkfQcT9gctTv/eCJcBBkeVYVlr9UmNDJnpX/x1EhQyKeku90+O4hexez6GdKZvP9ajnXKtIZbvwlqq35HLkSf66PE6s2vHGkcT3QEBJlazCjFaO85ma7TfuH14dst7H8Qb7yBse8u8Qb6YPaXn6mHh+eJ/3uMzKmyHvzwxPqxEkSA3vTqqOHapptbGWyVNt9imL/KDyAehYOhachH8sO4jT2AajWHw2fQBMu2cL4wsGNcb3TKJMCvZVXQ8CgeN1RLHuISnfzT/RO/z9Sg2nea+PGRWEdGYagp4ywIDAQABAoIBABFUArMZQkc0+3nd5mAcO2c0usrQSFeXRHJB7+cfL/cGYLMjLSRz0+Oscajf98dJLEPjw3aXGbHOjJ1sLNVhs3enEY50pr1mlYg/G2/xuoVyAxp0Uk4CRxvwUUz9ABps1cTCy+8PVYX9Urr9eu6uMKPZ5dSntnu3SyJaPoANCOCZHjOAVvOposzt6GioHFqw/yBB9eoqjlRWl7Rm0Ji9V5/yC8rFgSGsvTdpTOHDKzzagLJWG5LuhFvL1narIn8dIS21ixNmyV7lU2aG8rkaoQq9ye/skGti5FaOiKuKiUWTuMiVDTojSYcxuMgyQnY3KnZ+2DnLm88vUZR8xHEf4QECgYEAyG8+nHQDEYTNUTTP6/dbWguoVgN0dAQaN9g+TpCKC6OB6Z31DoDL/uMjO5fBArdNmVxT90HxXB1DOi0oT8nR80y3yPPe/d86sJtjLVCvIlYj2Bu1p6kHULaBSizt+YYfUTqEH9iKCf0/G5IhHj8lsPH/q1oW+RM90LLgtOIjCVsCgYEAqPkKSqE9Jl9UI+i6StbA87hH0umJr9eS9Ojdhtni83scdvZUFSnrwW+6Qa7I1Ab8AefzClJKzL9pytep115Q2N2TrWNL17qfgATF8ZTMzJHoZ1Y1dpm0IEg/OfDGcGNX1EFh2s3+uwh5zAw9ylfQcdgso1e8/lZjVkGk/daB+VECgYBOjCH6SBCHxgZ8Q3kUHQzEvfrLMnFpoc3wgqLwQP6ITxxzQ/6sH1dSXQsKqI7wFIuphSA2rrX6hlE5NEcu9YV8ll0TuFZ0IQ4r9ckpEgyAJUfIwhsvSDNgsxT5z5+3AJO3TnpGzKJUBdW8ZUjrRI3tyPiL2zNTivHXnrIm0NQwLwKBgHY9bvoyQjwr0REftdzQ0ky502i/i13FGEzQ9tN6fNmseIq2bLgykYrWyBtWV6onCQm8adH7K+SCvWD38R2IHkLjKofWSCg5q4tFy831Niqgn2RQazEG9YjxhWziE7ppifg5mGQoaewvTit/FW4WbQJ9Jx9WMiY4BWNCpFYMWP9BAoGABU0QoOaMqCw9ckD5vW4e5uaAx9hyEgxv8pmAe4hLMskWzCrs7DL4+RrYUqco6FpkkBh8Q7OrIg9O5ZPQtbOO475wU5cKOEXxOD83uts8nu9WWOxyqPW+1bgPJmGejrXSvmbVjGst0J2pyfvtjRYxuBgy2kJaUofiUB/JSs7OwbU=";
        //参数返回格式，只支持json
        public const string FORMAT = "json";
        //支持GBK和UTF-8
        public const string CHARSET = "UTF-8";
        //支付宝公钥
        public const string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlMSVhsQSzFcIJfFIlnjTVVa0XOUfhH2644mrnWq5EberlxDdyIQNUCS+Vkf/3Vqcv6p72uqRnMJ4tYBV62JSz75Rl1WF8MxMiiVWZRY8LfY4y0tQzLq+e5XIBkRiZn+fSwt0o6JmZSnYUDD0pww3eiR04jDxc+p8Z5UuKRbWmXB1hq/jmMdiRfLUqWnC7N3Vr9W/jIjedggSd0axjcp+dtQoi5QFenkuwfPvWHoh6UUkGHYJBeVyplWVGdE/tkVd+Qfgz8zwwIctaDKWYUZzpPLn2puH2TAao5sBcZr6GpyFv6R29JYKmzVn4A71mpMigT+oWggjm8e9JmeSgwNhFwIDAQAB";

    }
    // 订单信息
    public class Order
    {
        // 用户名
        public string Username { get; set; }
        // 广告商名称
        public string AdName { get; set; }
        // 充值金额
        public string Money { get; set; }
        // 订单类型
        public string Type { get; set; }
        //订单的编号
        public string OrderNo { get; set; }
    }
    // 用户及广告及账单表
    public class CombinedViewModel
    {
        public usertb Usertb { get; set; }
        public advertisertb Advertisertb { get; set; }
        public transaction_recordtb Ordertb { get; set; }
    }
    // 广告模型
    public class AdvertiserInfoViewModel
    {
        public string Username { get; set; }
        public string AdContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
    }

    public class FrontStageController : Controller
    {
        // 链接数据库
        private gpdbEntities db = new gpdbEntities();
        // GET: FrontStage  首页 
        public ActionResult Index()
        {
            List<string> advertiserContent = new List<string>();
            var advertiser= db.advertisertb
                .Where(ad => (ad.status == 1 || ad.status == 2) && ad.payment_status == 1)
                .ToList();
            foreach (var ad in advertiser)
            {
                // 获取当前时间
                var now = DateTime.Now;
                // 检查当前时间是否在广告的开始日期和结束日期之间
                if (ad.start_date <= now && ad.end_date >= now)
                {
                    // 当前时间在广告的有效日期范围内，进行后续操作
                    ad.status = 2; // 播放中
                    // 将ad.ad_content添加到advertiserContent列表中
                    advertiserContent.Add(ad.ad_content);
                }
                else if(ad.end_date <= now)
                {
                    if (ad.status == 2)
                    {
                        ad.status = 3; // 已结束
                    }
                }
            }
            db.SaveChanges();
            /* 视频在同一页面输出不同数据(Prompt5) */
            // Take()来限制输出元素数量
            var videoQuery = db.videotb.OrderByDescending(video => video.hotscore).Take(8);
            var moviesQuery = db.videotb.Where(video => video.type == "电影").OrderByDescending(video => video.hotscore).Take(8);
            var episodesQuery = db.videotb.Where(video => video.type == "电视剧").OrderByDescending(video => video.hotscore).Take(8);
            var varietyQuery = db.videotb.Where(video => video.type == "综艺").OrderByDescending(video => video.hotscore).Take(8);
            var animeQuery = db.videotb.Where(video => video.type == "动漫").OrderByDescending(video => video.hotscore).Take(8);
            var documentaryQuery = db.videotb.Where(video => video.type == "纪录片").OrderByDescending(video => video.hotscore).Take(8);
            // 创建viewModel，传入Index，用于输出不同数据
            var viewModel = new VideoHotModel
            {
                Videos = videoQuery.ToList(),
                Movies = moviesQuery.ToList(),
                Episodes = episodesQuery.ToList(),
                Variety = varietyQuery.ToList(),
                Anime = animeQuery.ToList(),
                Documentary = documentaryQuery.ToList(),
                Advertiser = advertiserContent
            };
            return View("Index", viewModel);
        }
        // 搜索实现
        public ActionResult Search(int? page, string query)
        {
            ViewBag.SearchTerm = query;
            //检查 query 是否为空
            if (string.IsNullOrWhiteSpace(query))
            {
                // 如果搜索关键字为空，显示全部视频
                var allVideos = db.videotb.ToList();
                int pageSize2 = 12;
                int pageNumber2 = (page ?? 1);
                // 如果有结果，返回 true；否则返回 false
                ViewBag.result = allVideos.Any();
                return View(allVideos.ToPagedList(pageNumber2, pageSize2));
            }
            // 对数据库中的videotb表执行搜索操作
            var results = db.videotb
                .Where(video => video.title.Contains(query))
                .OrderBy(video => video.videoID);
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            // 如果有结果，返回 true；否则返回 false
            ViewBag.result = results.Any();
            // 将结果传递给视图
            return View(results.ToPagedList(pageNumber, pageSize));
        }

        //电影界面 页面、类型、地区、年份
        public ActionResult Movies(int? page, string genre, string region, string year, string sort)
        {
            ViewBag.genre = genre;
            ViewBag.region = region;
            ViewBag.year = year;
            ViewBag.sort = sort;
            //转换为IQueryable接口,用于执行LINQ查询，且为延长执行（点击相应按钮才会输出）
            var videoQuery = db.videotb.AsQueryable();
            // 已知的标签和地区列表
            var knownGenres = new List<string> { "喜剧", "犯罪", "悬疑", "功夫", "爱情", "动作", "奇幻", "科幻", "青春", "动画", "惊悚", "荒诞", "枪战", "魔幻", "武侠", "战争", "纯爱", "灾难", "家庭", "院线", "网络电影" };
            var knownRegions = new List<string> { "华语", "香港地区", "美国", "日本", "韩国", "泰国", "欧洲", "日语", "英语", "韩语", "法语", "粤语" };
            var knownYears = new List<string> { "2023", "2022", "2021", "2020", "2019", "2018", "2017", "2016", "2015", "2014", "2013", "2012" };
            // 类型筛选
            if (!string.IsNullOrWhiteSpace(genre))
            {
                if (genre == "其他")
                {
                    //除了已知标签外
                    videoQuery = videoQuery.Where(video => !knownGenres.Any(tag => video.tag.Contains(tag)));
                }
                else if (genre != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(genre));
                }
            }

            // 地区筛选
            if (!string.IsNullOrWhiteSpace(region))
            {
                if (region == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownRegions.Any(tag => video.tag.Contains(tag)));
                }
                else if (region != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(region));
                }
            }

            // 年份筛选
            if (!string.IsNullOrWhiteSpace(year))
            {
                if (year == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownYears.Any(y => video.year.Contains(y)));
                }
                else if (year != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.year == year);
                }
            }
            // 排序筛选
            switch (sort)
            {
                case "时间":
                    videoQuery = videoQuery.Where(video => video.type == "电影").OrderByDescending(video => video.year);
                    break;
                case "热度":
                    videoQuery = videoQuery.Where(video => video.type == "电影").OrderByDescending(video => video.hotscore);
                    break;
                case "评分":
                    videoQuery = videoQuery.Where(video => video.type == "电影").OrderByDescending(video => video.rating);
                    break;
                default:
                    //展示类型为“电影”的视频信息，并按ID排序(只有在调用页面时，才加载数据，减少内存)
                    videoQuery = videoQuery.Where(video => video.type == "电影").OrderBy(video => video.videoID);
                    break;
            }
            int pageSize = 12; // 每页显示行数
            int pageNumber = (page ?? 1); // 当前页码
            // 因为ToPagedList内部可能会使用Skip方法，所以需要在调用ToPagedList之前对结果进行排序
            return View(videoQuery.ToPagedList(pageNumber, pageSize));
        }
        //电视剧界面 页面、类型、地区、年份
        public ActionResult Episodes(int? page, string genre, string region, string year, string sort)
        {
            ViewBag.genre = genre;
            ViewBag.region = region;
            ViewBag.year = year;
            ViewBag.sort = sort;
            var videoQuery = db.videotb.AsQueryable();
            // 已知的标签和地区列表
            var knownGenres = new List<string> { "都市", "罪案", "悬疑", "家庭", "剧情", "青春", "言情", "偶像", "战争", "古装", "武侠", "谍战", "年代", "喜剧", "奇幻", "宫廷", "民国", "现代", "历史", "自制", "网剧" };
            var knownRegions = new List<string> { "内地", "台湾地区", "英语", "美剧", "韩语", "韩剧", "港剧", "粤语", "泰剧" };
            var knownYears = new List<string> { "2023", "2022", "2021", "2020", "2019", "2018", "2017", "2016", "2015", "2014", "2013", "2012" };
            // 类型筛选
            if (!string.IsNullOrWhiteSpace(genre))
            {
                if (genre == "其他")
                {
                    //除了已知标签外
                    videoQuery = videoQuery.Where(video => !knownGenres.Any(tag => video.tag.Contains(tag)));
                }
                else if (genre != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(genre));
                }
            }

            // 地区筛选
            if (!string.IsNullOrWhiteSpace(region))
            {
                if (region == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownRegions.Any(tag => video.tag.Contains(tag)));
                }
                else if (region != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(region));
                }
            }

            // 年份筛选
            if (!string.IsNullOrWhiteSpace(year))
            {
                if (year == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownYears.Any(y => video.year.Contains(y)));
                }
                else if (year != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.year == year);
                }
            }
            switch (sort)
            {
                case "时间":
                    videoQuery = videoQuery.Where(video => video.type == "电视剧").OrderByDescending(video => video.year);
                    break;
                case "热度":
                    videoQuery = videoQuery.Where(video => video.type == "电视剧").OrderByDescending(video => video.hotscore);
                    break;
                case "评分":
                    videoQuery = videoQuery.Where(video => video.type == "电视剧").OrderByDescending(video => video.rating);
                    break;
                default:
                    //展示类型为“电视剧”的视频信息，并按ID排序(只有在调用页面时，才加载数据，减少内存)
                    videoQuery = videoQuery.Where(video => video.type == "电视剧").OrderBy(video => video.videoID);
                    break;
            }
            int pageSize = 12; // 每页显示行数
            int pageNumber = (page ?? 1); // 当前页码
            // 因为ToPagedList内部可能会使用Skip方法，所以需要在调用ToPagedList之前对结果进行排序
            return View(videoQuery.ToPagedList(pageNumber, pageSize));
        }
        //综艺界面 页面、类型、地区、年份
        public ActionResult Variety(int? page, string genre, string region, string year, string sort)
        {
            ViewBag.genre = genre;
            ViewBag.region = region;
            ViewBag.year = year;
            ViewBag.sort = sort;
            var videoQuery = db.videotb.AsQueryable();
            // 已知的标签和地区列表
            var knownGenres = new List<string> { "真人秀", "音乐", "竞演", "游戏", "搞笑", "美食", "生活", "旅游", "晚会", "舞蹈", "纪实", "小品", "相声", "脱口秀", "文化" };
            var knownRegions = new List<string> { "内地", "韩国", "港台", "欧美" };
            var knownYears = new List<string> { "2023", "2022", "2021", "2020", "2019", "2018", "2017", "2016", "2015", "2014", "2013", "2012" };
            // 类型筛选
            if (!string.IsNullOrWhiteSpace(genre))
            {
                if (genre == "其他")
                {
                    //除了已知标签外
                    videoQuery = videoQuery.Where(video => !knownGenres.Any(tag => video.tag.Contains(tag)));
                }
                else if (genre != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(genre));
                }
            }

            // 地区筛选
            if (!string.IsNullOrWhiteSpace(region))
            {
                if (region == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownRegions.Any(tag => video.tag.Contains(tag)));
                }
                else if (region != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(region));
                }
            }

            // 年份筛选
            if (!string.IsNullOrWhiteSpace(year))
            {
                if (year == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownYears.Any(y => video.year.Contains(y)));
                }
                else if (year != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.year == year);
                }
            }
            switch (sort)
            {
                case "时间":
                    videoQuery = videoQuery.Where(video => video.type == "综艺").OrderByDescending(video => video.year);
                    break;
                case "热度":
                    videoQuery = videoQuery.Where(video => video.type == "综艺").OrderByDescending(video => video.hotscore);
                    break;
                case "评分":
                    videoQuery = videoQuery.Where(video => video.type == "综艺").OrderByDescending(video => video.rating);
                    break;
                default:
                    //展示类型为“电视剧”的视频信息，并按ID排序(只有在调用页面时，才加载数据，减少内存)
                    videoQuery = videoQuery.Where(video => video.type == "综艺").OrderBy(video => video.videoID);
                    break;
            }
            int pageSize = 12; // 每页显示行数
            int pageNumber = (page ?? 1); // 当前页码
            // 因为ToPagedList内部可能会使用Skip方法，所以需要在调用ToPagedList之前对结果进行排序
            return View(videoQuery.ToPagedList(pageNumber, pageSize));
        }
        //动漫界面 页面、类型、地区、年份
        public ActionResult Anime(int? page, string genre, string region, string year, string sort)
        {
            ViewBag.genre = genre;
            ViewBag.region = region;
            ViewBag.year = year;
            ViewBag.sort = sort;
            var videoQuery = db.videotb.AsQueryable();
            // 已知的标签和地区列表
            var knownGenres = new List<string> { "日语", "动画", "搞笑", "奇幻", "动作", "冒险", "漫画改编", "热血", "恋爱", "玄幻", "动态漫画", "日常", "美食", "治愈", "校园", "励志", "合家欢", "短剧", "原创", "TV版" };
            var knownRegions = new List<string> { "中国大陆", "日本", "欧美" };
            var knownYears = new List<string> { "2023", "2022", "2021", "2020", "2019", "2018", "2017", "2016", "2015", "2014", "2013", "2012" };
            // 类型筛选
            if (!string.IsNullOrWhiteSpace(genre))
            {
                if (genre == "其他")
                {
                    //除了已知标签外
                    videoQuery = videoQuery.Where(video => !knownGenres.Any(tag => video.tag.Contains(tag)));
                }
                else if (genre != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(genre));
                }
            }

            // 地区筛选
            if (!string.IsNullOrWhiteSpace(region))
            {
                if (region == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownRegions.Any(tag => video.tag.Contains(tag)));
                }
                else if (region != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(region));
                }
            }

            // 年份筛选
            if (!string.IsNullOrWhiteSpace(year))
            {
                if (year == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownYears.Any(y => video.year.Contains(y)));
                }
                else if (year != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.year == year);
                }
            }
            switch (sort)
            {
                case "时间":
                    videoQuery = videoQuery.Where(video => video.type == "动漫").OrderByDescending(video => video.year);
                    break;
                case "热度":
                    videoQuery = videoQuery.Where(video => video.type == "动漫").OrderByDescending(video => video.hotscore);
                    break;
                case "评分":
                    videoQuery = videoQuery.Where(video => video.type == "动漫").OrderByDescending(video => video.rating);
                    break;
                default:
                    //展示类型为“电视剧”的视频信息，并按ID排序(只有在调用页面时，才加载数据，减少内存)
                    videoQuery = videoQuery.Where(video => video.type == "动漫").OrderBy(video => video.videoID);
                    break;
            }
            int pageSize = 12; // 每页显示行数
            int pageNumber = (page ?? 1); // 当前页码
            // 因为ToPagedList内部可能会使用Skip方法，所以需要在调用ToPagedList之前对结果进行排序
            return View(videoQuery.ToPagedList(pageNumber, pageSize));
        }
        //纪录片界面 页面、类型、地区、年份
        public ActionResult Documentary(int? page, string genre, string region, string year, string sort)
        {
            ViewBag.genre = genre;
            ViewBag.region = region;
            ViewBag.year = year;
            ViewBag.sort = sort;
            var videoQuery = db.videotb.AsQueryable();
            // 已知的标签和地区列表
            var knownGenres = new List<string> {"文化遗产", "人文", "旅游", "自然", "地理", "海洋",
                                                "野生动物", "风景", "探险", "历史", "古代史", "美食",
                                                "探秘", "长纪录", "科技", "军事", "动物揭秘", "萌宠",
                                                "社会", "荒野", "系列纪录片", "BBC"};

            var knownRegions = new List<string> { "国内", "国外", "英语" };
            var knownYears = new List<string> { "2023", "2022", "2021", "2020", "2019", "2018", "2017", "2016", "2015", "2014", "2013", "2012" };
            // 类型筛选
            if (!string.IsNullOrWhiteSpace(genre))
            {
                if (genre == "其他")
                {
                    //除了已知标签外
                    videoQuery = videoQuery.Where(video => !knownGenres.Any(tag => video.tag.Contains(tag)));
                }
                else if (genre != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(genre));
                }
            }

            // 地区筛选
            if (!string.IsNullOrWhiteSpace(region))
            {
                if (region == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownRegions.Any(tag => video.tag.Contains(tag)));
                }
                else if (region != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.tag.Contains(region));
                }
            }

            // 年份筛选
            if (!string.IsNullOrWhiteSpace(year))
            {
                if (year == "其他")
                {
                    videoQuery = videoQuery.Where(video => !knownYears.Any(y => video.year.Contains(y)));
                }
                else if (year != "全部")
                {
                    videoQuery = videoQuery.Where(video => video.year == year);
                }
            }
            switch (sort)
            {
                case "时间":
                    videoQuery = videoQuery.Where(video => video.type == "纪录片").OrderByDescending(video => video.year);
                    break;
                case "热度":
                    videoQuery = videoQuery.Where(video => video.type == "纪录片").OrderByDescending(video => video.hotscore);
                    break;
                case "评分":
                    videoQuery = videoQuery.Where(video => video.type == "纪录片").OrderByDescending(video => video.rating);
                    break;
                default:
                    //展示类型为“电视剧”的视频信息，并按ID排序(只有在调用页面时，才加载数据，减少内存)
                    videoQuery = videoQuery.Where(video => video.type == "纪录片").OrderBy(video => video.videoID);
                    break;
            }
            int pageSize = 12; // 每页显示行数
            int pageNumber = (page ?? 1); // 当前页码
            // 因为ToPagedList内部可能会使用Skip方法，所以需要在调用ToPagedList之前对结果进行排序
            return View(videoQuery.ToPagedList(pageNumber, pageSize));
        }
        // 个人信息修改
        public ActionResult Personal(string user)
        {
            if (!string.IsNullOrWhiteSpace(user))
            {
                var User = db.usertb.FirstOrDefault(p => p.username == user);
                var Advertiser = db.advertisertb.FirstOrDefault(p => p.username == user);
                var Order = db.transaction_recordtb.FirstOrDefault(p => p.username == user);
                if (Advertiser != null) {
                    var viewModel = new CombinedViewModel
                    {
                        Usertb = User, // 从数据库或其他地方获取数据
                        Advertisertb = Advertiser, // 同上
                        Ordertb = Order
                    };
                    return View("Personal", viewModel);
                }
                else
                {
                    var viewModel = new CombinedViewModel
                    {
                        Usertb = User, // 从数据库或其他地方获取数据
                        Advertisertb = new advertisertb() // 同上
                    };
                    return View("Personal", viewModel);
                }

            }
            TempData["notice"] = "用户未登录！";
            return RedirectToAction("Index");
        }
        // 视频采集页面跳转
        public ActionResult VideoCrawling()
        {
            return View("VideoCrawling");
        }
        // 修改密码
        [HttpPost]
        public ActionResult SavePassword(string newPassword, string username)
        {
            try
            {
                var user = db.usertb.FirstOrDefault(u => u.username == username);
                user.password = newPassword;
                db.SaveChanges();
                // 返回成功的 JSON 响应
                return Json(new { success = true, message = "密码修改成功" });
            }
            catch (Exception ex)
            {
                // 处理异常，返回失败的 JSON 响应
                return Json(new { success = false, message = "修改失败：" + ex.Message });
            }
        }
        // 修改昵称
        [HttpPost]
        public ActionResult SaveName(string newName, string username)
        {
            try
            {
                var user = db.usertb.FirstOrDefault(u => u.username == username);
                user.nickname = newName;
                db.SaveChanges();
                // 返回成功的 JSON 响应
                return Json(new { success = true, message = "昵称修改成功" });
            }
            catch (Exception ex)
            {
                // 处理异常，返回失败的 JSON 响应
                return Json(new { success = false, message = "修改失败：" + ex.Message });
            }
        }
        // 修改头像
        [HttpPost]
        public ActionResult SaveIcon(string username)
        {
            try
            {
                // 处理上传的文件，更新数据库等操作
                var file = Request.Files[0];
                // 将文件转换为 byte[]
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
                var user = db.usertb.FirstOrDefault(u => u.username == username);
                user.icon = fileBytes;
                db.SaveChanges();
                // 返回新的头像URL
                var imageUrl = user.icon; // 替换为实际的头像URL
                return Json(new { success = true, imageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // 申请成为广告商
        [HttpPost]
        public ActionResult SaveAdvertiser(string adName, string username)
        {
            // 检查adName是否已存在
            var existingAdName = db.advertisertb.FirstOrDefault(p => p.ad_name == adName);
            if (existingAdName != null)
            {
                return Json(new { success = false, message = "申请失败：该广告商名称已存在" });
            }
            var existingUser = db.advertisertb.FirstOrDefault(p => p.username == username);
            if (existingUser != null)
            {
                return Json(new { success = false, message = "申请失败：该用户名已申请，请进入广告管理界面" });
            }
            var newAdvertiser = new advertisertb
            {
                username = username,
                ad_name = adName,
                balance=0
            };
            //添加并保存
            db.advertisertb.Add(newAdvertiser);
            db.SaveChanges();
            return Json(new { success = true, message = "广告商申请成功" });
        }
        // 暂存广告信息(未支付)
        [HttpPost]
        public ActionResult SaveAdvertiserInfo([FromBody] AdvertiserInfoViewModel info)
        {
            var existingUser = db.advertisertb.FirstOrDefault(p => p.username == info.Username);
            if (existingUser == null)
            {
                return Json(new { success = false, message = "广告暂存失败：未申请成为广告商" });
            }
            // 修改并保存
            existingUser.ad_content = info.AdContent;
            existingUser.start_date = info.StartDate;
            existingUser.end_date = info.EndDate;
            existingUser.cost = info.Cost;
            existingUser.status = 0;
            existingUser.payment_status = 0;
            db.SaveChanges();
            return Json(new { success = true, message = "广告暂存成功" });
        }
        // 提交广告信息(已支付)
        [HttpPost]
        public ActionResult SubmitAdvertiserInfo([FromBody] AdvertiserInfoViewModel model)
        {
            var existingUser = db.advertisertb.FirstOrDefault(p => p.username == model.Username);
            if (existingUser == null)
            {
                return Json(new { success = false, message = "广告提交失败：未申请成为广告商" });
            }
            if(existingUser.balance - model.Cost >= 0)
            {
                existingUser.balance -= model.Cost; // 计算余额
            }
            else
            {
                return Json(new { success = false, message = "支付失败！余额不足！" });
            }
            // 修改并保存
            existingUser.ad_content = model.AdContent;
            existingUser.start_date = model.StartDate;
            existingUser.end_date = model.EndDate;
            existingUser.cost = model.Cost;
            existingUser.status = 0;
            existingUser.payment_status = 1;
            // 创建新用户并添加到数据库
            var order = new transaction_recordtb
            {
                username= existingUser.username,
                ad_name= existingUser.ad_name,
                transaction_type="广告",
                amount= model.Cost,
                transaction_date=DateTime.Now,
                alipay_trade_no= DateTime.Now.ToString("yyyyMMddHHmmss"),
                trade_status= "TRADE_SUCCESS"
            };
            //添加并保存
            db.transaction_recordtb.Add(order);
            db.SaveChanges();
            return Json(new { success = true, message = "广告提交并支付成功" });
        }
        // 注销广告商
        [HttpPost]
        public ActionResult DelAdvertiserInfo(string username)
        {
            // 通过用户名查找广告商信息
            var ad = db.advertisertb.FirstOrDefault(a => a.username == username);
            // 找不到该广告商
            if (ad == null)
            {
                return Json(new { success = false, message = "删除失败：广告商不存在" });
            }
            //删除用户，并保存数据库
            db.advertisertb.Remove(ad);
            db.SaveChanges();
            // 返回成功的 JSON 响应
            return Json(new { success = true, message = "注销成功!" });
        }
        // 查询订单状态 - 方法
        [HttpPost]
        public string QueryOrder(string trade_no = null, string out_trade_no = null)
        {
            DefaultAopClient client = new DefaultAopClient(Config.URL, Config.APPID, Config.APP_PRIVATE_KEY, "json", "1.0", "RSA2", Config.ALIPAY_PUBLIC_KEY, Config.CHARSET, false);
            AlipayTradeQueryModel model = new AlipayTradeQueryModel();
            if (!string.IsNullOrEmpty(trade_no))
            {
                model.TradeNo = trade_no; // 如果提供了支付宝交易号，则使用它
            }
            if (!string.IsNullOrEmpty(out_trade_no))
            {
                model.OutTradeNo = out_trade_no; // 如果提供了商户订单号，则使用它
            }
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.SetBizModel(model);

            AlipayTradeQueryResponse response = null;
            try
            {
                response = client.Execute(request);
                return response.TradeStatus;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        // 充值功能
        [HttpPost]
        public ActionResult PayMoney(Order order)
        {
            
            IAopClient client = new DefaultAopClient(Config.URL, Config.APPID, Config.APP_PRIVATE_KEY, Config.FORMAT, "2.0", "RSA2", Config.ALIPAY_PUBLIC_KEY, Config.CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：
            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();//创建API对应的request类,请求返回二维码
            AlipayTradePagePayRequest requestPagePay = new AlipayTradePagePayRequest();//请求返回支付宝支付网页
            string OutTradeNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            AlipayTradePagePayModel model = new AlipayTradePagePayModel
            {
                Body = order.Type, // 订单描述
                Subject = order.Type, // 订单标题
                TotalAmount = order.Money, // 订单金额
                OutTradeNo = OutTradeNo // 商家订单号
            };


            model.StoreId = "12580资源网"; // 门店编号
            model.ProductCode = "FAST_INSTANT_TRADE_PAY"; // 产品码-即时到账服务
            requestPagePay.SetBizModel(model);
            //这是要注意的，支付后就要通过他完成回调，这里填写你要跳转页面的地址
            requestPagePay.SetReturnUrl($"https://localhost:44368/FrontStage/Notify");
            var response = client.SdkExecute(requestPagePay);//Execute(request);
            // 创建流水账单
            var orderTemp = new transaction_recordtb
            {
                username = order.Username,
                ad_name = order.AdName,
                transaction_type = "充值",
                amount = 0m,
                transaction_date = DateTime.Now,
                alipay_trade_no = OutTradeNo,
                trade_status = "TRADE_CREATE" // 已创建订单,但未支付
            };
            var queryResult = QueryOrder(out_trade_no: OutTradeNo);
            orderTemp.trade_status = string.IsNullOrEmpty(queryResult) ? "TRADE_CREATE" : queryResult;

            if (decimal.TryParse(order.Money, out decimal money))
            {
                orderTemp.amount = money;
            }
            //添加并保存
            db.transaction_recordtb.Add(orderTemp);
            db.SaveChanges();
            if (!response.IsError)
            {
                var res = new
                {
                    success = true,
                    out_trade_no = response.OutTradeNo,
                    // qr_code = response.QrCode,    //二维码字符串
                    pay_url = Config.URL + "?" + response.Body
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    success = false,
                };
                return Json(res);
            }

        }
        // request 回来的信息组成的数组 - 方法
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.QueryString;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }
            // 然后移除
            sArray.Remove("username");
            sArray.Remove("ad_name");
            return sArray;

        }
        // 同步接收通知
        public ActionResult Notify(string out_trade_no,decimal total_amount, string trade_no, DateTime timestamp)
        {
            Dictionary<string, string> sArray = GetRequestGet();
            if (sArray.Count != 0)
            {
                // 验签
                bool flag = AlipaySignature.RSACheckV1(sArray, Config.ALIPAY_PUBLIC_KEY, Config.CHARSET, "RSA2", false);
                if (flag)
                {
                    var order = db.transaction_recordtb.FirstOrDefault(p => p.alipay_trade_no == out_trade_no);
                    var advertiser = db.advertisertb.FirstOrDefault(p => p.username == order.username);
                    if (order != null)
                    {
                        advertiser.balance += total_amount;
                        order.amount = total_amount;
                        order.transaction_date = timestamp;
                        order.alipay_trade_no = trade_no;
                        order.trade_status = QueryOrder(trade_no:trade_no);
                        // 保存
                        db.SaveChanges();
                        return RedirectToAction("Personal", "FrontStage", new { user = order.username });

                    }
                }
            }
            return View();
        }


    }

}