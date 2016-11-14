$(function () {

    /*画像ロールオーバー*****************************************
    *********************************************************/
    var images = $("img");
    for (var i = 0; i < images.size(); i++) {
        if (images.eq(i).attr("src").match("_off.")) {
            $("img").eq(i).hover(function () {
                $(this).attr('src', $(this).attr("src").replace("_off.", "_on."));
            }, function () {
                $(this).attr('src', $(this).attr("src").replace("_on.", "_off."));
            });
        }
    }

    /*ページ上部への移動*****************************************
    *********************************************************/
    $('#pagetop').click(function () {
        $('html,body').animate({ scrollTop: 0 }, 'fast');
        return false;
    });

    /* ボタンの初期化 */
    //$("input:button, input:submit,a[type='button']").button();
    $.fn.SetOption = function (optionType, optionProperty, optionValue) {
        var btn = $(this);
        if (optionType == "option" && optionProperty == "disabled")
        {
            if (optionValue == true) {
                btn.prop("disabled", "disabled");
            }
            else {
                btn.removeProp("disabled");
            }
        }
    };
    $("input:button, input:submit,a[type='button']").each(function () {
        var btn = $(this);
        btn.addClass("btn");
        var txt = btn.val();
        if (txt == "登録" || txt == "新規登録")
        {
            btn.addClass("btn-primary");
        }
        else if(txt == "削除")
        {
            btn.addClass("btn-danger");
        }
        else {
            btn.addClass("btn-default");
        }

    });

    $("a[type='button']").css("width", 54);


    $('.tabs').tabs();
});
/**************************************************
* [機能]	日付オブジェクトから文字列に変換します
* [引数]	date	対象の日付オブジェクト
* 			format	フォーマット
* [戻値]	フォーマット後の文字列
**************************************************/
function comDateFormat(date, format) {

    var result = format;

    var f;
    var rep;

    var yobi = new Array('日', '月', '火', '水', '木', '金', '土');

    f = 'yyyy';
    if (result.indexOf(f) > -1) {
        rep = date.getFullYear();
        result = result.replace(/yyyy/, rep);
    }

    f = 'MM';
    if (result.indexOf(f) > -1) {
        rep = comPadZero(date.getMonth() + 1, 2);
        result = result.replace(/MM/, rep);
    }

    f = 'ddd';
    if (result.indexOf(f) > -1) {
        rep = yobi[date.getDay()];
        result = result.replace(/ddd/, rep);
    }

    f = 'dd';
    if (result.indexOf(f) > -1) {
        rep = comPadZero(date.getDate(), 2);
        result = result.replace(/dd/, rep);
    }

    f = 'HH';
    if (result.indexOf(f) > -1) {
        rep = comPadZero(date.getHours(), 2);
        result = result.replace(/HH/, rep);
    }

    f = 'mm';
    if (result.indexOf(f) > -1) {
        rep = comPadZero(date.getMinutes(), 2);
        result = result.replace(/mm/, rep);
    }

    f = 'ss';
    if (result.indexOf(f) > -1) {
        rep = comPadZero(date.getSeconds(), 2);
        result = result.replace(/ss/, rep);
    }

    f = 'fff';
    if (result.indexOf(f) > -1) {
        rep = comPadZero(date.getMilliseconds(), 3);
        result = result.replace(/fff/, rep);
    }

    return result;

}

/**************************************************
* [機能]	文字列から日付オブジェクトに変換します
* [引数]	date	日付を表す文字列
* 			format	フォーマット
* [戻値]	変換後の日付オブジェクト
**************************************************/
function comDateParse(date, format) {

    var year = 1990;
    var month = 01;
    var day = 01;
    var hour = 00;
    var minute = 00;
    var second = 00;
    var millisecond = 000;

    var f;
    var idx;

    f = 'yyyy';
    idx = format.indexOf(f);
    if (idx > -1) {
        year = date.substr(idx, f.length);
    }

    f = 'MM';
    idx = format.indexOf(f);
    if (idx > -1) {
        month = parseInt(date.substr(idx, f.length), 10) - 1;
    }

    f = 'dd';
    idx = format.indexOf(f);
    if (idx > -1) {
        day = date.substr(idx, f.length);
    }

    f = 'HH';
    idx = format.indexOf(f);
    if (idx > -1) {
        hour = date.substr(idx, f.length);
    }

    f = 'mm';
    idx = format.indexOf(f);
    if (idx > -1) {
        minute = date.substr(idx, f.length);
    }

    f = 'ss';
    idx = format.indexOf(f);
    if (idx > -1) {
        second = date.substr(idx, f.length);
    }

    f = 'fff';
    idx = format.indexOf(f);
    if (idx > -1) {
        millisecond = date.substr(idx, f.length);
    }

    var result = new Date(year, month, day, hour, minute, second, millisecond);

    return result;

}

