/**
For infinte page loading on scroll in galleries index view.
*/
var pageNumber = 1;

$(window).scroll(function () {
    //When scrolled to bottom of page
    if ($(window).scrollTop() == $(document).height() - $(window).height()) {
        pageNumber++;
        $.get("/Galleries?page=" + pageNumber, function (data) {
            if (data === '') {
                $(window).unbind("scroll");
            }
            $("#galleriesDiv").hide().appendTo('#galleriesDiv').fadeIn();
            $("#galleriesDiv").append(data);
            $(window).scrollTop($(window).scrollTop() - 1);
        });
    }
});