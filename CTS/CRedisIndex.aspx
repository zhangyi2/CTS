<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRedisIndex.aspx.cs" Inherits="ctrip.Framework.ApplicationFx.CTS.CRedisIndex" %>

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
    <%--    <script src="<%=GetIncludeUrl("Scripts/upload.js")%>" type="text/javascript"></script>
    <script src="<%=GetIncludeUrl("Scripts/dmuploader.js")%>" type="text/javascript"></script>--%>
</head>
<body>
    <div id="main" class="content">
        <section id="config" style="display: block;">
            <div class="content-wrapper">
                <div class="cluster-info-container top-info-container">
                    <div class="cluster-baseinfo tab-info-line clearfix">
                        <div class="info-container tab-container">
                            <div class="name-wrapper tab-wrapper">
                                <div class="name-container">
                                    <span data-name="cluster-name" data-id="1">WS集群名称</span>
                                </div>
                            </div>
                        </div>
                        <div class="actions-container">
                            <button class="btn btn-primary" data-name="run-test">
                                Test services
                            </button>
                        </div>
                    </div>
                    <div class="cluster-serverinfo">
                        <div class="select-all-content clearfix">
                            <div class="select-all-panel ind label-data">
                                <span class="label">Server<input type="checkbox" title="Select all" data-name="cluster-select-all-ip" />
                                </span>
                            </div>
                            <div class="select-partial-panel">
                                <ul data-name="ip-list">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="filter-labelinfo" style="">
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
                </div>
                <div class="test-content-list clearfix">
                    <ul class="content-wrapper ui-sortable">
                        <li class="test-panel-container"></li>
                    </ul>
                </div>
            </div>
        </section>
        <section id="hidden_data" style="display: none;">
            <div id="username"><%= this.CurrentUserName %></div>
            <div id="userinfo"><%= this.CurrentUserInfo %></div>
            <div id="test-panel-container-templet">
                <div class="test-panel-container-wrapper">
                    <div class="info-container">
                        <div class="name-wrapper">
                            <div class="name-container">
                                <span data-name="item-name" title="Click to edit">GetCluster</span>
                            </div>
                            <div class="desc-container">
                                <span data-name="item-desc">No description</span>
                            </div>
                            <div class="params-container" style="display: none">
                            </div>
                        </div>
                    </div>
                    <div class="actions-container">
                        <div class="test-result" style="display: none">
                        </div>
                        <button class="btn" data-name="run-test" data-result="">
                            Test
                        </button>
                    </div>
                </div>
            </div>
            <div id="param-container-templet">
                <div class="param-container-wrapper">
                    <input type="text" data-name="param-value" class="input-text-light ui-autocomplete-input" placeholder="" autocomplete="off" />
                    <ul data-name="param-selector" class="dropdown-menu" style="display: none">
                    </ul>
                    <span data-name="item-desc">No description</span>
                </div>
            </div>
        </section>
        <section id="modal-container" style="display: block;">
            <div id="modal-test-report" class="modal fade" tabindex="-1" style="margin-top: 0px; display: none;">
                <div class="modal-header">
                    <a href="#" class="close" data-dismiss="modal">×</a>
                    <h3>Test result</h3>
                </div>
                <div class="modal-body clearfix">
                    <div class="control-group">
                        <span>Name: </span>
                        <span id="test-report-name"></span>
                    </div>
                    <div class="control-group">
                        <span>Result: </span>
                        <span id="test-report-result"></span>
                    </div>
                    <div class="control-group">
                        <span>Detail: </span>
                        <div id="test-report-detail">
                            <ul></ul>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a href="#" id="test-report-ok" class="btn btn-primary">OK</a>
                </div>
            </div>
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
                var data_ConfigService = {};
                var index_ServiceLabel = {};
                var body_status = {};
            </script>
            <%-- page event --%>
            <script type="text/javascript">
                $(document).ready(function () {
                    LoadConfig();
                    BindEvent();
                    AfterRender();
                })
                function BindEvent() {
                }
                function LoadConfig() {
                    loadConfig.getConfigServiceList(function (data) {
                        data_ConfigService = data || {};
                        InitConfigService(data_ConfigService.product[0]);
                        //var data_product = data_ConfigService.product;
                        //for (var i = 0; i < data_product.length; i++) {
                        //    var prod = data_product[i];
                        //    InitConfigService(prod);
                        //}
                    }, function (errorMessage) {
                        ModalAlert("Error", errorMessage, "OK");
                    });
                }
                function InitConfigService(prod) {
                    if (!prod) return;
                    //绑定cluster
                    var data_cluster = data_ConfigService.configapi_cluster;
                    var cluster = data_cluster[prod];
                    $("#config .content-wrapper").attr("prod", prod);
                    $("#config .cluster-baseinfo [data-name='cluster-name']").text(cluster["name"]).click(function () {
                        if (body_status["test-running"] == true) return;
                        ShowClusterEdit(false);
                    });

                    var ul_ip = $("#config .cluster-serverinfo .select-partial-panel ul").html("");
                    var all_server = $("#config .cluster-serverinfo .select-all-panel input[type='checkbox']").click(_selectAll);
                    var ul_label = $("#config .filter-labelinfo .select-partial-panel ul").html("");
                    var all_label = $("#config .filter-labelinfo .select-all-panel input[type='checkbox']").click(_selectAll);

                    for (var j = 0; j < cluster.ip.length; j++) {
                        var ip = cluster.ip[j];
                        $("<li></li>").attr("data-ip", ip).appendTo(ul_ip)
                            .append($("<input type=\"checkbox\" name=\"ip-select\"/>").val(ip).click(function () {
                                _selectPartial(this);
                            }))
                            .append($("<span></span>").text(ip).click(_selectPartialLabel))
                            .append($("<i data-name=\"test-status\" class=\"init\"></i>"));
                    }
                    $("#config .cluster-baseinfo .btn[data-name='run-test']").click(function () {
                        if (body_status["test-running"] == true) return;
                        RunPackage($(this).parentsUntil("#config", ".content-wrapper"));
                    });
                    $("#test-report-ok").click(function () {
                        CloseModal();
                    });
                    //绑定api列表
                    var apiList = data_ConfigService[prod + "_configapi"];
                    if (apiList.length > 0) {
                        var ul = $("#config .test-content-list>ul").html("");
                        var _setParam = function (container, param) {
                            var seletor_name = prod + "_" + cluster["name"] + "_" + param.name + "_seletor";
                            container.find(".params-container").append($("#param-container-templet").html());
                            var wrapper = container.find(".param-container-wrapper").attr("data-name", param.name);
                            body_status[seletor_name + "-refresh"] = true;
                            var val = param.default_value || "";
                            var input = wrapper.find("input[data-name='param-value']").val(val);
                            var seletor = wrapper.find("ul[data-name=\"param-selector\"]");
                            input.click(function () {
                                var items = _getItems(param.options, {
                                    labelBy: "name",
                                    valueBy: "value",
                                    titleBy: "desc",
                                    //autoMerge: true
                                });
                                seletor.attr("data-name", seletor_name);
                                _showItems(seletor, items, "relative", val, function (li, item) {
                                    input.next("[data-name='item-desc']").text(item.desc || "No description");
                                    input.val(item.value);
                                    seletor.css("display", "none");
                                });
                            });
                            wrapper.find("[data-name='item-desc']").text(param.desc || "No description");
                            $("body").click(function () {
                                if (body_status[seletor_name + "-display"] == "") {
                                    seletor.css("display", "none");
                                    body_status[seletor_name + "-display"] = "none";
                                }
                            });
                        };
                        var _setTestItem = function (api) {
                            var container = $("<li class='test-panel-container' data-item-name='" + api.name + "'></li>").appendTo(ul);
                            container.html($("#test-panel-container-templet").html());
                            container.find(".name-container [data-name='item-name']").text(api.name);
                            container.find(".desc-container [data-name='item-desc']").text(api.desc || "No description");
                            container.find(".name-container").click(function () {
                                if (container.is(".active")) {
                                    container.removeClass("active");
                                }
                                else container.addClass("active");
                                //单选后，label过滤失效
                                ul_label.find("li input[type='checkbox']").attr("checked", false);
                                all_label.attr("checked", false);
                            });
                            container.find(".btn[data-name='run-test']").click(function () {
                                RunTestCase(container);
                            });
                            container.find(".actions-container .test-result").click(function () {
                                $(this).css("display", "none").next(0).css("display", "");
                            });

                            if ($.isArray(api.params) && api.params.length > 0) {
                                container.find(".params-container").html("").css("display", "");
                                for (var m = 0; m < api.params.length; m++) {
                                    _setParam(container, api.params[m]);
                                }
                            }
                            if ($.isArray(api.label) && api.label.length > 0) {
                                for (var n = 0; n < api.label.length; n++) {
                                    var label = api.label[n];
                                    if ($.isArray(index_ServiceLabel[label])) index_ServiceLabel[label].push(api.name);
                                    else index_ServiceLabel[label] = [api.name];
                                }
                            }
                            else {
                                var label = "[None]";
                                if ($.isArray(index_ServiceLabel[label])) index_ServiceLabel[label].push(api.name);
                                else index_ServiceLabel[label] = [api.name];
                            }
                        };
                        index_ServiceLabel = {};
                        for (var i = 0; i < apiList.length; i++) {
                            _setTestItem(apiList[i]);
                        }
                    }
                    //绑定label
                    for (var k in index_ServiceLabel) {
                        $("<li></li>").attr("data-label", k).appendTo(ul_label)
                            .append($("<input type=\"checkbox\" name=\"label-select\"/>").val(k).click(function () {
                                var elem = $(this);
                                var label = elem.val();
                                if (ul_label.find("li[data-label!='" + label + "']").find("input[type='checkbox']:checked").length == 0) {
                                    $("li.test-panel-container").removeClass("active");
                                }
                                _selectPartial(this, function (selected) {
                                    if (selected) {
                                        $.each(index_ServiceLabel[label], function (idx, apiname) {
                                            $("li.test-panel-container[data-item-name='" + apiname + "']").addClass("active");
                                        })
                                    }
                                    else {
                                        $.each(index_ServiceLabel[label], function (idx, apiname) {
                                            var api_label = _getItemInfo(apiList, function (item) {
                                                return item.name == apiname;
                                            }, function (i, item) {
                                                return item.label;
                                            });
                                            var checked = false;
                                            for (var i = 0; i < api_label.length; i++) {
                                                if (label != api_label[i]) {
                                                    if (ul_label.find("li[data-label='" + api_label[i] + "']").is(":checked")) checked = true;
                                                }
                                            }
                                            if (!checked) $("li.test-panel-container[data-item-name='" + apiname + "']").removeClass("active");
                                        })
                                    }
                                });
                            }))
                            .append($("<span></span>").text(k).click(_selectPartialLabel));
                    }
                }
            </script>
            <%-- run test --%>
            <script type="text/javascript">
                function ShowClusterEdit(isNew) {
                    /*ShowModal("modal-cluster-edit", function () {//init
                        if (isNew == false) {
                            //set package info
                            //var packageName = $("#package-name").text();
                            var packageId = parseInt($("#package-name").attr("data-id"));
                            var pkg = _getPackage(packageId);
                            if (pkg) {
                                //$("#cluster-edit-id").val(packageId);
                                $("#cluster-edit-name").val(pkg.baseInfo.name);
                                $("#cluster-edit-description").val(pkg.baseInfo.description);

                                $("#modal-cluster-edit .modal-header h3").html("Edit cluster");
                                $("#modal-cluster-edit #cluster-edit-save").text("Save");
                            }
                            else isNew = true;
                        }
                        if (isNew == true) {
                            //$("#cluster-edit-id").val(0);
                            $("#cluster-edit-name").val("");
                            $("#cluster-edit-description").val("");

                            $("#modal-cluster-edit .modal-header h3").html("New cluster");
                            $("#modal-cluster-edit #cluster-edit-save").text("New");
                        }
                    }, null, null, $("#cluster-edit-name"));*/
                }

                function _lockRunning(container) {
                    if (container) {
                        container.addClass("disabled");
                        container.find("input[type='checkbox'], button").attr("disabled", true);
                        container.find("input[data-name='param-value']").blur().attr("disabled", true);
                        container.parentsUntil("#config", ".content-wrapper").find(".cluster-baseinfo .btn[data-name='run-test']").attr("disabled", true);
                    }
                    else {
                        $("input[type='checkbox'], button").attr("disabled", true);
                        $(".test-panel-container.active").addClass("disabled")
                            .find("input[data-name='param-value']:visible").blur().attr("disabled", true);
                    }
                    $("#config .cluster-info-container input[type='checkbox']").attr("disabled", true);
                    body_status["test-running"] = true;
                }
                function _unlockRunning(container) {
                    if (container) {
                        container.removeClass("disabled");
                        container.find("input[type='checkbox'], button").removeAttr("disabled");
                        container.find("input[data-name='param-value']").removeAttr("disabled");
                        container.parentsUntil("#config", ".content-wrapper").find(".cluster-baseinfo .btn[data-name='run-test']").removeAttr("disabled");
                    }
                    else {
                        $("input[type='checkbox'], button").removeAttr("disabled");
                        $(".test-panel-container.active").removeClass("disabled")
                            .find("input[data-name='param-value']").removeAttr("disabled");
                    }
                    $("#config .cluster-info-container input[type='checkbox']").removeAttr("disabled");
                    $(".lock-test-running").removeClass("lock-test-running");
                    body_status["test-running"] = false;
                }
                function _removeStatus(elem) {
                    elem.removeClass("success abort timeout exception failed running");
                    return elem;
                }
                function _getRunStatus(dataStatus) {
                    if (dataStatus && dataStatus.statusCode != undefined) {
                        var statusCode = dataStatus.statusCode;
                        var statusDescription = dataStatus.statusDescription || "";
                        var cssName = "", title = "";
                        switch (statusCode) {
                            case DataStatusCode.Success:
                                cssName = "success";
                                break;
                            case DataStatusCode.Abort:
                                cssName = "abort";
                                title = statusDescription;
                                break;
                            case DataStatusCode.Timeout:
                                cssName = "timeout";
                                title = statusDescription;
                                break;
                            case DataStatusCode.ValidationException:
                            case DataStatusCode.DalException:
                            case DataStatusCode.BizException:
                            case DataStatusCode.UnkownException:
                                cssName = "exception";
                                var exception = dataStatus.exception;
                                if (exception && exception.errorCode) {
                                    title = exception.errorCode + "\n" + exception.message;
                                }
                                else title = statusDescription;
                                break;
                            case DataStatusCode.Failed:
                            default:
                                cssName = "failed";
                                title = statusDescription;
                                break;
                        }
                        return {
                            statusCode: statusCode,
                            statusDescription: statusDescription,
                            cssName: cssName,
                            title: title.replace("<br/>", "\n")
                        };
                    }
                    else return {
                        statusCode: DataStatusCode.Failed,
                        statusDescription: "Failed for an unknown reason.",
                        cssName: "failed",
                        title: "Failed for an unknown reason."
                    }
                }
                function _clearRunTestStatus(container) {
                    if (container) {
                        //container.find(".actions-container .test-result").css("display", "none").find("div").removeAttr("class");
                        //container.find(".actions-container .btn[data-name='run-test']").css("display", "").attr("data-result", "");
                        container.find(".actions-container .test-result div").removeAttr("class");
                        container.find(".actions-container .btn[data-name='run-test']").attr("data-result", "");
                    }
                    else {
                        _removeStatus($("#config .test-panel-container"));
                        $("#config .test-panel-container .test-result").css("display", "none").find("div").removeAttr("class");
                        $("#config .test-panel-container .btn[data-name='run-test']").css("display", "").attr("data-result", "");
                        $("#config .cluster-baseinfo .btn[data-name='run-test']").attr("class", "btn btn-primary");
                        $("#config .cluster-serverinfo .select-partial-panel ul li i").attr("class", "init");
                    }
                }
                function GetReqInfo(api, paramsValue) {
                    var url = api.url;
                    if ($.isArray(api.params) && api.params.length > 0) {
                        for (var i = 0; i < api.params.length; i++) {
                            var param = api.params[i];
                            var value = paramsValue[param.name] || param.default_value;

                            if (param.handler == "UrlAppend") {
                                url += value;
                            }
                            else if (param.handler == "UrlParam") {
                                if (url.indexOf("?") <= 0) url += "?";
                                url += param.name + "=" + value;
                            }
                        }
                    }
                    var reqInfo = { method: api.method, requestUri: url, urlData: MatchUrl(url), contentType: api.contentType || DEFAULT_CONTENT_TYPE };
                    return reqInfo;
                }
                function RunTestCase(container, options) {
                    if (!container) return;
                    var isPackageRunning = options != undefined;
                    options = options || {};
                    //获取api信息
                    var prod = "credis";
                    var apiName = container.attr("data-item-name");
                    var apiList = data_ConfigService[prod + "_configapi"];
                    var api = _getItem(apiList, function (item) {
                        return item.name == apiName;
                    });
                    var paramsValue = {};
                    $.each(container.find(".params-container div"), function (i, paramWrapper) {
                        paramsValue[$(paramWrapper).attr("data-name")] = $(paramWrapper).find("input[data-name='param-value']").val();
                    })
                    //动态加ip测试结果块
                    var select_ips = $("#config .cluster-serverinfo .select-partial-panel input[type='checkbox']:checked");
                    var test_result = container.find(".test-result").html("");
                    $.each(select_ips, function (i, ip) {
                        test_result.append($("<div data-ip='" + $(ip).val() + "'></div>"));
                    })
                    var items = test_result.find("div");

                    var batchInfo = { stepCount: select_ips.length, finishCount: 0 };

                    //if (isPackageRunning != true)
                    //MC_Count("TestCounter", { "Product": prod, "About": apiName });

                    var runTest = function (reqInfo) {
                        var testInfo = { "product": prod, "testCaseName": apiName, "reqInfo": reqInfo, "testIp": "" };
                        var initFunc = function () {
                            _removeStatus(container).addClass("running");
                            if (select_ips.length > 0) {
                                container.find(".actions-container .test-result").css("display", "");
                                container.find(".actions-container .btn").css("display", "none");
                            }
                            if (!isPackageRunning) _lockRunning(container);
                        };
                        var stepNextFunc = function (stepResult) {
                            if (stepResult.batchInfo && stepResult.batchInfo.finishCount) {
                                batchInfo = stepResult.batchInfo;
                            }
                            else {
                                batchInfo.finishCount += 1;
                                stepResult.batchInfo = $.extend({}, stepResult.batchInfo, { finishCount: batchInfo.finishCount });
                            }
                            if (batchInfo.finishCount < items.length) testByStep(items[batchInfo.finishCount], batchInfo);
                        };
                        var setRunStatus = function (finalResult) {
                            var status = _getRunStatus(finalResult.finalStatus);
                            _removeStatus(container).addClass(status.cssName);
                            container.find(".btn[data-name='run-test']").attr("data-result", JSON.stringify(finalResult));
                            if (!isPackageRunning) {
                                _unlockRunning(container);
                                ShowTestReport(apiName, finalResult.finalStatus, [finalResult]);
                            }

                            //MC_Count("CaseRunningCounter", { "Product": prod, "About": apiName, "StatusCode": status.cssName });
                        };
                        var testByStep = function (item, batchInfo) {
                            var div = $(item);
                            var testIp = div.attr("data-ip");
                            $.extend(testInfo, { testIp: testIp });
                            SendTestCaseByStep(testInfo, batchInfo, {
                                initFunc: initFunc,
                                beforeSendFunc: function () {
                                    div.removeAttr("class").removeAttr("title").addClass("running");
                                },
                                stepNextFunc: stepNextFunc,
                                stepSuccessFunc: function (stepResult) {
                                    div.removeAttr("class").addClass("success").attr("title", testIp);
                                },
                                stepErrorFunc: function (stepResult) {
                                    var status = _getRunStatus(stepResult.stepStatus);
                                    div.removeAttr("class").addClass(status.cssName).attr("title", testIp + "&#13;".htmlDecode() + status.title);
                                },
                                finalSuccessFunc: function (finalResult) {
                                    setRunStatus(finalResult);
                                    if ($.isFunction(options.stepNextFunc)) options.stepNextFunc(finalResult);
                                },
                                finalErrorFunc: function (finalResult) {
                                    setRunStatus(finalResult);
                                    if ($.isFunction(options.stepNextFunc)) options.stepNextFunc(finalResult);
                                }
                            });
                        };

                        if (!isPackageRunning) _clearRunTestStatus(container);
                        if (items.length > 0) {//按IP逐个访问
                            var i = 0;
                            var item = items[i];
                            testByStep(item, batchInfo);
                        }
                        else {//随机按域名访问
                            SendTestCaseByStep(testInfo, batchInfo, {
                                initFunc: initFunc,
                                stepNextFunc: stepNextFunc,
                                finalSuccessFunc: function (finalResult) {
                                    setRunStatus(finalResult);
                                    if ($.isFunction(options.stepNextFunc)) options.stepNextFunc(finalResult);
                                },
                                finalErrorFunc: function (finalResult) {
                                    setRunStatus(finalResult);
                                    if ($.isFunction(options.stepNextFunc)) options.stepNextFunc(finalResult);
                                }
                            });
                        }
                    }

                    var reqInfo = GetReqInfo(api, paramsValue);
                    runTest(reqInfo);
                }
                function RunPackage(wrapper) {
                    //validation
                    var checkList = wrapper.find(".test-panel-container.active");
                    if (checkList.length == 0) {
                        ModalAlert("Alert", "You have not select any test case.", "OK");
                        return;
                    }
                    var prod = wrapper.attr("data-name");
                    var clusterName = wrapper.find(".cluster-baseinfo span[data-name='cluster-name']").text();
                    var btnRunTest = wrapper.find(".cluster-baseinfo .btn[data-name='run-test']");

                    //MC_Count("TestCounter", { "Department": dataHandler.settings.curUser.Department, "About": "TestPackage" });

                    //func
                    var initFunc = function () {
                        _clearRunTestStatus();
                        btnRunTest.addClass("running");
                        _lockRunning();
                    };
                    var setRunStatus = function () {
                        var cssName = success ? "success" : "failed";
                        btnRunTest.attr("class", "btn btn-primary").addClass(cssName);
                        _unlockRunning();
                        var dataStatus = { statusCode: success ? DataStatusCode.Success : DataStatusCode.Failed };
                        ShowTestReport(clusterName, dataStatus, resultData);
                    };
                    //run
                    var success = true;
                    var resultData = [];
                    var batchInfo = { stepCount: checkList.length, finishCount: 0 };
                    var testSeqMode = "Default";//Default,Sequence,Dependence
                    if (testSeqMode == "Default") {
                        initFunc();
                        $.each(checkList, function (idx, item) {
                            var container = $(item);
                            RunTestCase(container, {
                                stepNextFunc: function (stepResult) {
                                    resultData.push(stepResult);
                                    batchInfo.finishCount += 1;
                                    var stepSuccess = stepResult.finalStatus.statusCode == DataStatusCode.Success;
                                    if (!stepSuccess) success = false;
                                    if (batchInfo.finishCount == batchInfo.stepCount) setRunStatus();
                                },
                            });
                        });
                    }
                    else {
                        initFunc();
                        var testByStep = function (container) {
                            RunTestCase(container, {
                                stepNextFunc: function (stepResult) {//stepResult即testcase的finalResult
                                    resultData.push(stepResult);
                                    batchInfo.finishCount += 1;
                                    var stepSuccess = stepResult.finalStatus.statusCode == DataStatusCode.Success;
                                    if (!stepSuccess) success = false;
                                    if (batchInfo.finishCount == batchInfo.stepCount) setRunStatus();
                                    else {
                                        if (stepSuccess || testSeqMode == "Sequence") {
                                            container = $(checkList[batchInfo.finishCount]);
                                            testByStep(container);
                                        }
                                        else if (testSeqMode == "Dependence") {
                                            setRunStatus();
                                        }
                                    }
                                }
                            });
                        };
                        var container = $(checkList[batchInfo.finishCount]);
                        testByStep(container);
                    }
                }
                function ShowTestReport(testName, finalStatus, resultData) {
                    ShowModal("modal-test-report", function () {//init
                        var runStatus = _getRunStatus(finalStatus);
                        $("#test-report-name").text(testName);
                        $("#test-report-result").text(runStatus.cssName).attr("class", "test-status").addClass(runStatus.cssName);
                        var ul = $("#test-report-detail ul").html("");
                        $.each(resultData, function (idx, item) {
                            if (item.testCaseName && item.finalStatus) {
                                runStatus = _getRunStatus(item.finalStatus);
                                var li = $("<li class='test-status'></li>").addClass(runStatus.cssName);
                                li.append($("<span class='test-result-info'></span>").text(item.testCaseName + ": " + runStatus.cssName + " (" + item.responseTime.toFixed(3) + " ms)"));
                                if (runStatus.title) li.append($("<br/>")).append($("<span class='test-result-desc'></span>").text(runStatus.title.replace("\n", " ")));
                                ul.append(li);
                            }
                        });
                    }, null, null, $("#test-report-ok"), null);
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
