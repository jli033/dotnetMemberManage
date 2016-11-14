
/*
*FishSpeciesSelectMenu
*
*usage:
*       var b = $('#XXX').fishSpeciesSelectMenu();
*       b.setParam(fishingGroundGuid, fishKindGuid);

*/

(function ($, window, document, undefined) {

    var FishSpeciesSelectMenu = function (ele, opt) {
        this.$element = ele;
        this.options = $.extend({}, opt)
    }

    FishSpeciesSelectMenu.prototype = {
        setParam: function (fishingGroundGuid, fishKindGuid) {
            //debugger;
            var $thisObj = this;
            this._setFishingGroundGuid(fishingGroundGuid);
            this._setFishKindGuid(fishKindGuid);
            var id = this.getId();
            var name = this.getName();
            var event = this.getFishSpeciesSelectedEvent();
            var organizationID = this.getOrganizationID();            
            var url = this.getUrl();//'@Url.Action("FishSpeciesSelectMenuList", "UserControlPartial")';
            var menuID = this.getMenuID();
            var hideAll = this.getHideAll();
            var autoSelectFirst = this.getAutoSelectFirst();
            var isReadOnly = this.getIsReadOnly();
            var sendData = {
                ID: id,
                Name: name,
                SelectedValue: null,
                FishSpeciesSelectedEvent: event,
                OrganizationID: organizationID,
                FishingGroundGuid: fishingGroundGuid,
                FishKindGuid: fishKindGuid,
                HideAll: hideAll,
                AutoSelectFirst: autoSelectFirst,
                IsReadOnly: isReadOnly
            }

            $.post(url, sendData, function (data, status, jqXHR) {
                //debugger;
                $("#" + menuID).wijmenu("destroy");
                $("#" + menuID).detach();
                var div = $thisObj.$element.next();
                div.empty();
                div.append(data);
            }, "html");

        },
        setReadOnly: function (status)
        {
            debugger;
            var menuID = this.getMenuID();
            var textID = this.getTextID();
            if (status) {
                $("#" + menuID).wijmenu("option", "trigger", "");
            }
            else {
                $("#" + menuID).wijmenu("option", "trigger", "#" + textID);
            }
        },
        getId: function () {
            return this.$element.attr("id");
        },
        getName: function () {
            return this.$element.attr("name");
        },
        getFishKindGuid: function () {
            return $(this.$element.prev().children()[2]).val();
        },
        _setFishKindGuid: function (v) {
            return $(this.$element.prev().children()[2]).val(v);
        },
        getFishingGroundGuid: function () {
            return $(this.$element.prev().children()[1]).val();
        },
        _setFishingGroundGuid: function (v) {
            return $(this.$element.prev().children()[1]).val(v);
        },
        getFishSpeciesSelectedEvent: function () {
            return $(this.$element.prev().children()[0]).val();
        },
        getOrganizationID: function () {
            return $(this.$element.prev().children()[3]).val();
        },
        getUrl: function () {
            return $(this.$element.prev().children()[4]).val();
        },
        getMenuID: function () {
            return $(this.$element.prev().children()[5]).val();
        },
        getHideAll: function () {
            return $(this.$element.prev().children()[6]).val();
        },
        getAutoSelectFirst: function () {
            return $(this.$element.prev().children()[7]).val();
        },
        getTextID: function () {
            return $(this.$element.prev().children()[8]).val();
        },
        getIsReadOnly: function () {
            return $(this.$element.prev().children()[9]).val();
        }
    };


    //var fishSpeciesSelectMenu = null;
    //use instance in jquery plugin
    $.fn.fishSpeciesSelectMenu = function (options) {
        //debugger;
        //create instance
        var fishSpeciesSelectMenu = new FishSpeciesSelectMenu(this, options);
        return fishSpeciesSelectMenu;
    }

})(jQuery, window, document);
