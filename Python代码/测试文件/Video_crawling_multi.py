import requests
from multiprocessing import Pool, cpu_count


# 获取视频信息的函数
def video_info(args):
    # 基础URL、参数、请求头、页面ID、频道ID和文件URL
    BASE_URL, PARAMS, headers, page_id, channel_id, file_url = args
    count = 1
    PARAMS["page_id"] = page_id  # 设置页面ID
    PARAMS["channel_id"] = channel_id  # 设置频道ID
    # 发起GET请求
    response = requests.get(BASE_URL, params=PARAMS, headers=headers)
    # 如果返回的状态码不是200，则返回错误信息
    if response.status_code != 200:
        return f"第{page_id}页错误:状态错误"
    # 获得json数据
    data = response.json()
    # 如果没有下一页，则返回None
    if data["has_next"] == 0:
        return None
    # 如果返回数据中有"data"字段且为列表，则提取信息
    if 'data' in data and isinstance(data['data'], list):
        # 打开文件以追加模式
        with open(file_url, mode="a", encoding="utf-8") as file:
            # 遍历数据列表
            for item in data['data']:
                # 提取信息
                title = item.get("title", "未知标题")
                link = item.get("page_url", "未知链接")
                cover = item.get("image_url_normal", "未知封面")
                tag = item.get("tag", "未知标签")
                year = item.get("date", {}).get("year", "未知年份")
                hot_score = item.get("hot_score", "未知热度")

                # 根据频道ID构造信息字符串
                if channel_id == 1:
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
                # 将信息写入文件
                file.write(info_str)
                count += 1
    return f"已打印第{page_id}页"


# 主函数
def main():
    # 请求头
    headers = {
        "User-Agent": "Mozilla/5.0 ... (a long string indicating the browser)"
    }
    # 基础URL
    BASE_URL = "https://mesh.if.iqiyi.com/portal/videolib/pcw/data"
    # 请求参数
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
    # 频道ID
    channel_id = 6
    # 文件名映射
    file_url_map = {
        1: "电影.txt",
        2: "电视剧.txt",
        3: "纪录片.txt",
        4: "动漫.txt",
        6: "综艺.txt"
    }
    # 获取文件名
    file_url = file_url_map.get(channel_id, "默认.txt")
    # 创建参数列表 目前页数只有26页
    params_list = [(BASE_URL, PARAMS, headers, i, channel_id, file_url) for i in range(1, 27)]
    # 创建进程池
    pool = Pool(processes=cpu_count())
    # 使用多进程获取视频信息
    results = pool.map(video_info, params_list)

    # 打印结果
    for result in results:
        if result:
            print(result)


# 如果是主模块，则运行主函数
if __name__ == "__main__":
    main()
