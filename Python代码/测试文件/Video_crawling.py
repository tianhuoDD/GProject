import requests


def video_info(BASE_URL, PARAMS, headers):
    count = 1
    # 定义页数（每页有三十条）
    PARAMS["page_id"] = page_id
    # 改变分类   电影：1  电视剧：2
    PARAMS["channel_id"] = channel_id
    response = requests.get(BASE_URL, params=PARAMS, headers=headers)
    # 状态错误
    if response.status_code != 200:
        print("error:状态错误")
        return
    # # 测试json数据
    # res = response.text
    # print(res)
    # 存储json数据
    data = response.json()
    # 穷尽页数，结束
    if data["has_next"] == 0:
        return data["has_next"]
    # 验证“data”字段是否存在并且是一个列表
    if 'data' in data and isinstance(data['data'], list):
        with open(file_url, mode="a", encoding="utf-8") as file:
            for item in data['data']:
                title = item.get("title", "未知标题")
                link = item.get("page_url", "未知链接")
                cover = item.get("image_url_normal", "未知封面")
                tag = item.get("tag", "未知标签")
                year = item.get("date", {}).get("year", "未知年份")
                hot_score = item.get("hot_score", "未知热度")
                # 将提取的信息保存为字符串
                if channel_id == 1:
                    # 评分
                    rating = item.get("sns_score", "0.0")
                    info_str = f"""
                                标题: {title}
                                链接: {link}
                                封面: {cover}
                                标签: {tag}
                                年份: {year}
                                评分: {rating}
                                热度: {hot_score}
                                {"-" * 50}
                                """
                else:
                    # 级数状态
                    status = item.get("dq_updatestatus", "未知状态")
                    info_str = f"""
                                标题: {title}
                                链接: {link}
                                封面: {cover}
                                标签: {tag}
                                年份: {year}
                                状态: {status}
                                热度: {hot_score}
                                {"-" * 50}
                                """
                # 写入文件
                file.write(f"第{page_id}页，第{count}条")
                file.write(info_str)
                count += 1


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
    # 电影：1  电视剧：2
    "channel_id": 1,
    "tagName": "",
    "mode": 24
}
"""
变量设置
"""
# 默认第一页
page_id = 1
# 电影：1  电视剧：2
channel_id = 6
if channel_id == 1:
    file_url = "电影.txt"
elif channel_id == 2:
    file_url = "电视剧.txt"
elif channel_id == 3:
    file_url = "纪录片.txt"
elif channel_id == 4:
    file_url = "动漫.txt"
elif channel_id == 6:
    file_url = "综艺.txt"
"""
根据不同页数爬取，每页30条数据
"""
while True:
    stop = video_info(BASE_URL, PARAMS, headers)
    if stop == 0:
        break
    print(f"已打印第{page_id}页")
    page_id += 1