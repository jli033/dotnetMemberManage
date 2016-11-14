$(function () {
    $("#dialog-form").dialog({
        autoOpen: false,
        resizable: false,
        height: 400,
        width: 500,
        modal: true,
        buttons: {
            "登録": function () {
                // グリッド内のデータを配列に取り込む
                var items = getSelectableItems();

                // サーバーにデータベース登録の要求を出す。
                try {
                    $.ajax({
                        type: "POST",
                        url: OrderListPageUrl,
                        async: false,
                        data: {
                            Items: items
                        },
                        datatype: "text"
                    });
                }
                catch (e) {
                    alert(e);
                }
                // フォームを閉じる。
                $(this).dialog("close");
                return true;
            }
        },
        close: function () {
            // サブフォームを閉じる際、リロードを行う。
            //            location.reload();
            location = location;
            return true;
        }
    });

    $("#OrderChangeLink")
    .click(function () {
        // dialogを開く
        $("#dialog-form").dialog("open");
    });
    $("#upOrder")
    .button()
    .click(function () {
        MoveUpOrDown(true);
    });
    $("#downOrder")
    .button()
    .click(function () {
        MoveUpOrDown(false);
    });
});



//Moves the options up or down using the buttons on the right hand side of the Selected List
function MoveUpOrDown(isUp) {

    var listbox = $("select[name='OrderList']");
    if ($(listbox).find("option:selected").text().length == 0) {
        alert("移動させる項目を選択してください。");
    }
    else if ($(listbox).find("option:selected").length > 1) {
        alert("You can only select one option to move Up or Down.");
    }
    else {
        //Get the values
        var val = $(listbox).find("option:selected").val();
        var text = $(listbox).find("option:selected").text();
        var index = $(listbox).find("option:selected").index();

        //Get the length now
        var length = $(listbox).find("option").length;

        //Move it up or down
        if (isUp == true) {
            //Move the option a row above
            index = (index == 0 ? index : index - 1);
            //Insert the options
            $("<option value='" + val + "'>" + text + "</option>").insertBefore($(listbox).find("option:eq(" + index + ")"));
        }
        else // Down
        {
            //Move the option a row down
            index = (index == length - 1 ? index : index + 1);

            $("<option value='" + val + "'>" + text + "</option>").insertAfter($(listbox).find("option:eq(" + index + ")"));
        }

        //Remove the options
        $(listbox).find("option:selected").remove();
        //Set the value as selected
        $(listbox).val(val);
    }
}

// 順序変更 防汚剤名称の一覧を取得する
function getSelectableItems() {
    var ret = "";
    var option;

    // 全要素を取得
    for (var i = 0; i < $('#OrderList').children().length; i++) {
        option = $('#OrderList option:eq(' + i + ')');
        ret = ret + option.val() + "-DelimiterOfKeyValue-" + option.text() + "-DelimiterOfLine-";
    }
    return ret;
}
