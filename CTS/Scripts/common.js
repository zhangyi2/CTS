/*
* String function
*/

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

String.prototype.trimStart = function () {
    return this.replace(/(^\s*)/g, "");
}

String.prototype.trimEnd = function () {
    return this.replace(/(\s*$)/g, "");
}

String.prototype.startsWith = function (str) {
    return (this.match("^" + str) == str)
}

String.prototype.endsWith = function (str) {
    return (this.match(str + "$") == str)
}

String.prototype.Format = function () {
    var returnValue = this;
    for (var i = 0; i < arguments.length; i++) {
        var arg = arguments[i];
        var key = "";
        var value = "";
        if (typeof arg == "string") {
            key = i;
            value = arg;
        } else if (arg instanceof Array) {
            returnValue = returnValue.Format.apply(returnValue, arg);
            continue;
        } else {
            key = arg.key;
            value = arg.value;
        }

        var keySymple = [
           { reg: /\\/ig, value: "\\\\" },
           { reg: /\{/ig, value: "\\{" },
           { reg: /\}/ig, value: "\\}" },
           { reg: /\)/ig, value: "\\)" },
           { reg: /\(/ig, value: "\\(" },
        ]
        for (var j = 0; j < keySymple.length; j++) {
            var sympleReplace = keySymple[j];
            key = key.toString().replace(sympleReplace.reg, sympleReplace.value);
        }
        var reg = new RegExp("\\{" + key + "\\}", "ig");
        returnValue = returnValue.replace(reg, value);
    }
    return returnValue;
}

function hashcode(str) {
    var hash = 0, i, chr, len;
    if (str.length === 0) return hash;
    for (i = 0, len = str.length; i < len; i++) {
        chr = str.charCodeAt(i);
        hash = ((hash << 5) - hash) + chr;
        hash |= 0; // Convert to 32bit integer
    }
    return hash;
}


/*
* Date function
*/

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "H+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

/*
* document view attribution function
*/

jQuery.fn.limit = function () {
    var self = $("span[limit]");
    self.each(function () {
        var objString = jQuery.trim($(this).text());
        var objLength = objString.length;
        var num = $(this).attr("limit");
        if (objLength > num) {
           $(this).attr("title", objString);
            objString = $(this).text(objString.substring(0, num) + "...");
        }
    })
}


/*
* List Sort 
*/

//compareOrder=[{valueBy:func/propName (, direction:"desc")}],compareOrder为空时直接比较对象本身（可能会报异常），默认升序排列
function _sort(items, compareOrder, newReturn, direction) {
    if (!$.isArray(items) && !items.length) return items;
    if (newReturn == true) {//返回新的数组对象，不改变原有items的顺序，但数组内实际对象仍为引用
        var newItems = [];
        $.each(items, function (item) {
            newItems.push(item);
        });
        items = newItems;
    }
    var d = direction == "desc" ? -1 : 1;//若为desc，则表示根据compareOrder排序后再倒序
    for (var i = 1; i < items.length; i++) {
        for (var j = 0; j <= i - 1; j++) {
            if (_compare(items[i], items[j], compareOrder) * d < 0) {
                var temp = items[i];
                items.splice(i, 1);
                items.splice(j, 0, temp);
                break;
            }
        }
    }
    return items;
}
function _compare(e1, e2, compareOrder) {
    if (compareOrder) {
        if (!$.isArray(compareOrder)) {
            compareOrder = [compareOrder];
        }
        for (var i = 0; i < compareOrder.length; i++) {
            var valueBy = compareOrder[i].valueBy || compareOrder[i];
            var d = compareOrder[i].direction == "desc" ? -1 : 1;
            var v1, v2;
            if ($.isFunction(valueBy)) {
                v1 = valueBy(e1);
                v2 = valueBy(e2);
            }
            else {
                v1 = e1[valueBy];
                v2 = e2[valueBy];
            }
            var compareRtn = _compareValue(v1, v2);
            if (compareRtn != 0) return compareRtn * d;
        }
        return 0;
    }
    else {
        return _compareValue(e1, e2);
    }
}
function _compareValue(v1, v2) {
    if (v1 < v2) return -1;
    else if (v1 == v2) return 0;
    else return 1;
}

