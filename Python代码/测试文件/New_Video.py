import json
import requests


# 获得导演与演员
def get_director_actor(tvid):
    base_url = "https://pcw-api.iqiyi.com/video/video/baseinfo/"
    # 拼接tvid到URL
    url = f"{base_url}{tvid}"
    response = requests.get(url)
    if response.status_code == 200:
        data = response.json()
        director = clean_gbk_compatible(data["data"]["people"]["director"][0]["name"])
        actors = data["data"]["people"]["main_charactor"]
        actor_list = [actor["name"] for actor in actors if "name" in actor]
        # 使用";"将列表中的元素拼接成一个字符串，然后存储到变量actor中
        actor = clean_gbk_compatible(";".join(actor_list))
        return director, actor
    else:
        return " ", " "

# 获得大封面图
"""
imageSize":[
            "160_90",
            "220_124",
            "284_160",
            "320_180",
            "480_270",
            "1080_608",
            "1248_702",
            "832_468",
            "592_333",
            "480_360",
            "195_260",
            "260_360",
            "579_772",
            "318_424",
            "255_340",
            "128_128",
            "580_435"
        ],"""
def get_BigCover(cover):
    if cover.endswith("579_772.webp"):
        return clean_gbk_compatible(cover.replace("579_772.webp", "1248_702.webp"))
    else:
        return cover  # 或者返回一个错误提示，因为结尾不匹配

# 处理无法被gbk显示的字符串
def clean_gbk_compatible(text):
    # 首先，确保输入是字符串类型
    if not isinstance(text, str):
        text = str(text)
    try:
        text.encode('gbk')
    except UnicodeEncodeError:
        # 有不能转换的字符，逐个检查并去除
        text = text.encode('gbk', 'ignore').decode('gbk')
    return text

"""
视频标题 视频链接 视频封面 视频标签（需追加） 视频评分 视频状态 视频年份 视频热度 视频类型
"""
def video_info(BASE_URL, PARAMS, headers):
    global count
    global video_json
    global page_id
    # 定义页数（每页有三十条）
    PARAMS["page_id"] = page_id
    # 改变分类   电影：1  电视剧：2
    PARAMS["channel_id"] = channel_id
    response = requests.get(BASE_URL, params=PARAMS, headers=headers)
    # 状态错误
    if response.status_code != 200:
        print("error:状态错误")
        return
    # 存储json数据
    data = response.json()
    # 穷尽页数，结束
    if data["has_next"] == 0:
        return data["has_next"]
    # 返回json数据
    for item in data['data']:
        title = clean_gbk_compatible(item.get("title", "未知标题"))
        link = clean_gbk_compatible(item.get("page_url", "未知链接"))
        cover = clean_gbk_compatible(item.get("image_url_normal", "未知封面"))
        bigcover = get_BigCover(cover)
        tag = clean_gbk_compatible(item.get("tag_pcw", "其他"))
        year = clean_gbk_compatible(item.get("date", {}).get("year", "其他"))
        # 只有电影使用rating 评分
        rating = clean_gbk_compatible(item.get("sns_score", "0.0"))
        # 其他情况使用  状态
        status = clean_gbk_compatible(item.get("dq_updatestatus", "其他"))
        if status == "":
            status = " "
        hotscore = clean_gbk_compatible(item.get("hot_score", "0"))
        # 电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6
        type = clean_gbk_compatible(item.get("channel_id", "其他"))
        tvid = clean_gbk_compatible(item.get("tv_id", "0000"))
        desc = clean_gbk_compatible(item.get("description", "其他"))
        date = clean_gbk_compatible(item.get("showDate", "0000-00-00"))
        time = clean_gbk_compatible(item.get("time_length", "其他"))
        director, actor = get_director_actor(tvid)
        if type == "1":
            type = "电影"
        elif type == "2":
            type = "电视剧"
        elif type == "3":
            type = "纪录片"
        elif type == "4":
            type = "动漫"
        elif type == "6":
            type = "综艺"
        else:
            type = "其他"
        video_data.append(
            {
                "title": title,
                "link": link,
                "cover": cover,
                "tag": tag,
                "year": year,
                "rating": rating,
                "status": status,
                "hotscore": hotscore,
                "type": type,
                "tvid": tvid,
                "desc": desc,
                "date": date,
                "time": time,
                "bigcover": bigcover,
                "director": director,
                "actor": actor,
            }
        )
        # 将提取后的数据转换为JSON
        video_json = json.dumps(video_data, ensure_ascii=False)
        # 统计视频条数
        count += 1
    # 统计视频页数
    page_id += 1


"""
main
"""
# 爱奇艺api
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36"
}
BASE_URL = "https://mesh.if.iqiyi.com/portal/videolib/pcw/data"
PARAMS = {
    "version": "1.0",
    # 每页显示多少视频数据
    "ret_num": 30,
    "page_id": 1,
    "device_id": "781a214eb3398301b866a5c83cd0bb5a",
    "passport_id": "",
    "watch_list": "852317800,2306,0;1179685151569500,7636,0;4139677129668500,3923,0;5921287477461900,4642,0;3328687457426500,5414,0",
    "recent_selected_tag": "综合;喜剧;2023;友谊;内地;最热;搞笑;免费;1;2",
    "recent_search_query": "",
    "ip": "202.108.14.240",
    "scale": 150,
    "dfp": "a0401a7bbdbe484de1adbe1730b68025f04f703bdf3190269167b927c4a5c025d4",
    # 电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6
    "channel_id": 1,
    "tagName": "",
    "mode": 24
}
"""
变量设置
"""

# 电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6
channel_id = 1
video_data = []
video_json = []
count = 0
page_id = 1
"""
根据不同页数爬取，每页30条数据
"""
while True:
    stop = video_info(BASE_URL, PARAMS, headers)
    if stop == 0:
        break
print(video_json)
print(f" 已更新‘{count}’条数据，")
