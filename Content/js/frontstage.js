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
/* 登录、注销 按钮显示 */
// 用户名显示
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
    // 从localStorage中获取'username'
    var storedUsername = localStorage.getItem("username");
    //_self：当前页面加载    _blank：通常在新标签页打开
    window.open('/FrontStage/Personal?user=' + storedUsername, '_self');
}

//打开后台管理
function openAdmin() {

    window.open('/BackStage/UserMan', '_self');

}

/* 放大的内容 */
// 获取所有li
const items = document.querySelectorAll('li');
items.forEach(item => {
    item.addEventListener('mouseover', () => {
        // 显示图片
        item.querySelector('.video-mask').classList.remove('hidden');
    });

    item.addEventListener('mouseout', () => {
        // 隐藏图片
        item.querySelector('.video-mask').classList.add('hidden');
    });
});

/* 轮播图片实现 */
// 当前显示的轮播图索引
var currentIndex = 0;
// 获取所有轮播图元素
var slides = document.getElementsByClassName("slide");
// 显示指定索引的轮播图
function showSlide(index) {
    // 隐藏所有轮播图
    for (var i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    // 显示选定的轮播图
    slides[index].style.display = "block";
}
// 切换到上一张轮播图
function prevSlide() {
    currentIndex = (currentIndex - 1 + slides.length) % slides.length;
    showSlide(currentIndex);
    restartTimer();
}

// 切换到下一张轮播图
function nextSlide() {
    currentIndex = (currentIndex + 1) % slides.length;
    showSlide(currentIndex);
    restartTimer();
}

// 重新启动定时器
function restartTimer() {
    clearInterval(timer); // 清除之前的定时器
    timer = setInterval(autoSlide, 3000); // 设置新的定时器
}
// 自动切换到下一张轮播图
function autoSlide() {
    nextSlide();
}
// 在页面加载时显示第一张轮播图
showSlide(currentIndex);
// 设置定时器，每隔3000毫秒（3秒）自动滑动到下一张轮播图
var timer = setInterval(autoSlide, 3000);
function showSlide(index) {
    // 计算轮播图容器的偏移量
    var offset = -index * 100 + "%";

    // 使用CSS transform属性来平滑滑动
    document.getElementById("slides").style.transform = "translateX(" + offset + ")";
}

