/* 视频管理 */
/* AddVideo */
// 添加视频按钮实现
document.getElementById('addButtonVideo').addEventListener('click', function () {
    window.location.href = '/Add/AddVideo'; // 设置目标 URL
});
// 清空视频添加表单
function clearFormAddVideo() {
    document.getElementById("videoID").value = "";
    document.getElementById("inTitle").value = "";
    document.getElementById("inLink").value = "";
    document.getElementById("inCover").value = "";
    document.getElementById("inTag").value = "";
    document.getElementById("inYear").value = "";
    document.getElementById("inRating").value = "";
    document.getElementById("inStatus").value = "";
    document.getElementById("inHotScore").value = "";
    document.getElementById("inType").value = "";
    document.getElementById("intvID").value = "";
    document.getElementById("inDesc").value = "";
    document.getElementById("inBigCover").value = "";
    document.getElementById("inDate").value = "";
    document.getElementById("inTime").value = "";
    document.getElementById("inDirector").value = "";
    document.getElementById("inActor").value = "";
}
// 提交视频添加表单时检查是否有空信息
function checkFormAddVideo() {
    var videoID = $("#videoID").val();
    var inTitle = $("#inTitle").val();
    var inLink = $("#inLink").val();
    var inCover = $("#inCover").val();
    var inTag = $("#inTag").val();
    var inYear = $("#inYear").val();
    var inRating = $("#inRating").val();
    var inStatus = $("#inStatus").val();
    var inHotScore = $("#inHotScore").val();
    var inType = $("#inType").val();
    var intvID = $("#intvID").val();
    var inDesc = $("#inDesc").val();
    var inBigCover = $("#inBigCover").val();
    var inDate = $("#inDate").val();
    var inTime = $("#inTime").val();
    var inDirector = $("#inDirector").val();
    var inActor = $("#inActor").val();

    if (videoID == "" || inTitle == "" || inLink == "" || inCover == "" || inTag == ""
        || inYear == "" || inRating == "" || inStatus == "" || inHotScore == "" || inType == ""
        || intvID == "" || inDesc == "" || inBigCover == "" || inDate == "" || inTime == ""
        || inDirector == "" || inActor == "" ) {
        alert("视频信息不完整！");
        return false;
    }
    if (isNaN(parseInt(videoID))) {
        alert("请输入有效的视频ID！");
        return false;
    }
    if (isNaN(parseInt(inHotScore))) {
        alert("请输入有效的视频热度！");
        return false;
    }
    if (isNaN(parseInt(inTime))) {
        alert("请输入有效的视频时长！");
        return false;
    }
    if (/^\s|\s$|\s{2,}/.test(intvID)) {
        alert("唯一标识不能包含空格！");
        return false;
    }
    // 使用正则表达式验证inRating是否是浮点数，最多一位小数，并且总长度为3位
    var ratingPattern = /^(?:\d(\.\d)?|\.\d|10)$/;
    if (!ratingPattern.test(inRating)) {
        alert("评分最高是10，且最多一位小数！");
        return false;
    }

}

/* BackStage/DeleteVideo */
function confirmDeleteVideo(videoID) {
    var isConfirmed = window.confirm('确认删除视频 #' + videoID + ' 吗？');
    return isConfirmed;
}

/* EditVideo */
//清空输入框
function clearFormEditVideo() {
    document.getElementById("inTitle").value = "";
    document.getElementById("inLink").value = "";
    document.getElementById("inCover").value = "";
    document.getElementById("inTag").value = "";
    document.getElementById("inYear").value = "";
    document.getElementById("inRating").value = "";
    document.getElementById("inStatus").value = "";
    document.getElementById("inHotScore").value = "";
    document.getElementById("inType").value = "";
    document.getElementById("intvID").value = "";
    document.getElementById("inDesc").value = "";
    document.getElementById("inBigCover").value = "";
    document.getElementById("inDate").value = "";
    document.getElementById("inTime").value = "";
    document.getElementById("inDirector").value = "";
    document.getElementById("inActor").value = "";
}
// 检查提交的表单是否有空
function checkFormEditVideo() {
    var inTitle = $("#inTitle").val();
    var inLink = $("#inLink").val();
    var inCover = $("#inCover").val();
    var inTag = $("#inTag").val();
    var inYear = $("#inYear").val();
    var inRating = $("#inRating").val();
    var inStatus = $("#inStatus").val();
    var inHotScore = $("#inHotScore").val();
    var inType = $("#inType").val();
    var intvID = $("#intvID").val();
    var inDesc = $("#inDesc").val();
    var inBigCover = $("#inBigCover").val();
    var inDate = $("#inDate").val();
    var inTime = $("#inTime").val();
    var inDirector = $("#inDirector").val();
    var inActor = $("#inActor").val();

    if (inTitle == "" || inLink == "" || inCover == "" || inTag == ""
        || inYear == "" || inRating == "" || inStatus == "" || inHotScore == "" || inType == ""
        || intvID == "" || inDesc == "" || inBigCover == "" || inDate == "" || inTime == ""
        || inDirector == "" || inActor == "") {
        alert("视频信息不完整！");
        return false;
    }
    if (isNaN(parseInt(inHotScore))) {
        alert("请输入有效的视频热度！");
        return false;
    // 使用正则表达式验证inRating是否是浮点数，最多一位小数，并且总长度为3位
    var ratingPattern = /^(?:\d(\.\d)?|\.\d|10)$/;
    if (!ratingPattern.test(inRating)) {
        alert("评分最高是10，且最多一位小数！");
        return false;
    }

    }
    if (isNaN(parseInt(inTime))) {
        alert("请输入有效的视频时间！");
        return false;
    }
    if (/^\s|\s$|\s{2,}/.test(intvID)) {
        alert("唯一标识不能包含空格！");
        return false;
    }
    // 使用正则表达式验证inRating是否是浮点数，最多一位小数，并且总长度为3位
    var ratingPattern = /^(?:\d(\.\d)?|\.\d|10)$/;
    if (!ratingPattern.test(inRating)) {
        alert("评分最高是10，且最多一位小数！");
        return false;
    }


}
