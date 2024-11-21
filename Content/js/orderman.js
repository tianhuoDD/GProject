// 确认删除用户
function confirmDeleteOrder(id) {
    var isConfirmed = window.confirm('确认删除订单 #' + id + ' 吗？');
    return isConfirmed;
}