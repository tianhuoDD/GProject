/* 用户管理 */
// 添加按钮实现
document.getElementById('addButtonUser').addEventListener('click', function () {
    window.location.href = '/Add/AddUser'; // 设置目标 URL
});
// 清空用户添加表单
function clearFormAddUser() {
    document.getElementById("id").value = "";
    document.getElementById("inUser").value = "";
    document.getElementById("inPwd").value = "";
    document.getElementById("inNickname").value = "";
    document.getElementById("regTime").value = "";
    document.getElementById("inIcon").value = "";
    // 如果是单选框，根据实际情况设置选中状态
    document.getElementById("ifAdmin1").checked = false;
    document.getElementById("ifAdmin0").checked = false;
}
// 提交用户时检查是否有空信息
function checkFormAddUser() {
    var id = $("#id").val();
    var username = $("#inUser").val();
    var pwd = $("#inPwd").val();
    var nickname = $("#inNickname").val();
    var regtime = $("#regTime").val();
    var icon = $("#inIcon").val();
    var ifAdmin = $("input[name='ifAdmin']:checked").val();
    if (id == "" || username == "" || pwd == "" || nickname == "" || regtime == "" || icon == "" || ifAdmin == "") {
        alert("用户表单信息不完整！");
        return false;
    }
    if (isNaN(parseInt(id))) {
        alert("请输入有效的用户ID！");
        return false;
    }
}
// 确认删除用户
function confirmDeleteUser(id) {
    var isConfirmed = window.confirm('确认删除用户 #' + id + ' 吗？');
    return isConfirmed;
}

/* EditComment */
// 清空评论表单
function clearFormEditUser() {
    document.getElementById("id").value = "";
    document.getElementById("inUser").value = "";
    document.getElementById("inPwd").value = "";
    document.getElementById("inNickname").value = "";
    document.getElementById("regTime").value = "";
    document.getElementById("inIcon").value = "";
    // 如果是单选框，根据实际情况设置选中状态
    document.getElementById("ifAdmin1").checked = false;
    document.getElementById("ifAdmin0").checked = false;
}
// 提交时检查评论表单是否有空信息
function checkFormEditUser() {
    var username = $("#inUser").val();
    var pwd = $("#inPwd").val();
    var nickname = $("#inNickname").val();
    var regtime = $("#regTime").val();
    var icon = $("#inIcon").val();
    var ifAdmin = $("input[name='ifAdmin']:checked").val();
    if ( username == "" || pwd == "" || nickname == "" || regtime == "" || icon == "" || ifAdmin == "") {
        alert("用户表单信息不完整！");
        return false;
    }
}