//options:{labelBy, valueBy, titleBy, orderBy, categaryBy, autoMerge, defaultToOthers, subHiddenCategary}
function _getItems(source, options) {
    var items = [];
    var categary = [];
    var opt = options;
    if ($.isArray(source)) {
        var merge = function (item, checkList) {//合并相同label|value的item，取后者
            var isConflict = false;
            if (opt.autoMerge) {
                for (var j = 0; j < checkList.length; j++) {
                    if (checkList[j].label == item.label || checkList[j].value == item.value) {
                        checkList[j] = item;
                        isConflict = true;
                        break;
                    }
                }
            }
            if (!isConflict) checkList.push(item);
        };
        $.each(source, function (index, data) {
            if ($.isPlainObject(data)) {
                if ($.isFunction(options.washBy)) options.washBy(data);//为兼容老版本，预处理数据

                var lab = $.isFunction(opt.labelBy) ? opt.labelBy(data) : data[opt.labelBy];
                var val = $.isFunction(opt.valueBy) ? opt.valueBy(data) : data[opt.valueBy];
                if (!lab && val) lab = val;
                if (lab && !val) val = lab;
                var item = { label: lab, value: val, raw: data };
                var tit = $.isFunction(opt.titleBy) ? opt.titleBy(data) : data[opt.titleBy];
                if (tit) $.extend(item, { title: tit });

                if (opt.categaryBy) {
                    var cat = $.isFunction(opt.categaryBy) ? opt.categaryBy(data) : data[opt.categaryBy];
                    if (!cat && opt.defaultToOthers) cat = "Others";
                    if (cat) {
                        if (opt.subHiddenCategary) {
                            item.label = item.label.substr(item.label.indexOf(cat) + cat.length);
                            if (!item.label) item.label = "[root]";
                        }
                        var hasCategary = false;
                        for (var idx = 0; idx < categary.length; idx++) {
                            if (categary[idx].label == cat) {
                                merge(item, categary[idx].subItems);//合并相同label的item
                                hasCategary = true;
                                break;
                            }
                        }
                        if (!hasCategary)
                            categary.push({ label: cat, value: cat, subItems: [item] });
                    }
                    else {
                        merge(item, items);
                    }
                }
                else merge(item, items);
            }
            else if(data){
                merge({ label: data.toString(), value: data.toString() });
            }
        });
    }
    return categary.length > 0 ? $.extend(categary, items) : items;
}
function _showItems(ul, items, position, emptyMark, selectedFunc, refreshFunc) {
    if (body_status[ul.attr("data-name") + "-refresh"]) {
        ul.html("");
        if (items.length == 0 && emptyMark) {
            ul.append("<li class=\"ui-autocomplete-category\">" + emptyMark + "</li>");
        }
        var showItem = function (item) {
            var name = " item-name='" + (item.name || item.value) + "'";
            var title = item.title ? (" title='" + item.title + "'") : "";
            return $("<li></li>").append($("<a" + name + title + ">" + item.label + "</a>"))
            .click(function () {
                if ($.isFunction(selectedFunc)) selectedFunc($(this), item);
                ul.css("display", "none").find("a.hover").removeClass(".hover");
                body_status[ul.attr("data-name") + "-display"] = "none";
            });
        }
        $.each(items, function (index, item) {
            var li;
            if (item.subItems) {
                var name = " item-name='" + (item.name || item.value) + "'";
                var title = item.title ? (" title='" + item.title + "'") : "";
                li = $("<li></li>").append($("<a" + name + title + ">" + item.label + "</a>"));

                var subItems = item.subItems;
                var subUl = $("<ul class=\"dropdown-menu\"></ul>").appendTo(li);
                $.each(subItems, function (idx, sub) {
                    showItem(sub).appendTo(subUl);
                });
            }
            else {
                li = showItem(item);
            }
            ul.append(li);
        });
        if ($.isFunction(refreshFunc)) refreshFunc();
        body_status[ul.attr("data-name") + "-refresh"] = false;
    }
    ul.find("li a.hover").removeClass("hover");
    if(ul.children().length > 0) _dropdownMenuPosition(ul, position).css("display", "");
    setTimeout(function () {
        body_status[ul.attr("data-name") + "-display"] = "";
    }, 300);
}
/*
*   根据direction（默认asc）遍历，后者若与前者冲突则覆盖前者；
*   在equalOrder同值条件下，compareOrder排序后，后者覆盖前者（默认取值大的覆盖值小的）
*/
function _mergeItems(list, direction, equalOrder, compareOrder) {
    var checkList = [];
    var isConflict = false;
    var merge = function (item, checkList, equalOrder, compareOrder) {
        if (!equalOrder) return;
        for (var j = 0; j < checkList.length; j++) {
            if (_compare(checkList[j], item, equalOrder) == 0) {
                if (compareOrder && _compare(checkList[j], item, compareOrder) < 0) {
                    checkList[j] = item;
                }
                return true;
            }
        }
        return false;
    };
    if (direction == "desc") {
        for (var i = 0; i < list.length; i++) {
            isConflict = merge(list[i], checkList, equalOrder, compareOrder);
            if (!isConflict) checkList.push(list[i]);
        }
    }
    else {
        for (var i = list.length - 1; i >= 0; i--) {
            isConflict = merge(list[i], checkList, equalOrder, compareOrder);
            if (!isConflict) checkList.splice(0, 0, list[i]);
        }
    }
    return checkList;
}
function _getItem(list, matchFunc) {
    if (!matchFunc) return null;
    for (var i = 0; i < list.length; i++) {//若冲突取第一个
        if(matchFunc (list[i])) {
            return list[i];
        }
    }
    return null;
}
function _getItemInfo(list, matchFunc, returnFunc) {
    if (!matchFunc) return null;
    for (var i = 0; i < list.length; i++) {//若冲突取第一个
        if (matchFunc(list[i])) {
            return returnFunc ? returnFunc(i, list[i]) : list[i];
        }
    }
    return null;
}
function _exists(list, matchFunc) {
    if (!matchFunc) return false;
    for (var i = 0; i < list.length; i++) {
        if (matchFunc(list[i])) {
            return true;
        }
    }
    return false;
}


