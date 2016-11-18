$(document).ready(function () {    
    $("#replyBox").click(function () {
        $("#replyText").toggleClass("hidden");
        $("#replyText textarea").focus();
    });
    $(".UserDataRow").click(function () {
        
    });
    //$(".PictureModalLink").click(function () {
    //    $("#PictureModal").modal("toggle");
    //});
    
});
function PopUpModal() {    
    $("#PictureModal").modal("show");
};
function UpdateLikeAndCommentOnMainPage(target) {
    $("#btnLike" + target).val($("#btnLikeInModal").val());
    $("#anchorComment" + target).text($("#anchorCommentInModal").text());
    $("#anchorNumberOfLikes" + target).text($("#anchorNumberOfLikes").text());
};