/**************************************************
* [機能]	ゼロパディングを行います
* [引数]	value	対象の文字列
* 			length	長さ
* [戻値]	結果文字列
**************************************************/
function comPadZero(value, length) {
    return new Array(length - ('' + value).length + 1).join('0') + value;
}

jQuery(function () {
    jQuery('.datepicker').datepicker({
        showButtonPanel: true
    });
});

$(function() {
    $(".NumText").css("ime-mode", "inactive");
    $(".NumABC").css("ime-mode", "inactive");
    $(".Fullwidth").css("ime-mode", "active");
    $(".NumPhone").css("ime-mode", "inactive");
    $(".NumUrl").css("ime-mode", "inactive");
    $(".NumEmail").css("ime-mode", "inactive");
    $(".NumZipCode").css("ime-mode", "inactive");

    $(".NumText").blur(function() {
        Chk_keyNumericAndPoint(this);
    });

    $(".NumPhone").blur(function() {
        ckeckNumber(this);
    });

    $(".NumUrl").blur(function() {
        checkUrl(this);
    });

    $(".NumZipCode").blur(function() {
        ckeckZipCode(this);
    });

    $(".NumEmail").blur(function() {
        checkEmail(this);
    });

    $(".NumABC").blur(function() {
        ckeckEnglish(this);
    });

    $(".NumText,.NumABC,.Fullwidth,.NumEmail,.NumUrl,.NumPhone,.NumZipCode,.CheckCompCode").focus(function() {
        $(this).select();
    });

});

//日本語を入力してはいけない
function ckeckEnglish(txtObj) {
    var reg=/^[^\u0391-\uFFE5]+$/; 
    return EngLishReg(reg, txtObj);
}

//郵便番号検証
function ckeckZipCode(txtObj) {
    var patt1 = new RegExp(/[\d,-]+/);
    return processReg(patt1, txtObj);
}

//電話番号の検証
function ckeckNumber(txtObj) {
    var patt1 = new RegExp(/[(,),\d,-]+/);
    return processReg(patt1, txtObj);
}

//Urlの検証
function checkUrl(txtObj) {
   var patt1 = new RegExp(/((http|ftp|https):\/\/)?[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&;/~\+#])?/);
    return processReg(patt1, txtObj);
}

//Emailの検証
function checkEmail(txtObj) {
    var patt1 = new RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/);
    return processReg(patt1, txtObj);
}

//数字の検証
function Chk_keyNumericAndPoint(txtObj) {
    var patt1 = new RegExp(/^(-?\d+)(\.\d+)?/);
    return processReg(patt1, txtObj);
}

function EngLishReg(patt1,txtObj) {
    var value=$(txtObj).val();
    if(value=='') return;
    var result=value.replace(/[\u0391-\uFFE5]/g,'');
        $(txtObj).val(result);
}

function processReg(patt1, txtObj) {

    var value = $(txtObj).val();
    if (value == '') return;
    var result = patt1.test(value);
    if (result == false) {
        $(txtObj).focus();
        $(txtObj).val('');
    }
    else {
        var res = patt1.exec(value);
        if (res.length > 0) {
            $(txtObj).val(res[0]);
        }
    }
    return;
}

//レポートの検索条件のドロップダウンリストの高さを生簀ユーザコントロールの高さにセットします
function reportSearchComboboxHeight() {
    $("select").wijcombobox({
        isEditable: false
    });
    $(".wijmo-wijcombobox-input").css("height", "22px");
    $(".wijmo-wijcombobox-trigger").css("height", "27px");
}