/*
* common event
*/
function _enterSelect(selectorId) {
    if ($("#" + selectorId).is(":visible")) {
        var li_a = $("#" + selectorId + " li a:hover");
        if (li_a.length == 0) li_a = $("#" + selectorId + " li a.hover");
        if (li_a.length > 0) {
            li_a.click();
            return true;
        }
    }
    return false;
}
function _keydownSelect(inputId, selectorId, showFunc) {
    if ($("#" + inputId).is(":focus")) {
        if ($("#" + selectorId).is(":visible")) {
            if ($("#" + selectorId + " li a:hover").length > 0) return true;
            var li_a = $("#" + selectorId + " li a.hover");
            var newli = [];
            if (li_a.length > 0) {
                newli = li_a.parent(0).next();
                li_a.removeClass("hover");
            }
            else newli = $("#" + selectorId + " li:first");
            if (newli.length > 0) newli.children("a").addClass("hover");
        }
        else {
            if ($.isFunction(showFunc)) showFunc();
        }
        return true;
    }
    return false;
}
function _keyupSelect(inputId, selectorId, showFunc) {
    if ($("#" + inputId).is(":focus")) {
        if ($("#" + selectorId).is(":visible")) {
            if ($("#" + selectorId + " li a:hover").length > 0) return true;
            var li_a = $("#" + selectorId + " li a.hover");
            var newli = [];
            if (li_a.length > 0) {
                newli = li_a.parent(0).prev();
                li_a.removeClass("hover");
            }
            else newli = $("#" + selectorId + " li:last");
            if (newli.length > 0) newli.children("a").addClass("hover");
        }
        else {
            if ($.isFunction(showFunc)) showFunc();
        }
        return true;
    }
    return false;
}
function _dropdownMenuPosition(ul, position) {
    var relation = position == "relative" ? ul.prev() : $(ul.attr("data-relation"));
    var mode = ul.attr("data-position-mode");
    if (relation && relation.length == 1) {
        var position = relation.position();
        if (mode == "right-bottom") {
            ul.css("right", position.right);
            ul.css("top", position.top + relation.height() + 1);
        }
        else if (mode == "right-top") {
            ul.css("right", position.right);
            ul.css("bottom", position.top - 1);
        }
        else if (mode == "left-top") {
            ul.css("left", position.left);
            ul.css("bottom", position.top - 1);
        }
        else {//left-bottom
            ul.css("left", position.left);
            ul.css("top", position.top + relation.height() + 1);
        }
    }
    return ul;
}
//select all-partial
function _selectAll() {
    var content = $(this).parentsUntil(".content-wrapper", ".select-all-content");
    var ul = content.find(".select-partial-panel ul");
    if ($(this).is(":checked")) ul.find("input[type=checkbox]:not(:checked)").click();//.attr("checked", true);
    else ul.find("input[type=checkbox]:checked").click();//.attr("checked", false);
}
function _selectPartial(elem, selectFunc) {
    var content = $(elem).parentsUntil(".content-wrapper", ".select-all-content");
    var ul = content.find(".select-partial-panel ul");
    var all_select = content.find(".select-all-panel input[type=checkbox]");
    var selected = $(elem).is(":checked");
    if (selected) {
        if (ul.find("input[type=checkbox]:not(:checked)").length == 0) all_select.attr("checked", true);
    }
    else if (all_select.is(":checked")) {
        all_select.attr("checked", false);
    }
    if ($.isFunction(selectFunc)) selectFunc(selected);
}
function _selectPartialLabel() {
    $(this).prev(0).click();
}
//modal
function ShowModal(id, initFunc, beforeCloseFunc, afterCloseFunc, inFocus, outFocus) {
    var modal = $("#" + id);
    if (!modal.length) return;

    if ($.isFunction(initFunc)) initFunc();

    $("div.modal-backdrop.fade.in").css("display", "block");
    $("div.modal-scrollable").append(modal).css("display", "block");
    modal.css("display", "block");
    if (modal.outerHeight() > $(window).innerHeight()) {
        modal.css("margin-top", "0px").addClass("modal-overflow");
    }
    else modal.css("margin-top", "-" + (modal.height() / 2) + "px");
    modal.addClass("in");

    $("div.modal-scrollable a.close").unbind("click").click(function () {
        if ($.isFunction(beforeCloseFunc)) beforeCloseFunc();
        modal.appendTo("#modal-container").removeClass("modal-overflow").removeClass("in").css("display", "none")
            .css("margin-top", "0px");
        $("div.modal-scrollable").css("display", "none");
        $("div.modal-backdrop.fade.in").css("display", "none");
        if ($.isFunction(afterCloseFunc)) afterCloseFunc();

        body_status["modal-show"] = false;
        if (outFocus instanceof jQuery) outFocus.focus();
        else $("#toolbar a[href='#" + id + "']").focus();
    });
    body_status["modal-show"] = true;
    if (inFocus instanceof jQuery) inFocus.focus();
    else {
        var input = modal.find(":input:first");
        if (input.length > 0) input.focus();
        else {
            var alink = modal.find("a.btn-primary:not(.disabled):first");
            if (alink.length > 0) alink.focus();
            else modal.focus();
        }
    }
}
function ModalAlert(title, content, mode, callbackFunc) {//mode=[trueBtnText(-falseBtnText)], 例:"OK-Cancel","OK";"-"隐藏按钮
    $("#modal-alert-title").html(title);
    $("#modal-alert-content").html(content);
    if (!mode) mode = "OK";
    var modeText = mode.split("-");
    if (modeText[0]) {
        $("#alert-ok").text(modeText[0]).unbind("click").click(function () {
            CloseModal();
            if ($.isFunction(callbackFunc)) callbackFunc(true);
        });
    }
    else $("#alert-ok").css("display", "none");
    if (modeText.length > 1 && modeText[1]) {
        $("#alert-cancel").text(modeText[1]).css("display", "").unbind("click").click(function () {
            CloseModal();
            if ($.isFunction(callbackFunc)) callbackFunc(false);
        });
    }
    else $("#alert-cancel").css("display", "none");
    ShowModal("modal-alert");
}
function CloseModal() {
    var modalDiv = $(".modal-scrollable div[id^='modal-']:visible");
    if (modalDiv.length > 0) modalDiv.find("a.close").click();
}



