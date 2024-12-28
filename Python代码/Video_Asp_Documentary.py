import json
import requests
from multiprocessing import Pool
import time

# 防止访问url过快
def make_request(url, retries=5):
    for _ in range(retries):
        try:
            response = requests.get(url)
            response.raise_for_status()  # 抛出HTTPError异常
            return response
        except requests.exceptions.RequestException as e:
            print(f"Request failed: {e}")
            time.sleep(1)  # 等待一段时间后重试
    return None  # 如果重试次数用尽，返回None或者适当的默认值
# 获得导演与演员
def get_director_actor(tvid):
    base_url = "https://pcw-api.iqiyi.com/video/video/baseinfo/"
    url = f"{base_url}{tvid}"
    response = make_request(url)
    data=response.json()
    # 确保data["data"]是一个字典
    if isinstance(data.get("data"), dict):
        # 确保"people"键存在且其值是一个字典
        if isinstance(data["data"].get("people"), dict):
            # 确保"director"键存在且其值是一个列表
            if isinstance(data["data"]["people"].get("director"), list):
                # 确保列表不为空并且列表中的第一个元素是一个字典
                if data["data"]["people"]["director"] and isinstance(data["data"]["people"]["director"][0], dict):
                    director_name = data["data"]["people"]["director"][0].get("name", " ")
                    if director_name:
                        director = clean_gbk_compatible(director_name)
                    else:
                        director = " "
                else:
                    director = " "
            else:
                director = " "
            try:
                actors = data["data"]["people"]["main_charactor"]
                actor_list = [actor["name"] for actor in actors if "name" in actor]
                actor = "/".join(actor_list).encode('gbk', 'ignore').decode('gbk')
            except KeyError:
                actor = " "
            return director, actor
        else:
            return " ", " "
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

def video_info(page_id, channel_id):
    headers = {
        "User-Agent": "Mozilla/5.0 ..."
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
    # 这里需要定义 video_data 作为此函数的局部变量
    video_data = []
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
        bigcover = clean_gbk_compatible(get_BigCover(cover))
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
        """
        避免 ASP 读取时出现 An error occurred: Unexpected character encountered while parsing value: R. Path '', line 0, position 0.
        """
        # 替换所有反斜杠
        desc = desc.replace(r"\\", "")
        # 替换所有'\n'
        desc = desc.replace('\n', '')
        desc = desc.replace('\r', '')
        desc = desc.replace('\x0b', '')
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
    page_id+=1
    return json.dumps(video_data, ensure_ascii=False)


# 主函数
def main(channel_id, max_pages):
    pool = Pool(processes=32)  # 选择你想要的进程数
    # 为每个页码创建任务
    tasks = [(page_id, channel_id) for page_id in range(1, max_pages + 1)]
    # map函数会自动分配任务给进程
    results = pool.starmap(video_info, tasks)
    pool.close()  # 关闭进程池，不再接受新的任务
    pool.join()  # 等待所有进程结束

    # 合并所有子进程返回的 JSON 数据
    all_videos = []
    for video_json in results:
        all_videos.extend(json.loads(video_json))
    count = len(all_videos)
    return all_videos, count


# 调用主函数
if __name__ == "__main__":
    # 电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6
    channel_id = 3
    max_pages = 10  # 最大页数
    all_videos_data,count = main(channel_id, max_pages)
    # 如果你需要保存数据到文件或者进行其他操作，可以在这里继续
    json_string = json.dumps(all_videos_data, ensure_ascii=False)
    print(json_string)
    # 打印更新的数据数量
    print(f"已更新 ‘{count}’ 条数据。")

