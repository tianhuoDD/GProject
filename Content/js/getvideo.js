/* GetVideo */
// 更新电影
document.getElementById('getMovies').addEventListener('click', function () {
    var isConfirmed = window.confirm('确认更新电影吗？');
    if (isConfirmed) {
        // 显示加载弹窗
        document.getElementById('loadingModal').style.display = 'block';
        // 执行跳转
        window.location.href = '/GetVideo/GetMovies'; // 设置目标 URL
    }
});
// 更新电视剧
document.getElementById('getEpisodes').addEventListener('click', function () {
    var isConfirmed = window.confirm('确认更新电视剧吗？');
    if (isConfirmed) {
        // 显示加载弹窗
        document.getElementById('loadingModal').style.display = 'block';
        // 执行跳转
        window.location.href = '/GetVideo/GetEpisodes'; // 设置目标 URL
    }
});
// 更新综艺
document.getElementById('getVariety').addEventListener('click', function () {
    var isConfirmed = window.confirm('确认更新综艺吗？');
    if (isConfirmed) {
        // 显示加载弹窗
        document.getElementById('loadingModal').style.display = 'block';
        // 执行跳转
        window.location.href = '/GetVideo/GetVariety'; // 设置目标 URL
    }
});
// 更新动漫
document.getElementById('getAnime').addEventListener('click', function () {
    var isConfirmed = window.confirm('确认更新动漫吗？');
    if (isConfirmed) {
        // 显示加载弹窗
        document.getElementById('loadingModal').style.display = 'block';
        // 执行跳转
        window.location.href = '/GetVideo/GetAnime'; // 设置目标 URL
    }
});
// 更新纪录片
document.getElementById('getDocumentary').addEventListener('click', function () {
    var isConfirmed = window.confirm('确认更新纪录片吗？');
    if (isConfirmed) {
        // 显示加载弹窗
        document.getElementById('loadingModal').style.display = 'block';
        // 执行跳转
        window.location.href = '/GetVideo/GetDocumentary'; // 设置目标 URL
    }
});