/*
* http 
*/

//Html编码获取Html转义实体
String.prototype.htmlEncode = function () {
    return $('<div/>').text(this.toString()).html();
}
//Html解码获取Html实体
String.prototype.htmlDecode = function () {
    return $('<div/>').html(this.toString()).text();
}

//[protocol]://[host]:[port][path]?[queryString]
function MatchUrl(url) {
    //获取protocol
    if (url.indexOf("://") < 0) url = "http://" + url;
    var e1 = url.match(/^(?:(https?|ftp|mms|mailto|ssh):\/\/)(.*)$/);
    var defaultPort = "80";
    if (e1[1] == "ftp") {
        defaultPort = "443";
    }
    var urlData = { "protocol": e1[1], "url": e1[2] };

    //获取query string
    var idx = e1[2].indexOf("?"), path, queryString = "";
    if (idx > 0) {
        path = e1[2].substr(0, idx);
        queryString = e1[2].substr(idx + 1);
    }
    else {
        path = e1[2];
    }
    $.extend(urlData, { "queryString": queryString });

    //获取host & port
    var e2;
    idx = path.indexOf("/")
    if (idx < 0) {
        e2 = path.split(":");
        $.extend(urlData, { "path": "" });
    }
    else {
        e2 = path.substr(0, idx).split(":");
        $.extend(urlData, { "path": path.substr(idx) });
    }
    $.extend(urlData, { "host": e2[0] });
    $.extend(urlData, { "port": e2.length > 1 ? e2[1] : defaultPort });

    return urlData;
}

