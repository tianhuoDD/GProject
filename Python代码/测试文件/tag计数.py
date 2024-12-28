import json
import requests
from collections import defaultdict

def video_info(BASE_URL, PARAMS, headers):
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
        tag = item.get("tag_pcw", "其他")
        sub_tags = tag.split(';')
        for sub_tag in sub_tags:
            sub_tag = sub_tag.strip()
            # 增加每个子标签的计数
            tag_count[sub_tag] += 1
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
# 在全局变量中使用defaultdict来自动初始化任何新标签的计数为0
tag_count = defaultdict(int)

# 电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6
channel_id = 3
tag_list = []
count = 0
page_id = 1
"""
根据不同页数爬取，每页30条数据
"""
while True:
    stop = video_info(BASE_URL, PARAMS, headers)
    if stop == 0:
        break
print(page_id)
# 打印每个标签和对应的计数
for tag, count in tag_count.items():
    print(f"{tag}: {count}")