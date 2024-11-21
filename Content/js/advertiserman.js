/* 广告管理 */
// 添加按钮实现
document.getElementById('addButtonAdvertiser').addEventListener('click', function () {
    window.location.href = '/Add/AddAdvertiser'; // 设置目标 URL
});
// 清空用户添加表单
function clearFormAddAdvertiser() {
    document.getElementById("id").value = "";
    document.getElementById("username").value = "";
    document.getElementById("inAdName").value = "";
    document.getElementById("balance").value = "";
    document.getElementById("inAdContent").value = "";
    document.getElementById("startDate").value = "";
    document.getElementById("endDate").value = "";
    // 如果是单选框，根据实际情况设置选中状态
    document.getElementById("adStatus0").checked = false;
    document.getElementById("adStatus1").checked = false;
    document.getElementById("adStatus2").checked = false;
    document.getElementById("adStatus3").checked = false;
    document.getElementById("cost").value = "";
    document.getElementById("payStatus0").checked = false;
    document.getElementById("payStatus1").checked = false;
}
// 提交用户时检查是否有空信息
function checkFormAddAdvertiser() {
    var id = $("#id").val();
    var username = $("#username").val();
    var inAdName = $("#inAdName").val();
    var balance = $("#balance").val();
    var inAdContent = $("#inAdContent").val();
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();
    var adStatus = $("input[name='adStatus']:checked").val();
    var cost = $("#cost").val();
    var payStatus = $("input[name='payStatus']:checked").val();
    if (username == "" || inAdName == "" || balance==""|| inAdContent == "" || startDate == "" || endDate == "" || adStatus == "" || cost == "" || payStatus == "") {
        alert("用户表单信息不完整！");
        return false;
    }
    if (isNaN(parseInt(id))) {
        alert("请输入有效的用户ID！");
        return false;
    }
}

// 确认删除用户
function confirmDeleteAdvertiser(id) {
    var isConfirmed = window.confirm('确认删除广告商 #' + id + ' 吗？');
    return isConfirmed;
}

/* EditAdvertiser */
// 清空评论表单
function clearFormEditAdvertiser() {
    document.getElementById("username").value = "";
    document.getElementById("inAdName").value = "";
    document.getElementById("balance").value = "";
    document.getElementById("inAdContent").value = "";
    document.getElementById("startDate").value = "";
    document.getElementById("endDate").value = "";
    // 如果是单选框，根据实际情况设置选中状态
    document.getElementById("adStatus0").checked = false;
    document.getElementById("adStatus1").checked = false;
    document.getElementById("adStatus2").checked = false;
    document.getElementById("adStatus3").checked = false;
    document.getElementById("cost").value = "";
    document.getElementById("payStatus0").checked = false;
    document.getElementById("payStatus1").checked = false;
}
// 提交时检查评论表单是否有空信息
function checkFormEditAdvertiser() {
    var username = $("#username").val();
    var inAdName = $("#inAdName").val();
    var balance = $("#balance").val();
    var inAdContent = $("#inAdContent").val();
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();
    var adStatus = $("input[name='adStatus']:checked").val();
    var cost = $("#cost").val();
    var payStatus = $("input[name='payStatus']:checked").val();
    if (username == "" || inAdName == "" || balance=="" || inAdContent == "" || startDate == "" || endDate == "" || adStatus == "" || cost == "" || payStatus == "" ) {
        alert("用户表单信息不完整！");
        return false;
    }
}