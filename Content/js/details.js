/* 登录、注销 按钮显示 */
// 当文档加载完成后执行的函数
$(document).ready(function () {
    // 从localStorage中获取'username'
    var storedUsername = localStorage.getItem("username");
    // 检查是否存在存储的用户名(是否登录)
    if (storedUsername) {
        // 将用户名显示在指定的div中
        $("#user-id").text(storedUsername);
        // 显示注销按钮
        $("#logout").show();
        // 隐藏登录按钮
        $("#login-btn").hide();
    } else {
        // 隐藏注销按钮
        $("#logout").hide();
        // 显示登录按钮
        $("#login-btn").show();
    }
});
/* 登出按钮 */
$("#logout").click(function (event) {
    // 阻止默认的链接点击行为
    event.preventDefault();
    // 清除localStorage中的用户名
    localStorage.removeItem("username");
    // 手动触发链接的点击事件
    window.location.href = $(this).attr("href");
});

/* 下拉框展示 */
// 当点击用户名时，切换下拉框的显示/隐藏
userId.addEventListener('click', function (event) {
    event.stopPropagation(); // 阻止事件冒泡
    dropdown.style.display = dropdown.style.display === 'none' ? 'block' : 'none';
});
// 当点击下拉框时，阻止事件冒泡，以防止下拉框被隐藏
dropdown.addEventListener('click', function (event) {
    event.stopPropagation();
});
// 当点击文档的其他部分时，隐藏下拉框
document.addEventListener('click', function () {
    dropdown.style.display = 'none';
});

/* 下拉框内容 */
//打开个人中心
function openProfile() {
    //_self：当前页面加载    _blank：通常在新标签页打开
    // 从localStorage中获取'username'
    var storedUsername = localStorage.getItem("username");
    //_self：当前页面加载    _blank：通常在新标签页打开
    window.open('/FrontStage/Personal?user=' + storedUsername, '_self');
}

//打开后台管理
function openAdmin() {

    window.open('/BackStage/UserMan', '_self');

}

/* 视频展示 */
// 视频播放,线路拼接
$(document).ready(function () {
    // 当选择框的值变化时
    $(".form-control").change(function () {
        // 获取选中的线路值
        var selectedLine = $(this).val();

        // 拼接新的视频链接
        var newVideoLink = selectedLine + link;

        // 更新视频链接
        $("iframe").attr("src", newVideoLink);

        // 刷新视频
        $("video")[0].load();
    });
});

// 获取刷新按钮
const refreshButton = document.getElementById('refreshButton');
// 为按钮添加点击事件
refreshButton.addEventListener('click', () => {

    // 获取iframe
    const iframe = document.querySelector('iframe');

    // 重新设置iframe的src属性以实现刷新
    iframe.src = iframe.src;

});

// 提交表单
document.getElementById('commentForm').addEventListener('submit', function (event) {
    event.preventDefault();

    var storedUsername = localStorage.getItem("username");
    var commentText = document.getElementById('commentText').value;
    if (storedUsername == null) {
        alert("请登录后再发表评论！")
        return false;
    }
    // 发送评论和昵称到后端
    fetch('/VideoDetails/ReleaseComments', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            username: storedUsername,
            content: commentText,
            tvid: tvid,
        }),
    })
        .then(response => response.json())
        .then(data => {
            // 清空评论文本框
            document.getElementById('commentText').value = '';
            alert("评论发表成功,请等待审核~");
            // 处理后端返回的数据
            console.log(data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
});
