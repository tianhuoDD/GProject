/* Login */
//登录表单检查（本地检查）
function checkFormLog() {
    var usn = $("#usn").val();
    var pwd = $("#pwd").val();
    if (usn.length < 1) {
        $("#noticeUser").text("账号不能为空！");
        //阻止表单向后执行
        return false;
    }
    if (pwd.length < 1) {
        $("#noticePwd").text("密码不能为空！");
        //阻止表单向后执行
        return false;
    }
    /* 登录用户名收集 */
    // 阻止默认的表单提交行为
    event.preventDefault();
    // 获取输入框的值
    var loggedInUser = $("#usn").val();
    // 存储数据到 localStorage
    localStorage.setItem("username", loggedInUser);
    // 提交表单
    $("#myForm").get(0).submit(); // 使用表单的提交方法
    // 返回 false 以确保不执行默认的表单提交
    return false;
}
// 输入框获得焦点时清除提示信息
$("#usn").focus(function () {
    $("#noticeUser").text("");
    $("#notice").text("");
});
$("#pwd").focus(function () {
    $("#noticePwd").text("");
    $("#notice").text("");
});

/* Register */
//注册检查表单
function checkFormReg() {
    var usn2 = $("#usn2").val();
    var pwd2 = $("#pwd2").val();
    var cofPwd = $("#cofPwd").val();
     if (usn2.length < 1) {
        $("#noticeUser2").text("账号不能为空！");
        //阻止表单向后执行
        return false;
    }
    if (pwd2.length < 1) {
        $("#noticePwd2").text("密码不能为空！");
        //阻止表单向后执行
        return false;
    }
    // 账号长度为2-16位，只能为字母和数字的组合
    if (!/^[a-zA-Z0-9]{2,16}$/.test(usn2)) {
        $("#noticeUser2").text("账号格式不正确，长度为2-16位，只能包含字母和数字的组合。");
        // 阻止表单向后执行
        return false;
    }
    // 密码长度为6-16位，必须包含字母和数字
    if (!/^(?=.*[a-zA-Z])(?=.*\d).{6,16}$/.test(pwd2)) {
        $("#noticePwd2").text("密码格式不正确，长度为6-16位，必须包含字母和数字。");
        // 阻止表单向后执行
        return false;
    }
    if (cofPwd.length < 1) {
        $("#noticeCofPwd").text("确认密码不能为空！");
        //阻止表单向后执行
        return false;
    }
    if (pwd2 != cofPwd) {
        $("#noticeCofPwd").text("两次密码不一致！");
        //阻止表单向后执行
        return false;
    }
}
/* 注册提示信息清除 */
$("#usn2").focus(function () {
    $("#noticeUser2").text("");
});
$("#pwd2").focus(function () {
    $("#noticePwd2").text("");
});
$("#cofPwd").focus(function () {
    $("#noticeCofPwd").text("");
});

/* Login、Register */
//密码可见与不可见
function togglePassword(passwordFieldId, eyeIconID) {
    var passwordField = document.getElementById(passwordFieldId);
    var eyeIcon = document.getElementById(eyeIconID);
    //如果为password
    if (passwordField.type === "password") {
        passwordField.type = "text";
        eyeIcon.src = "../Content/img/eye1.png"; // 切换为密码可见时的图标路径
    } else {
        passwordField.type = "password";
        eyeIcon.src = "../Content/img/eye2.png"; // 切换为密码不可见时的图标路径
    }
}