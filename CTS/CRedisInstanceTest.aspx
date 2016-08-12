<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRedisInstanceTest.aspx.cs" Inherits="ctrip.Framework.ApplicationFx.CTS.CRedisInstanceTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CTS- www.ctripcorp.com</title>
    <link href="<%=GetIncludeUrl("Content/themes/base/jquery-ui-1.10.4.css")%>" rel="stylesheet" />
    <link href="<%=GetIncludeUrl("Content/Site.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=GetIncludeUrl("Content/common.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=GetIncludeUrl("Content/test.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=GetIncludeUrl("Content/bootstrap-modal.css")%>" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.10.4.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-migrate-git.min.js" type="text/javascript"></script>

    <script src="<%=GetIncludeUrl("Scripts/common.js")%>" type="text/javascript"></script>
    <script src="<%=GetIncludeUrl("Scripts/loadconfig.js")%>" type="text/javascript"></script>
    <script src="<%=GetIncludeUrl("Scripts/cachehandler.js")%>" type="text/javascript"></script>
</head>
<body>
    <div id="main" class="content">
        <section id="instance" style="display: block;">
            <div class="content-wrapper">
                    <div class="tab-info-line clearfix">
                        <div class="info-container tab-container">
                            <div class="name-wrapper tab-wrapper">
                                <div class="name-container">
                                    <span>Redis Instance List</span>
                                </div>
                            </div>
                        </div>
                        <div class="actions-container">
                            <label data-name="auto-clear"><input type="checkbox" checked/><span>自动清空状态</span></label>
                            <button class="btn btn-primary" data-name="run-test">
                                Test
                            </button>
                        </div>
                    </div>
                    <div class="filter-labelinfo">
                        <div class="select-all-content clearfix">
                            <div class="select-all-panel ind label-data">
                                <span class="label">filter<input type="checkbox" title="Select all" data-name="cluster-select-all-label" />
                                </span>
                            </div>
                            <div class="select-partial-panel">
                                <ul data-name="label-list">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="filter-header clearfix">
                        <div class="filter-title">
                            <span>Cluster</span>
                            （<span class="label">c<span data-name="cluster-count"></span></span> -
                            <span class="label">g<span data-name="group-count"></span></span> -
                            <span class="label">i<span data-name="instance-count"></span></span>）
                        </div>
                        <div class="filter-content-wrapper">
                            <div class="filter-content">
                                <ul data-name="idc-list-header">
                                </ul>
                            </div>
                        </div>
                        <div class="rim"></div>
                    </div>
                    <div class="test-content-list clearfix">
                        <div class="filter-line">
                            <ul class="content-wrapper ui-sortable">
                            </ul>
                        </div>
                    </div>
            </div>
        </section>
        <section id="hidden_data" style="display: none;">
            <div id="username"><%= this.CurrentUserName %></div>
            <div id="userinfo"><%= this.CurrentUserInfo %></div>
            <div id="test-panel-container-templet">
                <div class="test-panel-container-wrapper">
                    <div class="info-container push-left">
                        <div class="name-wrapper">
                            <div class="name-container w000 no-margin">
                                <span data-name="item-name" title="Click to edit">Cluster Name</span>
                            </div>
                        </div>
                    </div>
                    <div class="list-container">
                        <ul data-name="idc-list-content">
                        </ul>
                    </div>
                </div>
            </div>
            <ul id="idc-list-header-templet">
                <li data-env="SHAFQ"><div><span>FQ-福泉</span>（<span class="label">g<span data-name="group-count"></span></span> - <span class="label">i<span data-name="instance-count"></span></span>）</div></li>
            </ul>
            <ul id="idc-list-content-templet">
                <li data-env="SHAFQ"><div class="group-container"></div></li>
            </ul>
            <div id="group-container-templet">
                <div class="i-group" data-group-id="0"><i data-server-id="0" class="master" title="10.8.187.75:6381"></i><i data-server-id="0" title="10.8.187.75:6381"></i></div>
            </div>
        </section>
        <section id="modal-container" style="display: block;">
            <div id="modal-alert" class="modal fade" tabindex="-1" style="margin-top: 0px; display: none;">
                <div class="modal-header">
                    <a href="#" class="close" data-dismiss="modal">×</a>
                    <h3 id="modal-alert-title">Alert</h3>
                </div>
                <div class="modal-body">
                    <div class="control-group">
                        <label id="modal-alert-content" class="control-label"></label>
                    </div>
                </div>
                <div class="modal-footer">
                    <a href="#" id="alert-ok" class="btn btn-primary">OK</a>
                    <a href="#" id="alert-cancel" class="btn btn-secondary">Cancel</a>
                </div>
            </div>
            <div class="modal-scrollable" style="z-index: 1050; display: none;"></div>
            <div class="modal-backdrop fade in" style="z-index: 1040; display: none;"></div>
        </section>
        <section id="script">
            <%-- 常量、参数定义 --%>
            <script type="text/javascript">
                var DEFAULT_CONTENT_TYPE = "application/x-www-form-urlencoded;charset=utf-8";
                var loadConfig = $.loadConfig({
                    curUser: $.extend({ Name: "Unknown", Department: "Unknown" }, JSON.parse($("#userinfo").text() || "{}"))
                });
                var cacheHandler = $.cacheHandler({
                });
                var data_Productline = [], data_Cluster = [];
                var index_IDC = {};//{env:{name: "", "clusterCount":0, "groupCount":0, "insCount":0, "clusters": {}, "groups": {}, "servers": {}}}
                var index_IDC_filter = {};
                var stat_Count = { "clusterCount": 0, "groupCount": 0, "insCount": 0, "invalidClusterCount": 0 };
                var stat_Count_filter = {};
                //var data_Instance = {};
                var index_ClusterLabel = {};
                var filterConditions = [];
                var isFilterProductLine = false;
                var body_status = {};

                var ul_label = $("#instance .filter-labelinfo>.select-all-content>.select-partial-panel>ul[data-name=\"label-list\"]");
                var all_label = $("#instance .filter-labelinfo>.select-all-content>.select-all-panel input[type='checkbox']");
                var envHeader = $("#instance .filter-header ul[data-name='idc-list-header']");
                var testContainer = $("#instance .test-content-list>.filter-line>ul.content-wrapper");

            </script>
            <%-- page event --%>
            <script type="text/javascript">
                $(document).ready(function () {
                    LoadConfig();
                    BindEvent();
                    AfterRender();
                })
                function BindEvent() {
                    $("#instance .tab-info-line button[data-name=\"run-test\"]").click(RunTest);
                }
                function LoadConfig() {
                    var InitIDC = function () {
                        envHeader.html("");
                        var envContainer = $("#test-panel-container-templet .list-container>ul").html("");
                        var idcCount = 0;
                        for (var idc in index_IDC) {
                            idcCount++;
                            var name = index_IDC[idc].name;
                            $.extend(index_IDC[idc], { "groupCount": 0, "insCount": 0, "groups": {}, "servers": {} });
                            index_IDC_filter[idc] = { "groupCount": 0, "insCount": 0, "groups": {}, "servers": {} };

                            envHeader.append($("<li></li>").attr("data-env", idc)
                                .click(function () {
                                    if (body_status["test-running"] == true) return false;

                                    var elem = $(this);
                                    var idc = elem.attr("data-env");
                                    if (testContainer.find("li.test-panel-container:visible li[data-env=\"" + idc + "\"] div.i-group").length == 0) return false;

                                    _resetSelected("env");
                                    if (elem.is(".active")) {
                                        elem.removeClass("active");
                                    }
                                    else elem.addClass("active");
                                    _autoSelected("env", elem);
                                })
                                .append($("<div></div>")
                                    .html("<span>" + name + "</span>（<span class=\"label\">g<span data-name=\"group-count\"></span></span> - <span class=\"label\">i<span data-name=\"instance-count\"></span></span>）")
                                ));
                            envContainer.append($("<li></li>").attr("data-env", idc).append($("<div></div>").addClass("group-container")));
                        }
                        var width = idcCount > 0 ? (100/idcCount).toFixed(2) : 100;
                        envHeader.children().css("width", width + "%");
                        envContainer.children().css("width", width + "%");
                    };

                    loadConfig.getIDCIndex(function (data) {
                        index_IDC = data || {};
                        InitIDC();
                    }, function (errorMessage) {
                        ModalAlert("Error", errorMessage, "OK");
                    });
                    loadConfig.getProductlineList(function (data) {
                        data_Productline = data || [];
                    }, function (errorMessage) {
                        ModalAlert("Error", errorMessage, "OK");
                    });
                    loadConfig.getClusterList(null, function (data) {
                        data_Cluster = data || [];
                        InitCluster();
                        filterConditions = [];
                        isFilterProductLine = false;
                        BindLabels();
                    }, function (errorMessage) {
                        ModalAlert("Error", errorMessage, "OK");
                    });
                }
                function _updateStatInfo(isFilter) {
                    var idx_IDC = isFilter == true ? index_IDC_filter : index_IDC;
                    var stat = isFilter == true ? stat_Count_filter : stat_Count;

                    var envHeader = $("#instance .filter-header ul[data-name='idc-list-header']>li");
                    for (var i = 0; i < envHeader.length; i++) {
                        var li = $(envHeader[i]);
                        var env = li.attr("data-env");
                        li.find("span[data-name=\"group-count\"]").text(idx_IDC[env]["groupCount"]);
                        li.find("span[data-name=\"instance-count\"]").text(idx_IDC[env]["insCount"]);
                    }

                    var filterTitle = $("#instance .filter-header .filter-title");
                    filterTitle.find("span[data-name=\"cluster-count\"]").text(stat["clusterCount"]);
                    filterTitle.find("span[data-name=\"group-count\"]").text(stat["groupCount"]);
                    filterTitle.find("span[data-name=\"instance-count\"]").text(stat["insCount"]);
                }
                function _resetSelected(eventType, elem) { //env, cluster, cluster-env
                    if (elem != undefined && $.isXMLDoc(elem)) elem = $(elem);

                    var reset = function () {
                        testContainer.children("li.active").removeClass("active");
                        envHeader.children("li.active").removeClass("active");
                        testContainer.find("li[data-env].active").removeClass("active");
                    };

                    if(eventType == "env"){
                        if (testContainer.children("li.active").length > 0)
                            reset();
                    }
                    else if (eventType == "cluster") {
                        if(envHeader.children("li.active").length > 0)
                            reset();
                    }
                    else if (eventType == "cluster-env"){
                        if (elem && elem.is(".active")) {
                            var idc = elem.attr("data-env");
                            envHeader.children("li[data-env=\"" + idc + "\"].active").removeClass("active");
                            elem.parentsUntil("ul.content-wrapper", "li.test-panel-container.active").removeClass("active");
                        }
                    }
                    else reset();
                }
                function _autoSelected(eventType, elem) {//elem对应li(.active)
                    if (elem != undefined && $.isXMLDoc(elem)) elem = $(elem);

                    var selected = elem.is(".active");
                    var selectElems = [];
                    if (eventType == "env") {
                        var idc = elem.attr("data-env");
                        selectElems = testContainer.find("li.test-panel-container:visible li[data-env=\"" + idc + "\"]:has(div.i-group)");
                    }
                    else if (eventType == "cluster") {
                        selectElems = elem.find("li[data-env]:has(div.i-group)");
                    }
                    if (selected) {
                        selectElems.addClass("active");
                    }
                    else {
                        selectElems.removeClass("active");
                    }
                }
                function BindLabels() {
                    ul_label.html("");
                    all_label.click(_selectAll);

                    var filterStat = function () {
                        var filterStatByEnv = function (env) {
                            var grpElems = testContainer.find("li.test-panel-container:visible ul[data-name=\"idc-list-content\"]>li[data-env=\"" + env + "\"] div.i-group");
                            $.extend(index_IDC_filter[env], {
                                "groupCount": 0,
                                "insCount": 0,
                                "groups": {},
                                "servers": {}
                            });
                            for (var j = 0; j < grpElems.length; j++) {
                                var grpElem = $(grpElems[j]);
                                var groupId = grpElem.attr("data-group-id");
                                if (!index_IDC_filter[env]["groups"][groupId]) {
                                    index_IDC_filter[env]["groups"][groupId] = 1;
                                    index_IDC_filter[env]["groupCount"] += 1;

                                    var insElems = grpElem.children();
                                    for (var i = 0; i < insElems.length; i++) {
                                        var elem = $(insElems[i]);
                                        var serverId = elem.attr("data-server-id");
                                        var instance = elem.attr("data-instance");
                                        if (!index_IDC_filter[env]["servers"][serverId]) {
                                            index_IDC_filter[env]["servers"][serverId] = instance;
                                            index_IDC_filter[env]["insCount"] += 1;
                                        }
                                    }
                                }
                            }
                        }

                        stat_Count_filter["clusterCount"] = testContainer.find("li.test-panel-container:visible").length;
                        stat_Count_filter["groupCount"] = 0;
                        stat_Count_filter["insCount"] = 0;
                        for (var idc in index_IDC) {
                            filterStatByEnv(idc);
                            stat_Count_filter["groupCount"] += index_IDC_filter[idc]["groupCount"];
                            stat_Count_filter["insCount"] += index_IDC_filter[idc]["insCount"];
                        }

                        _updateStatInfo(true);
                    };

                    for (var k in index_ClusterLabel) {
                        var text = index_ClusterLabel[k].name + "[C" + index_ClusterLabel[k].clusterCount + "]";
                        var css = index_ClusterLabel[k].clusterCount > 10 ? " class=\"highlight\"" : "";
                        $("<li></li>").attr("data-label", k).appendTo(ul_label)
                            .append($("<input type=\"checkbox\" name=\"label-select\"/>").val(k).click(function () {
                                var elem = $(this);
                                var pid = elem.val();
                                _selectPartial(this, function (selected) {
                                    if (selected) {
                                        if (isFilterProductLine == true) {
                                            $("li.test-panel-container[data-label='" + pid + "']").css("display", "");
                                            filterStat();
                                        }
                                        else {
                                            $("li.test-panel-container[data-label!='" + pid + "']").css("display", "none");
                                            isFilterProductLine = true;
                                            filterStat();
                                        }
                                    }
                                    else {
                                        if (ul_label.find("li[data-label!='" + pid + "']").find("input[type='checkbox']:checked").length == 0) {
                                            $("li.test-panel-container").css("display", "");
                                            isFilterProductLine = false;
                                            _updateStatInfo(false);
                                        }
                                        else {
                                            $("li.test-panel-container[data-label='" + pid + "']").css("display", "none");
                                            filterStat();
                                        }
                                    }
                                });
                                _resetSelected();
                            }))
                            .append($("<span" + css + "></span>").text(text).click(_selectPartialLabel));
                    }
                }
                function InitCluster() {
                    testContainer.html("");//.css("display", "none");
                    index_ClusterLabel = {};

                    var genTitleLine = function (key, val, mapType) {
                        if (mapType == "boolean") {
                            val = val ? "true" : "false";
                        }
                        else if (mapType == "status") {
                            val = val ? "有效" : "无效";
                        }
                        return key + ": " + val + "\r\n";
                    };

                    var bindCluster = function (clst) {
                        if (!index_ClusterLabel[clst.pid]) {
                            index_ClusterLabel[clst.pid] = _getItemInfo(data_Productline, function (item) {
                                return item.id == clst.pid;
                            }, function (i, item) {
                                return { name: item.name, clusterCount: 0 };
                            });
                        }
                        index_ClusterLabel[clst.pid].clusterCount += 1;

                        cacheHandler.credis_getCluster(clst.name, null, function (data) {
                            var usingIDC = data["UsingIDC"];

                            cacheHandler.credis_renderCluster(data, {
                                "getOrRenderCluster": function (data, parentContainer) {
                                    var container = parentContainer || testContainer;
                                    var id = data["ID"];
                                    if (id == 0) {
                                        stat_Count["invalidClusterCount"] += 1;
                                        console.log("cluster-id=" + id + ", name=" + clst.name);
                                        return null;
                                    }
                                    var clstContainer = container.find("[data-cluster-id='" + id + "']");
                                    if (clstContainer.length == 0) {
                                        clstContainer = $("<li class=\"test-panel-container no-padding\"></li>")
                                            .attr("data-cluster-id", id)
                                            .attr("data-label", clst.pid)
                                            .appendTo(container);
                                        if (data["Status"] == 0) clstContainer.addClass("disable");
                                        clstContainer.html($("#test-panel-container-templet").html());

                                        stat_Count["clusterCount"] += 1;

                                        clstContainer.find(".info-container .name-container")
                                            .click(function () {
                                                if (body_status["test-running"] == true) return false;

                                                var container = clstContainer;
                                                _resetSelected("cluster");
                                                if (container.is(".active")) {
                                                    container.removeClass("active");
                                                }
                                                else container.addClass("active");
                                                _autoSelected("cluster", container);
                                            });
                                        clstContainer.find(".info-container .name-container>span[data-name=\"item-name\"]").text(data["Name"])
                                            .click(function () {
                                                if (body_status["test-running"] == true) return false;
                                                //ShowClusterEdit(false);
                                            });
                                        clstContainer.find("li[data-env]").click(function () {
                                            if (body_status["test-running"] == true) return false;

                                            var elem = $(this);
                                            var idc = elem.attr("data-env");
                                            if (elem.find("div.i-group").length == 0) return false;

                                            _resetSelected("cluster-env", elem);
                                            if (elem.is(".active")) {
                                                elem.removeClass("active");
                                            }
                                            else elem.addClass("active");
                                        });

                                        //设置属性标签
                                        if (usingIDC) {
                                            clstContainer.find(".info-container .name-container").append("<span class=\"label-light pull-right\">IDC</span>");
                                        }
                                    }
                                    return clstContainer;
                                },
                                "getOrRenderGroup": function (data, parentContainer) {
                                    var container = parentContainer || testContainer;
                                    var id = data["ID"];
                                    if (id == 0) {
                                        console.log("group-id=" + id);
                                        return null;
                                    }
                                    var cid = data["ClusterID"];

                                    var env = data["Env"] || "NONE";
                                    var envContainer = container.find(".list-container>ul");
                                    var grpContainer = envContainer.find("li[data-env='" + env + "']>.group-container");

                                    var grpDiv = grpContainer.find(".i-group[data-group-id='" + id + "']");
                                    if (grpDiv.length == 0) {
                                        grpDiv = $("<div class=\"i-group\"></div>").attr("data-group-id", id).appendTo(grpContainer);

                                        if (!index_IDC[env]["groups"][id]) {
                                            index_IDC[env]["groups"][id] = 1;//只用来索引，1无意义
                                            index_IDC[env]["groupCount"] += 1;
                                            stat_Count["groupCount"] += 1;
                                        }
                                    }
                                    return grpDiv;
                                },
                                "getOrRenderInstance": function (data, clstContainer) {
                                    var container = clstContainer || testContainer;
                                    var id = data["ID"];
                                    if (id == 0) {
                                        console.log("server-id=" + id);
                                        return null;
                                    }
                                    var gid = data["GroupID"];

                                    var grpDiv = container.find(".i-group[data-group-id='" + gid + "']");
                                    var env = grpDiv.parentsUntil("ul", "li[data-env]").attr("data-env");

                                    var isMaster = data["ParentID"] == 0;
                                    var ins = grpDiv.find("[data-server-id='" + id + "']");
                                    if (ins.length == 0) {
                                        ins = $("<i></i>").attr("data-server-id", id).attr("data-instance", data["IP"] + ":" + data["Port"]);
                                        if (isMaster) ins.addClass("master").prependTo(grpDiv);
                                        else ins.appendTo(grpDiv);
                                        //ins.attr("title", data["IP"] + ":" + data["Port"]);

                                        if (!index_IDC[env]["servers"][id]) {
                                            index_IDC[env]["servers"][id] = data["IP"] + ":" + data["Port"];
                                            index_IDC[env]["insCount"] += 1;
                                            stat_Count["insCount"] += 1;
                                        }
                                    }
                                    return ins;
                                },
                                "viewClusterTitle": function (info, clstContainer) {
                                    var elem = clstContainer.find(".info-container .name-container>span[data-name=\"item-name\"]");
                                    var title = genTitleLine("ID", info["ID"]);
                                    title += genTitleLine("Name", info["Name"]);
                                    title += genTitleLine("UsingIDC", info["UsingIDC"], "boolean");
                                    title += genTitleLine("Status", info["Status"], "status");
                                    elem.attr("title", title);
                                },
                                //"viewGroupTitle": function (info, groupDiv) {
                                //    var elem = groupDiv;
                                //    var title = genTitleLine("ID", info["ID"]);
                                //    elem.attr("title", title);
                                //},
                                "viewInstanceTitle": function (info, insElem) {
                                    var elem = insElem;
                                    var title = genTitleLine("ID", info["ID"]);
                                    title += genTitleLine("IP", info["IP"]);
                                    title += genTitleLine("Port", info["Port"]);
                                    title += genTitleLine("DBNumber", info["DBNumber"]);
                                    title += genTitleLine("CanRead", info["CanRead"], "boolean");
                                    //title += genTitleLine("Status", info["Status"], "status");
                                    elem.attr("title", title);
                                },
                                "filterByLabels": function (labels) {

                                }
                            }, function (data) {
                                //render所有cluster之后
                                if ((stat_Count["clusterCount"] + stat_Count["invalidClusterCount"]) >= data_Cluster.length) {
                                    //cluster排序
                                    if (data_Cluster.length > 1) {
                                        var list = _sort(testContainer.children(), [{
                                            valueBy: function (elem) {
                                                return $(elem).find(".info-container .name-container>span[data-name=\"item-name\"]").text().toLowerCase();//order by cluster name
                                            }
                                        }], false, "asc");
                                        $.each(list, function (idx, elem) {
                                            $(elem).insertAfter(testContainer.children().last());
                                        });
                                    }
                                    //testContainer.css("display", "");

                                    //显示统计值
                                    _updateStatInfo(false);
                                }
                            });//credis_renderCluster successFunc
                        }, function (data) {//credis_getCluster errorFunc
                        });
                    };

                    $.each(data_Cluster, function (idx, clst) {
                        bindCluster(clst);
                    });
                }
                function RunTest() {
                    var getGroupList = function () {
                        var index_test = {};
                        var testList = [];

                        var selected = testContainer.find("li.test-panel-container:visible li[data-env].active .i-group");
                        for (var j = 0; j < selected.length; j++) {
                            var grpElem = $(selected[j]);
                            var groupId = grpElem.attr("data-group-id");
                            if (!index_test[groupId]) {
                                var insList = grpElem.children();
                                var instanceList = [];
                                for (var i = 0; i < insList.length; i++) {
                                    var insElem = $(insList[i]);
                                    var serverId = insElem.attr("data-server-id");
                                    var ins = insElem.attr("data-instance");
                                    var isMaster = insElem.is(".master");
                                    var ip_port = ins.split(':');
                                    instanceList.push({ "ip": ip_port[0], "port": ip_port[1], "isMaster": isMaster });
                                }
                                index_test[groupId] = { "groupId": groupId, "instanceList": instanceList };
                            }
                        }

                        for (var k in index_test) {
                            testList.push(index_test[k]);
                        }
                        return testList;
                    };
                    var _clearTestResult = function(selectedElems, splitTitle) {
                        if (!selectedElems) {
                            selectedElems = testContainer.find("li.test-panel-container:visible li[data-env]");
                        }
                        var testElems = selectedElems.find(".i-group>i.test-status")
                            .removeClass("success").removeClass("failed").removeClass("warning").removeClass("test-status")
                            .removeAttr("data-result");
                        for (var i = 0; i < testElems.length; i++) {
                            var elem = $(testElems[i]);
                            var title = elem.attr("title").split(splitTitle)[0];
                            elem.attr("title", title);
                        }
                    };

                    var list = getGroupList();
                    var testStack = 0, unitCount = 4, longTime = 200;//设置每4个instance批量处理，responseTime最长200ms则提示
                    body_status["test-running"] = false;
                    var batchResult = { successCount: 0, failedCount: 0, longTimeCount: 0, errorMessage: "", timeMessage: "" };
                    var statusCss = { "success": "success", "failed": "failed", "longTime": "warning" };
                    var titleHeader = "【Test Result】\r\n";

                    var lockRunning = function () {
                        $("#instance .tab-info-line .btn[data-name='run-test']").addClass("running").attr("disabled", "disabled");
                        $("#instance .filter-labelinfo input[type=\"checkbox\"]").attr("disabled", "disabled");
                    };
                    var unlockRunning = function () {
                        $("#instance .tab-info-line .btn[data-name='run-test'].running").removeClass("running").removeAttr("disabled");
                        $("#instance .filter-labelinfo input[type=\"checkbox\"][disabled]").removeAttr("disabled");
                    };

                    var genInstanceMessage = function (instance, message, css) {
                        return "<li class=\"test-status " + css + "\"><span class=\"test-result-info\">" + instance + ": " + message + "</span></li>";
                    };
                    var genResultMessage = function (batchResult) {
                        var genGroupDiv = function (name, value, css, detail) {
                            var body = "";
                            if (detail) body = "<div><ul>" + detail + "</ul></div>";
                            return "<div class=\"control-group test-status " + css + "\"><span>" + name + ": </span><span>" + value + "</span>" + body + "</div>";
                        }

                        showMessage = genGroupDiv("Success instances", batchResult.successCount, statusCss["success"]);
                        if (batchResult.failedCount > 0)
                            showMessage += genGroupDiv("Failed instances", batchResult.failedCount, statusCss["failed"], batchResult.errorMessage);
                        if (batchResult.longTimeCount > 0)
                            showMessage += genGroupDiv("Long time instances (>" + longTime + " ms)", batchResult.longTimeCount, statusCss["longTime"], batchResult.timeMessage);
                        return showMessage;
                    };
                    var setBatchStatus = function (stepResult) {
                        batchResult.successCount += stepResult.successCount;
                        if (stepResult.failedCount > 0) {
                            batchResult.errorMessage += stepResult.errorMessage;
                            batchResult.failedCount += stepResult.failedCount;
                        }
                        if (stepResult.longTimeCount > 0) {
                            batchResult.timeMessage += stepResult.timeMessage;
                            batchResult.longTimeCount += stepResult.longTimeCount;
                        }
                        
                        if (testStack == 0) {//全部测试完成
                            unlockRunning();
                            body_status["test-running"] = false;
                            var showMessage = genResultMessage(batchResult);
                            ModalAlert("Test Result", showMessage, "OK");
                        }
                    };
                    var setStepStatus = function (data) {
                        var errorMessage = "", timeMessage = "";
                        var successCount = 0; failedCount = 0, longTimeCount = 0;
                        for (var i = 0; i < data.resultList.length; i++) {
                            result = data.resultList[i];
                            var instance = result.instance.ip + ":" + result.instance.port;
                            var css = result.status ? statusCss["success"] : statusCss["failed"];

                            if (result.status) successCount++;
                            else {
                                failedCount++;
                                errorMessage += genInstanceMessage(instance, result.message, css);
                            }
                            //testStack--;

                            var timeStr = result.responseTime.toFixed(3) + " ms (SET " + result.setTime.toFixed(3) + " ms, GET " + result.getTime.toFixed(3) + " ms)";
                            if (result.responseTime > longTime) {
                                longTimeCount++;
                                css = statusCss["longTime"];
                                timeMessage += genInstanceMessage(instance, timeStr, css);
                            }

                            var insElem = testContainer.find("li.test-panel-container:visible li[data-env].active .i-group>i[data-instance=\"" + instance + "\"]")
                                .addClass(css).addClass("test-status");
                            insElem.attr("data-result", JSON.stringify(result));

                            var title = insElem.attr("title") + titleHeader + result.message + "\r\n" + timeStr;
                            insElem.attr("title", title);
                        }

                        setBatchStatus({ "successCount": successCount, "failedCount": failedCount, "errorMessage": errorMessage, "longTimeCount": longTimeCount, "timeMessage": timeMessage });
                    };

                    var runBatch = function (list, unitCount) {
                        var batchCount = 0;
                        var unitCount = unitCount || 1;//设置批量数
                        var batchTestList = [];
                        for (var j = 0; j < list.length; j++) {
                            batchTestList.push(list[j]);
                            batchCount++;
                            testStack++;
                            if (batchCount % unitCount == 0) {
                                runByStep(batchTestList);
                                batchTestList = [];
                                batchCount = 0;
                            }
                        }
                        if (batchCount > 0) {
                            runByStep(batchTestList);
                            batchTestList = [];
                            batchCount = 0;
                        }
                    };
                    var runByStep = function (testList) {
                        cacheHandler.credis_testGroupList(testList, null, function () {
                            if (!body_status["test-running"]) {
                                var autoClear = $("#instance .actions-container>[data-name=\"auto-clear\"]>input[type=\"checkbox\"]").is(":checked");
                                if (!autoClear)
                                    _clearTestResult(testContainer.find("li.test-panel-container:visible li[data-env].active"), titleHeader);
                                else _clearTestResult(null, titleHeader);
                                body_status["test-running"] = true;
                                lockRunning();
                            }
                        }, function (data) {
                            testStack -= testList.length;
                            setStepStatus(data);
                        }, function (xhr) {
                            if (testList.length > 1) {
                                testStack -= testList.length;
                                runBatch(testList, 1);
                            }
                            else {
                                var message = xhr.status + ": " + xhr.statusText;
                                setStepStatus({ status: false, resultList: [{ instance: testList[0], status: false, message: message, responseTime: 0, setTime: 0, getTime: 0 }] });
                            }
                        });
                    };

                    runBatch(list, unitCount);
                }
            </script>
            <%-- page event --%>
            <script type="text/javascript">
                function AfterRender() {
                    $(document.body).limit();  //自动隐藏超长度字符
                    $(window).resize();
                    //$("#url").focus();
                }
                $(window).resize(function () {
                    $("body").outerHeight($(window).innerHeight() - $("body").offset().top);
                    //_responseEditorResize();
                });
            </script>
        </section>
    </div>
</body>
</html>
