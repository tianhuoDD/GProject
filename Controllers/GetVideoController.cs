using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GProject.Models;
using Newtonsoft.Json;

namespace GProject.Controllers
{
    public class GetVideoController : Controller
    {
        private gpdbEntities db = new gpdbEntities();
        private int count = 0;
        /* 获取视频的方法(Prompt3) */
        //获取电影
        public ActionResult GetMovies()
        {
            /* 先删除所有视频，再添加视频，实现视频信息的更新（HotScore需要实时更新） */
            // 查询所有类型为“电影”的视频
            var movies = db.videotb.Where(v => v.type == "电影").ToList();
            // 删除查询到的所有电影
            db.videotb.RemoveRange(movies);
            // 保存更改到数据库
            db.SaveChanges();

            // 设置 Python 脚本执行的参数
            var startInfo = new ProcessStartInfo(@"E:\Python\PythonVenv\自己写的代码\Scripts\python.exe")
            {
                Arguments = @"E:\Python\PyCharm\Project\自己写的代码\毕业设计\Video_Asp_Movies.py", // Python 脚本的路径 （注意：文件名“不能有空格”）
                UseShellExecute = false, // 不使用操作系统的外壳程序启动进程（关闭这个后，必须使用绝对路径）
                RedirectStandardOutput = true, // 重定向输出，以便在 .NET 应用程序中捕获
                CreateNoWindow = true // 不创建窗口
            };
            try
            {
                // 启动进程来执行 Python 脚本
                using (var process = Process.Start(startInfo))
                {
                    // 获取标准输出流
                    using (var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.Default, true, 8192))
                    {
                        // 读取所有输出文本
                        string output = reader.ReadToEnd();
                        // 以换行符分割输出文本，得到每一行作为一个元素的数组
                        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        // 第一行作为结果
                        string result = lines[0];
                        // 将第一行输出文本解析为 Video 类型的对象
                        var videos = JsonConvert.DeserializeObject<List<Video>>(result);
                        // 其余的输出为可视化部分
                        string pythonData = lines[1];
                        
                    // 遍历解析后的视频数据
                    foreach (var videoData in videos)
                    {
                        // 首先检查数据库中是否已存在具有相同标题的视频(api会提供相同的标题的视频)
                        bool videoExists = db.videotb.Any(v => v.title == videoData.Title);
                        // 如果视频不存在，则执行添加操作
                        if (!videoExists)
                        {
                            // 获取表中最大的 ID，如果不存在则设置为 0（主键ID不能为空）
                            int maxId = db.videotb.Any() ? db.videotb.Max(u => u.videoID) : 0;
                            // 创建一个新的 videotb 对象，填充数据  (python返回的数据应与videotb中的表头相同)
                            var video = new videotb
                            {
                                videoID = maxId + 1,
                                title = videoData.Title,
                                link = videoData.Link,
                                cover = videoData.Cover,
                                tag = videoData.Tag,
                                year = videoData.Year,
                                rating = videoData.Rating,
                                status = videoData.Status,
                                hotscore = videoData.HotScore,
                                type = videoData.Type,
                                tvid = videoData.tvID,
                                desc = videoData.Desc,
                                date = videoData.Date,
                                time = videoData.Time,
                                bigcover = videoData.BigCover,
                                director = videoData.Director,
                                actor = videoData.Actor
                            };
                            // 将新视频对象添加到数据库上下文中
                            db.videotb.Add(video);
                            // 保存所有更改到数据库
                            db.SaveChanges();
                        }
                        else
                        {
                            //记录重复视频数量
                            count = count + 1;
                        }

                    }

                    // pythonData作用域在using中
                    TempData["notice"] = "视频更新成功!" + pythonData + "已删除重复视频‘" + count + "’条。";
                    return RedirectToAction("VideoMan", "BackStage");

                    }

                }
            }
            catch (Exception ex)
            {
                // 如果发生异常，返回错误信息
                // 在实际应用中，这里应该有更完善的错误处理逻辑
                return Content("An error occurred: " + ex.Message);
            }
        }