var DataStatusCode = {
    Success: 0,
    Abort: -1,
    Failed: -2,
    Error: -3,
    Timeout: -9,
    ValidationException: -10,
    DalException: -20,
    BizException: -30,
    UnkownException: -99
};

function HttpRequest(req, beforeSendFunc, successFunc, errorFunc) {
    if (!req || !req.info || (!req.info.requestUri && !req.info.method)) return null;
    var request = {
        type: "POST",
        url: "./Handler/PostService.asmx/HttpRequest",
        data: { "request": JSON.stringify(req) },
        dataType: 'text',
        //async: false,
        cache: false,
        beforeSend: function(xhr){
            if ($.isFunction(beforeSendFunc)) beforeSendFunc(xhr);
        },
        complete: function (xhr, textStatus) {
            if (xhr.status == 200) {
                var d = $.parseJSON(xhr.responseText);
                if (d.status.statusCode == 200) {
                    if ($.isFunction(successFunc)) successFunc(d);
                }
                else {
                    if ($.isFunction(errorFunc)) errorFunc(d);
                }
            }
            else {
                if ($.isFunction(errorFunc)) errorFunc(xhr);
            }
        }
    };
    if (req.options && req.options.timeout > 0)
        $.extend(request, { timeout: req.options.timeout });
    $.ajax(request);
}
function RunTestCase(req, bindIps, beforeSendFunc, successFunc, errorFunc) {
    if (!req || !req.info || (!req.info.requestUri && !req.info.method)) return null;
    var request = {
        type: "POST",
        url: "./Handler/TestManager.asmx/RunTestCase",
        data: { "request": JSON.stringify(req), "bindIps": JSON.stringify(bindIps) },
        dataType: 'text',
        //async: false,
        cache: false,
        beforeSend: function (xhr) {
            if ($.isFunction(beforeSendFunc)) beforeSendFunc(xhr);
        },
        complete: function (xhr, textStatus) {
            if (xhr.status == 200) {
                var d = $.parseJSON(xhr.responseText);
                if (d.status.statusCode == 200) {
                    if ($.isFunction(successFunc)) successFunc(d);
                }
                else {
                    if ($.isFunction(errorFunc)) errorFunc(d);
                }
            }
            else {
                if ($.isFunction(errorFunc)) errorFunc(xhr);
            }
        }
    };
    if (req.options && req.options.timeout > 0)
        $.extend(request, { timeout: req.options.timeout });
    $.ajax(request);
}
function SendTestCaseByStep(testInfo, testBatchInfo, options) {
    var reqInfo = testInfo.reqInfo;
    if (!(reqInfo && reqInfo.requestUri && reqInfo.method)) return null;

    //var testInfo = { "testCaseId": testCaseId, "reqInfo": reqInfo, "reqOptions": reqOptions, "testIp": testIp };
    var batchInfo = $.extend({
        batchId: "",
        stepId: testInfo.testIp,
        stepCount: 0,
        finishCount: 0,
    }, testBatchInfo);
    if (testInfo.reqOptions && testInfo.reqOptions.timeout > 0)
        $.extend(batchInfo, { timeout: testInfo.reqOptions.timeout });

    if (batchInfo.finishCount == 0 && $.isFunction(options.initFunc)) options.initFunc();
    SendByStep({
        sendUrl: "./Handler/TestManager.asmx/RunTestCaseByStep", 
        batchInfo: batchInfo,
        stepInfo: testInfo,
        isFinished: false,
        beforeSendFunc: options.beforeSendFunc,
        stepNextFunc: options.stepNextFunc,
        stepSuccessFunc: options.stepSuccessFunc,
        stepErrorFunc: options.stepErrorFunc,
        finalSuccessFunc: options.finalSuccessFunc,
        finalErrorFunc: options.finalErrorFunc
    });
}
function MC_Count(mcName, tags, setValue) {
    SendData({
        "url": "./Metrics/MetricsService.asmx/MC_Count",
        "data": { "mcName": mcName, "tagsJson": JSON.stringify(tags), "setValue": setValue || 1 }
    });
};

