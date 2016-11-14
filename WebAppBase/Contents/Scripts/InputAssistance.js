$(document).ready(function () {
    $(".Input-Assistance").each(function () {
        if (this.value.length == 0) {
            this.value = "キーワードで検索";
            $(this).addClass("text-non-focus");
        }
    });

    $(".Input-Assistance").focus(function () {
        $(this).removeClass("text-non-focus");
        if (this.value == 'キーワードで検索') {
            this.value = '';
        }
    });
    $(".Input-Assistance").blur(function () {
        if (this.value == '') {
            this.value = 'キーワードで検索';
            $(this).addClass("text-non-focus");
        }
    });
});

function clearTransparentString() {
    $(".Input-Assistance").each(function () {
        if (this.value == 'キーワードで検索') {
            this.value = '';
        }
    });
    return true;
}