        //获取电视剧
        public ActionResult GetEpisodes()
        {
            /* 先删除所有视频，再添加视频，实现视频信息的更新（HotScore需要实时更新） */
            // 查询所有类型为“电视剧”的视频
            var episodes = db.videotb.Where(v => v.type == "电视剧").ToList();
            // 删除查询到的所有电影
            db.videotb.RemoveRange(episodes);
            // 保存更改到数据库
            db.SaveChanges();

            // 设置 Python 脚本执行的参数
            var startInfo = new ProcessStartInfo(@"E:\Python\PythonVenv\自己写的代码\Scripts\python.exe")
            {
                Arguments = @"E:\Python\PyCharm\Project\自己写的代码\毕业设计\Video_Asp_Episodes.py", // Python 脚本的路径 （注意：文件名“不能有空格”）
                UseShellExecute = false, // 不使用操作系统的外壳程序启动进程（关闭这个后，必须使用绝对路径）
                RedirectStandardOutput = true, // 重定向输出，以便在 .NET 应用程序中捕获
                CreateNoWindow = true // 不创建窗口
            };
            try
            {
                // 启动进程来执行 Python 脚本
                using (var process = Process.Start(startInfo))
                {
                    // 获取标准输出流
                    using (var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.Default, true, 8192))
                    {
                        // 读取所有输出文本
                        string output = reader.ReadToEnd();

                        // 以换行符分割输出文本，得到每一行作为一个元素的数组
                        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        // 第一行作为结果
                        string result = lines[0];
                        // 将第一行输出文本解析为 Video 类型的对象
                        var videos = JsonConvert.DeserializeObject<List<Video>>(result);
                        // 其余的输出为可视化部分
                        string pythonData = lines[1];
                        // 遍历解析后的视频数据
                        foreach (var videoData in videos)
                        {
                            // 首先检查数据库中是否已存在具有相同标题的视频(api会提供相同的标题的视频)
                            bool videoExists = db.videotb.Any(v => v.title == videoData.Title);
                            // 如果视频不存在，则执行添加操作
                            if (!videoExists)
                            {
                                // 获取表中最大的 ID，如果不存在则设置为 0（主键ID不能为空）
                                int maxId = db.videotb.Any() ? db.videotb.Max(u => u.videoID) : 0;
                                // 创建一个新的 videotb 对象，填充数据  (python返回的数据应与videotb中的表头相同)
                                var video = new videotb
                                {
                                    videoID = maxId + 1,
                                    title = videoData.Title,
                                    link = videoData.Link,
                                    cover = videoData.Cover,
                                    tag = videoData.Tag,
                                    year = videoData.Year,
                                    rating = videoData.Rating,
                                    status = videoData.Status,
                                    hotscore = videoData.HotScore,
                                    type = videoData.Type,
                                    tvid = videoData.tvID,
                                    desc = videoData.Desc,
                                    date = videoData.Date,
                                    time = videoData.Time,
                                    bigcover = videoData.BigCover,
                                    director = videoData.Director,
                                    actor = videoData.Actor
                                };

                                // 将新视频对象添加到数据库上下文中
                                db.videotb.Add(video);
                                // 保存所有更改到数据库
                                db.SaveChanges();
                            }
                            else
                            {
                                //记录重复视频数量
                                count = count + 1;
                            }

                        }
                        // pythonData作用域在using中
                        TempData["notice"] = "视频更新成功!" + pythonData + "已删除重复视频‘" + count + "’条。";
                        return RedirectToAction("VideoMan", "BackStage");
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果发生异常，返回错误信息
                // 在实际应用中，这里应该有更完善的错误处理逻辑
                return Content("An error occurred: " + ex.Message);
            }
        }

        //获取综艺
        public ActionResult GetVariety()
        {
            /* 先删除所有视频，再添加视频，实现视频信息的更新（HotScore需要实时更新） */
            // 查询所有类型为“综艺”的视频
            var variety = db.videotb.Where(v => v.type == "综艺").ToList();
            // 删除查询到的所有综艺
            db.videotb.RemoveRange(variety);
            // 保存更改到数据库
            db.SaveChanges();

            // 设置 Python 脚本执行的参数
            var startInfo = new ProcessStartInfo(@"E:\Python\PythonVenv\自己写的代码\Scripts\python.exe")
            {
                Arguments = @"E:\Python\PyCharm\Project\自己写的代码\毕业设计\Video_Asp_Variety.py", // Python 脚本的路径 （注意：文件名“不能有空格”）
                UseShellExecute = false, // 不使用操作系统的外壳程序启动进程（关闭这个后，必须使用绝对路径）
                RedirectStandardOutput = true, // 重定向输出，以便在 .NET 应用程序中捕获
                CreateNoWindow = true, // 不创建窗口
            };
            try
            {
                // 启动进程来执行 Python 脚本
                using (var process = Process.Start(startInfo))
                {
                    // 获取标准输出流
                    using (var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.Default, true, 8192))
                    {
                        // 读取所有输出文本
                        string output = reader.ReadToEnd();
                        // 以换行符分割输出文本，得到每一行作为一个元素的数组
                        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        // 第一行作为结果
                        string result = lines[0];
                        // 将第一行输出文本解析为 Video 类型的对象
                        var videos = JsonConvert.DeserializeObject<List<Video>>(result);
                        // 其余的输出为可视化部分
                        string pythonData = lines[1];
                        // 遍历解析后的视频数据
                        foreach (var videoData in videos)
                        {
                            // 首先检查数据库中是否已存在具有相同标题的视频(api会提供相同的标题的视频)
                            bool videoExists = db.videotb.Any(v => v.title == videoData.Title);
                            // 如果视频不存在，则执行添加操作
                            if (!videoExists)
                            {
                                // 获取表中最大的 ID，如果不存在则设置为 0（主键ID不能为空）
                                int maxId = db.videotb.Any() ? db.videotb.Max(u => u.videoID) : 0;
                                // 创建一个新的 videotb 对象，填充数据  (python返回的数据应与videotb中的表头相同)
                                var video = new videotb
                                {
                                    videoID = maxId + 1,
                                    title = videoData.Title,
                                    link = videoData.Link,
                                    cover = videoData.Cover,
                                    tag = videoData.Tag,
                                    year = videoData.Year,
                                    rating = videoData.Rating,
                                    status = videoData.Status,
                                    hotscore = videoData.HotScore,
                                    type = videoData.Type,
                                    tvid = videoData.tvID,
                                    desc = videoData.Desc,
                                    date = videoData.Date,
                                    time = videoData.Time,
                                    bigcover = videoData.BigCover,
                                    director = videoData.Director,
                                    actor = videoData.Actor
                                };

                                // 将新视频对象添加到数据库上下文中
                                db.videotb.Add(video);
                                // 保存所有更改到数据库
                                db.SaveChanges();
                            }
                            else
                            {
                                //记录重复视频数量
                                count = count + 1;
                            }

                        }
                        // pythonData作用域在using中
                        TempData["notice"] = "视频更新成功!" + pythonData + "已删除重复视频‘" + count + "’条。";
                        return RedirectToAction("VideoMan", "BackStage");
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果发生异常，返回错误信息
                // 在实际应用中，这里应该有更完善的错误处理逻辑
                return Content("An error occurred: " + ex.Message);
            }
        }

        //获取动漫
        public ActionResult GetAnime()
        {
            /* 先删除所有视频，再添加视频，实现视频信息的更新（HotScore需要实时更新） */
            // 查询所有类型为“动漫”的视频
            var anime = db.videotb.Where(v => v.type == "动漫").ToList();
            // 删除查询到的所有综艺
            db.videotb.RemoveRange(anime);
            // 保存更改到数据库
            db.SaveChanges();

            // 设置 Python 脚本执行的参数
            var startInfo = new ProcessStartInfo(@"E:\Python\PythonVenv\自己写的代码\Scripts\python.exe")
            {
                Arguments = @"E:\Python\PyCharm\Project\自己写的代码\毕业设计\Video_Asp_Anime.py", // Python 脚本的路径 （注意：文件名“不能有空格”）
                UseShellExecute = false, // 不使用操作系统的外壳程序启动进程（关闭这个后，必须使用绝对路径）
                RedirectStandardOutput = true, // 重定向输出，以便在 .NET 应用程序中捕获
                CreateNoWindow = true // 不创建窗口
            };
            try
            {
                // 启动进程来执行 Python 脚本
                using (var process = Process.Start(startInfo))
                {
                    // 获取标准输出流
                    using (var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.Default, true, 8192))
                    {
                        // 读取所有输出文本
                        string output = reader.ReadToEnd();
                        // 以换行符分割输出文本，得到每一行作为一个元素的数组
                        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        // 第一行作为结果
                        string result = lines[0];
                        // 将第一行输出文本解析为 Video 类型的对象
                        var videos = JsonConvert.DeserializeObject<List<Video>>(result);
                        // 其余的输出为可视化部分
                        string pythonData = lines[1];
                        // 遍历解析后的视频数据
                        foreach (var videoData in videos)
                        {
                            // 首先检查数据库中是否已存在具有相同标题的视频(api会提供相同的标题的视频)
                            bool videoExists = db.videotb.Any(v => v.title == videoData.Title);
                            // 如果视频不存在，则执行添加操作
                            if (!videoExists)
                            {
                                // 获取表中最大的 ID，如果不存在则设置为 0（主键ID不能为空）
                                int maxId = db.videotb.Any() ? db.videotb.Max(u => u.videoID) : 0;
                                // 创建一个新的 videotb 对象，填充数据  (python返回的数据应与videotb中的表头相同)
                                var video = new videotb
                                {
                                    videoID = maxId + 1,
                                    title = videoData.Title,
                                    link = videoData.Link,
                                    cover = videoData.Cover,
                                    tag = videoData.Tag,
                                    year = videoData.Year,
                                    rating = videoData.Rating,
                                    status = videoData.Status,
                                    hotscore = videoData.HotScore,
                                    type = videoData.Type,
                                    tvid = videoData.tvID,
                                    desc = videoData.Desc,
                                    date = videoData.Date,
                                    time = videoData.Time,
                                    bigcover = videoData.BigCover,
                                    director = videoData.Director,
                                    actor = videoData.Actor
                                };

                                // 将新视频对象添加到数据库上下文中
                                db.videotb.Add(video);
                                // 保存所有更改到数据库
                                db.SaveChanges();
                            }
                            else
                            {
                                //记录重复视频数量
                                count = count + 1;
                            }

                        }
                        // pythonData作用域在using中
                        TempData["notice"] = "视频更新成功!" + pythonData + "已删除重复视频‘" + count + "’条。";
                        return RedirectToAction("VideoMan", "BackStage");
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果发生异常，返回错误信息
                // 在实际应用中，这里应该有更完善的错误处理逻辑
                return Content("An error occurred: " + ex.Message);
            }
        }

        //获取纪录片
        public ActionResult GetDocumentary()
        {
            /* 先删除所有视频，再添加视频，实现视频信息的更新（HotScore需要实时更新） */
            // 查询所有类型为“电视剧”的视频
            var documentary = db.videotb.Where(v => v.type == "纪录片").ToList();
            // 删除查询到的所有电影
            db.videotb.RemoveRange(documentary);
            // 保存更改到数据库
            db.SaveChanges();

            // 设置 Python 脚本执行的参数
            var startInfo = new ProcessStartInfo(@"E:\Python\PythonVenv\自己写的代码\Scripts\python.exe")
            {
                Arguments = @"E:\Python\PyCharm\Project\自己写的代码\毕业设计\Video_Asp_Documentary.py", // Python 脚本的路径 （注意：文件名“不能有空格”）
                UseShellExecute = false, // 不使用操作系统的外壳程序启动进程（关闭这个后，必须使用绝对路径）
                RedirectStandardOutput = true, // 重定向输出，以便在 .NET 应用程序中捕获
                CreateNoWindow = true // 不创建窗口
                /* RedirectStandardError = true // 重定向标准错误 */
            };
            try
            {
                // 启动进程来执行 Python 脚本
                using (var process = Process.Start(startInfo))
                {
                    /* 返回错误文本(Prompt6)
                        string error = process.StandardError.ReadToEnd(); */
                    // 获取标准输出流
                    using (var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.Default, true, 8192))
                    {
                        // 读取所有输出文本
                        string output = reader.ReadToEnd();
                        // 以换行符分割输出文本，得到每一行作为一个元素的数组
                        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        // 第一行作为结果
                        string result = lines[0];
                        // 将第一行输出文本解析为 Video 类型的对象
                        var videos = JsonConvert.DeserializeObject<List<Video>>(result);
                        // 其余的输出为可视化部分
                        string pythonData = lines[1];
                        // 遍历解析后的视频数据
                        foreach (var videoData in videos)
                        {
                            // 首先检查数据库中是否已存在具有相同标题的视频(api会提供相同的标题的视频)
                            bool videoExists = db.videotb.Any(v => v.title == videoData.Title);
                            // 如果视频不存在，则执行添加操作
                            if (!videoExists)
                            {
                                // 获取表中最大的 ID，如果不存在则设置为 0（主键ID不能为空）
                                int maxId = db.videotb.Any() ? db.videotb.Max(u => u.videoID) : 0;
                                // 创建一个新的 videotb 对象，填充数据  (python返回的数据应与videotb中的表头相同)
                                var video = new videotb
                                {
                                    videoID = maxId + 1,
                                    title = videoData.Title,
                                    link = videoData.Link,
                                    cover = videoData.Cover,
                                    tag = videoData.Tag,
                                    year = videoData.Year,
                                    rating = videoData.Rating,
                                    status = videoData.Status,
                                    hotscore = videoData.HotScore,
                                    type = videoData.Type,
                                    tvid = videoData.tvID,
                                    desc = videoData.Desc,
                                    date = videoData.Date,
                                    time = videoData.Time,
                                    bigcover = videoData.BigCover,
                                    director = videoData.Director,
                                    actor = videoData.Actor
                                };

                                // 将新视频对象添加到数据库上下文中
                                db.videotb.Add(video);
                                // 保存所有更改到数据库
                                db.SaveChanges();
                            }
                            else
                            {
                                //记录重复视频数量
                                count = count + 1;
                            }

                        }
                        // pythonData作用域在using中
                        TempData["notice"] = "视频更新成功!" + pythonData + "已删除重复视频‘" + count + "’条。";
                        return RedirectToAction("VideoMan", "BackStage");
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果发生异常，返回错误信息
                // 在实际应用中，这里应该有更完善的错误处理逻辑
                return Content("An error occurred: " + ex.Message);
            }
        }

    }

}