function SendData(options, beforeSendFunc, successFunc, errorFunc) {
    if (!options || !options.url) return null;
    var request = {
        type: "POST",
        url: options.url,
        data: options.data,
        dataType: 'text',
        //async: false,
        cache: false,
        beforeSend: function (xhr) {
            if ($.isFunction(beforeSendFunc)) beforeSendFunc(xhr);
        },
        complete: function (xhr, textStatus) {
            if (xhr.status == 200) {
                var d = $.parseJSON(xhr.responseText);
                if (d.status.statusCode == 0) {
                    if ($.isFunction(successFunc)) successFunc(d);
                }
                else {
                    if ($.isFunction(errorFunc)) errorFunc(d);
                }
            }
            else {
                if ($.isFunction(errorFunc)) errorFunc(xhr);
            }
        }
    };
    $.ajax(request);
}
function SendByStep(options) {
    if (!options.sendUrl || !options.stepInfo) return;
    var data = {
        "batchInfo": options.batchInfo,
        "stepInfo": options.stepInfo
    };
    var request = {
        type: "POST",
        url: options.sendUrl,
        data: { "data": JSON.stringify(data), "isFinished": options.isFinished || false },
        dataType: 'text',
        //async: false,
        cache: false,
        beforeSend: function (xhr) {
            if ($.isFunction(options.beforeSendFunc)) options.beforeSendFunc(xhr);
        },
        complete: function (xhr, textStatus) {
            if (xhr.status == 200) {
                var stepDone = function (stepResult, afterFunc) {
                    if (stepResult.stepStatus.statusCode == 0) {
                        if ($.isFunction(options.stepSuccessFunc)) options.stepSuccessFunc(stepResult);
                        if ($.isFunction(options.stepNextFunc)) options.stepNextFunc(stepResult);
                    }
                    else {
                        if ($.isFunction(options.stepErrorFunc)) options.stepErrorFunc(stepResult);
                        if ($.isFunction(options.stepNextFunc)) options.stepNextFunc(stepResult);
                    }
                    if ($.isFunction(afterFunc)) afterFunc();
                };
                var finalDone = function (finalResult) {
                    if (finalResult.finalStatus.statusCode == 0) {
                        if ($.isFunction(options.finalSuccessFunc)) options.finalSuccessFunc(finalResult);
                    }
                    else {
                        if ($.isFunction(options.finalErrorFunc)) options.finalErrorFunc(finalResult);
                    }
                };

                var d = $.parseJSON(xhr.responseText);
                if (d.finalStatus) {//批处理结束
                    if (d.lastStepResult) {
                        stepDone(d.lastStepResult, function () {
                            finalDone(d);
                        });
                    }
                    else finalDone(d);
                }
                else {
                    stepDone(d);
                }
            }
            else {
                result = {
                    batchInfo: {
                        stepCount: data.batchInfo.stepCount
                    },
                    stepStatus: {
                        statusCode: DataStatusCode.Failed,
                        statusDescription: xhr.statusText + "(" + xhr.status + ")"// + "<br/>" + xhr.responseText
                    }
                };
                if ($.isFunction(options.stepErrorFunc)) options.stepErrorFunc(result);
                if ($.isFunction(options.stepNextFunc)) options.stepNextFunc(result);
                if ((options.isFinished == true || result.batchInfo.finishCount >= result.batchInfo.stepCount) && $.isFunction(options.finalErrorFunc)) {
                    options.finalErrorFunc({
                        finalStatus: DataStatusCode.Failed,
                    });
                }
            }
        }
    };
    $.ajax(request);
}

