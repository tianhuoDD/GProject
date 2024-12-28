import json
import requests
from multiprocessing import Pool


def video_info(channel_id):
    BASE_URL = "https://mesh.if.iqiyi.com/portal/videolib/pcw/data"
    PARAMS = {
        "version": "1.0",
        "ret_num": 30,
        "page_id": 1,
        "device_id": "781a214eb3398301b866a5c83cd0bb5a",
        "passport_id": "",
        "watch_list": "852317800,2306,0;...",
        "recent_selected_tag": "综合;喜剧;2023;...",
        "recent_search_query": "",
        "ip": "202.108.14.240",
        "scale": 150,
        "dfp": "a0401a7bbdbe484de1adbe1730b68025f04f703b...",
        "channel_id": channel_id,
        "tagName": "",
        "mode": 24
    }
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36"
    }

    video_data = []
    page_id = 1
    count = 0
    while True:
        # 每次循环更新page_id
        PARAMS['page_id'] = page_id
        response = requests.get(BASE_URL, params=PARAMS, headers=headers)
        if response.status_code != 200 or 'data' not in response.json():
            break
        data = response.json()
        if data["has_next"] == 0:
            break
        for item in data['data']:
            title = item.get("title", "未知标题")
            link = item.get("page_url", "未知链接")
            cover = item.get("image_url_normal", "未知封面")
            tag = item.get("tag_pcw", "其他")
            # 只有电影使用rating 评分
            rating = item.get("sns_score", "0.0")
            # 其他情况使用  状态
            status = item.get("dq_updatestatus", "其他")
            year = item.get("date", {}).get("year", "其他")
            hotscore = item.get("hot_score", "0")
            # 电影：1  电视剧：2  纪录片:3   动漫:4   综艺:6
            type = item.get("channel_id", "其他")
            if type == 1:
                type = "电影"
            elif type == 2:
                type = "电视剧"
            elif type == 3:
                type = "纪录片"
            elif type == 4:
                type = "动漫"
            elif type == 6:
                type = "综艺"
            else:
                type = "其他"
            video_data.append(
                {
                    "title": title,
                    "link": link,
                    "cover": cover,
                    "tag": tag,
                    "rating": rating,
                    "status": status,
                    "year": year,
                    "hotscore": hotscore,
                    "type": type,
                }
            )
            count += 1
        page_id += 1
    return count, json.dumps(video_data, ensure_ascii=False)


def main():
    channel_ids = [1, 2, 3, 4, 6]
    with Pool(len(channel_ids)) as p:
        results = p.map(video_info, channel_ids)

    total_count = sum(count for count, _ in results)
    all_videos = [videos for _, videos in results]
    for videos in all_videos:
        print(videos)
    print(f"总共更新了 '{total_count}' 条数据。")


if __name__ == '__main__':
    main()
