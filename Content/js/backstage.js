/* BackStage */
// 验证是否为管理员（所有后端文件都使用此js）
function checkAdminAccess() {
    // 从浏览器的localStorage中获取"username"键的值，并将其存储在变量storedUsername中。
    var storedUsername = localStorage.getItem("username");
    // 使用fetch API向服务器发送一个GET请求，请求的URL是'/BackStage/CheckAdminStatus'，
    fetch('/BackStage/CheckAdminStatus?username=' + storedUsername)
        // 当收到响应后，使用response.json()方法将响应体解析为JavaScript对象。
        .then(response => response.json())
        // 当响应体被成功解析后，执行以下代码。
        .then(data => {
            // 检查从服务器返回的数据中的isAdmin属性是否为true。
            if (data.isAdmin) {
                // 如果用户是管理员，从<body>元素中移除'hidden'类，使页面内容可见。
                document.body.classList.remove('hidden');
            } else {
                // 如果用户不是管理员，显示一个警告消息。
                alert('您不是管理员，无法进入后台管理页面！');
                // 重定向用户到'/FrontStage/Index'页面。
                window.location.href = '/FrontStage/Index';
            }
        })
        // 发生任何错误
        .catch(error => {
            alert('服务器链接发生错误！');
            window.location.href = '/FrontStage/Index';
        });
}
// 立即调用函数
checkAdminAccess();

// 操作反馈信息提示（页面加载完毕执行）
window.onload = function () {
    // 获取TempDataNotice提示信息
    var notice1 = TempDataNotice;
    // 获取ViewBagnotice提示信息
    var notice2 = ViewBagNotice;
    // 如果提示信息不为空，显示弹窗
    if (notice1) {
        alert(notice1);
    } else if (notice2) {
        alert(notice2);
    }
};

// 用户名展示（当文档加载完成,即DOM加载完成）
$(document).ready(function () {
    // 从localStorage中获取'username'
    var storedUsername = localStorage.getItem("username");
    // 检查是否存在存储的用户名
    if (storedUsername) {
        // 将用户名显示在指定的div中
        $("#user-id").text(storedUsername);
    }
});

// 登出按钮（通过id找到此按钮）
$("#logout a").click(function (event) {
    // 阻止默认的链接点击行为
    event.preventDefault();
    // 清除localStorage中的用户名
    localStorage.removeItem("username");
    // 手动触发链接的点击事件
    window.location.href = $(this).attr("href");
});