//Cookie function
function addCookie(name,value,expiresHours){
    var cookieString=name+"="+escape(value);
    //判断是否设置过期时间
    if(expiresHours>0){
        var date=new Date();
        date.setTime(date.getTime+expiresHours*3600*1000);
        cookieString=cookieString+"; expires="+date.toGMTString();
    }
    document.cookie=cookieString;
}
function getCookie(name){
    var strCookie=document.cookie;
    var arrCookie=strCookie.split("; ");
    for(var i=0;i<arrCookie.length;i++){
        var arr=arrCookie[i].split("=");
        if (arr[0] == name) return unescape(arr[1]);
    }
    return "";
}
function deleteCookie(name) {
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    document.cookie = name + "=v; expires=" + date.toGMTString();
}


/*
* File function
*/
function GetFullName(path, name) {
    if (!path || !name) return null;
    if (path[path.length - 1] != "\\") path += "\\";
    name.replace("\\", "");
    return path + name;
}
function DownloadFile(filename, content) {
    var $inputFileName = $('<input>').attr({ name: "fileName", value: filename });
    var $inputContent = $('<input>').attr({ name: "content", value: content });
    var $form = $("<form>");
    $form.attr({ target: '_self', method: 'post', action: './Handler/PostService.asmx/Download' }).append($inputFileName).append($inputContent);
    $form.submit();
}