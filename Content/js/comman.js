/* 评论管理 */
/* AddComment */
// 添加按钮实现
document.getElementById('addButtonComment').addEventListener('click', function () {
    window.location.href = '/Add/AddComment'; // 设置目标 URL
});
// 清空评论添加表单
function clearFormAddComment() {
    document.getElementById("comID").value = "";
    document.getElementById("username").value = "";
    document.getElementById("content").value = "";
    document.getElementById("comtime").value = "";
    document.getElementById("tvid").value = "";
    // 如果是单选框，根据实际情况设置选中状态
    document.getElementById("ifStatus1").checked = false;
    document.getElementById("ifStatus0").checked = false;
}
// 提交评论时检查是否有空信息
function checkFormAddComment() {
    var comID = $("#comID").val();
    var username = $("#username").val();
    var content = $("#content").val();
    var comtime = $("#comtime").val();
    var tvid = $("#tvid").val();
    var ifStatus = $("input[name='ifStatus']:checked").val();
    if (comID == "" || username == "" || content == "" || comtime == "" || tvid == "" || ifStatus == "") {
        alert("评论表单信息不完整！");
        return false;
    }
    if (isNaN(parseInt(comID)) || isNaN(parseInt(videoID)) || isNaN(parseInt(like))) {
        alert("请输入有效的评论ID、唯一标识！");
        return false;
    }
}

/* BackStage/DeleteComment */
function confirmDeleteComment(commentId) {
    var isConfirmed = window.confirm('确认删除评论 #' + commentId + ' 吗？');
    return isConfirmed;
}

/* EditComment */
// 清空评论表单
function clearFormEditComment() {
    document.getElementById("username").value = "";
    document.getElementById("content").value = "";
    document.getElementById("comtime").value = "";
    document.getElementById("tvid").value = "";
    // 如果是单选框，根据实际情况设置选中状态
    document.getElementById("ifStatus1").checked = false;
    document.getElementById("ifStatus0").checked = false;
}
// 提交时检查评论表单是否有空信息
function checkFormEditComment() {
    var username = $("#username").val();
    var content = $("#content").val();
    var comtime = $("#comtime").val();
    var tvid = $("#tvid").val();
    var ifStatus = $("input[name='ifStatus']:checked").val();
    if (username == "" || content == "" || comtime == "" || tvid == "" || ifStatus == "" ) {
        alert("评论表单信息不完整！");
        return false;
    }
    if (isNaN(parseInt(videoID)) || isNaN(parseInt(like))) {
        alert("请输入有效的唯一标识！");
        return false;
    }
}
