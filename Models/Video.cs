using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GProject.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Cover { get; set; }
        public string Tag { get; set; }
        public string Year { get; set; }
        public float Rating { get; set; }
        public string Status { get; set; }
        public int HotScore { get; set; }
        public string Type { get; set; }
        public string tvID { get; set; }
        public string Desc { get; set; }
        public string Date { get; set; }
        public int Time { get; set; }
        public string BigCover { get; set; }
        public string Director { get; set; }
        public string Actor { get; set; }
    }
    public class VideoHotModel
    {
        public List<videotb> Videos { get; set; }
        public List<videotb> Movies { get; set; }
        public List<videotb> Episodes { get; set; }
        public List<videotb> Variety { get; set; }
        public List<videotb> Anime { get; set; }
        public List<videotb> Documentary { get; set; }
        public List<string> Advertiser { get; set; }

    }
    public class CommentModel
    {
        public List<comtb> Comment { get; set; }
        public videotb Video { get; set; }
        public List<usertb> User { get; set; }
    }
}