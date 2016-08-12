(function ($) {
    var cacheHandler_default_settings = {
    };

    var CacheHandler = function (options) {
        this.settings = $.extend({}, cacheHandler_default_settings, options);
        return true;
    };

    CacheHandler.prototype.credis_getCluster = function (clusterName, options, successFunc, errorFunc) {
        if (!$.isFunction(errorFunc)) errorFunc = SendDataError;
        if (!clusterName) {
            errorFunc("Cluster name is required!");
            return false;
        }

        var url = "http://ws.fx.fws.qa.nt.ctripcorp.com/credis/configapi/getcluster/" + clusterName;
        var req = {
            name: "", 
            info: {
                method: "POST",
                requestUri: url,
                postData: { data: null, dataLanguage: "text", dataMode: "raw" },
                urlData: MatchUrl(url)
            }
        };
        HttpRequest(req, null, function (data) {//successFunc
            var response = data.info;
            var cluster = JSON.parse(response.responseContent || "{}");
            if ($.isFunction(successFunc)) successFunc(cluster);
        }, function (data) {//errorFunc
            var status = data.status;
            if ($.isPlainObject(status) && (status.exception || status.statusCode)) {
                if (status.exception) errorFunc(status.exception.message);
                if (status.statusCode != 0 && status.statusDescription != "") {
                    errorFunc(data.info.responseUri + ": [" + status.statusCode + "] " + status.statusDescription);
                }
            }
            else {//data=xhr
                errorFunc("Status: " + data.status + " (" + data.statusText + ")<hr>" + data.responseText);
            }
        });

        return true;
    };
    CacheHandler.prototype.credis_renderCluster = function (clusterData, options, successFunc, errorFunc) {
        if (!$.isFunction(errorFunc)) errorFunc = SendDataError;
        if (!clusterData) {
            errorFunc("Cluster is empty.");
            return false;
        }

        var data = clusterData;
        var clst = {
            "ID": data["ID"],
            "Name": data["Name"],
            "UsingIDC": data["UsingIDC"],
            "Status": data["Status"]
        };
        var clusterContainer = null;
        if ($.isFunction(options.getOrRenderCluster)) {
            clusterContainer = options.getOrRenderCluster(clst, null);
        }
        if (clusterContainer) {
            if ($.isFunction(options.viewClusterTitle)) {
                options.viewClusterTitle(clst, clusterContainer);
            }

            //if (data.Servers.length == 0)
            $.each(data.Servers, function (idx, svr) {
                var groupId = svr["GroupID"];
                var grp = _getItemInfo(data.Groups, function (item) {
                    return item["ID"] == groupId;
                }, function (i, item) {
                    return {
                        "ID": item["ID"],
                        "ClusterID": item["ClusterID"],
                        "Env": item["Env"]
                    };
                });
                var ins = {
                    "ID": svr["ID"],
                    "IP": svr["IPAddress"],
                    "Port": svr["Port"],
                    "ParentID": svr["ParentID"],
                    "GroupID": svr["GroupID"],
                    "CanRead": svr["CanRead"],
                    "DBNumber": svr["DBNumber"],
                    "IsPipelined": svr["IsPipelined"]
                };
                var groupDiv = null;
                if ($.isFunction(options.getOrRenderGroup)) {
                    groupDiv = options.getOrRenderGroup(grp, clusterContainer);
                }
                if (groupDiv) {
                    if ($.isFunction(options.viewGroupTitle)) {
                        options.viewGroupTitle(grp, groupDiv);
                    }

                    if ($.isFunction(options.getOrRenderInstance)) {
                        insElem = options.getOrRenderInstance(ins, clusterContainer);
                    }
                    if (insElem && $.isFunction(options.viewInstanceTitle)) {
                        options.viewInstanceTitle(ins, insElem);
                    }
                }
            });
        }

        if ($.isFunction(successFunc)) successFunc(data);
        return true;
    };

    CacheHandler.prototype.credis_testInstanceList = function (testList, options, beforeSendFunc, successFunc, errorFunc) {
        //if (!$.isFunction(errorFunc)) errorFunc = SendDataError;
        if (!$.isArray(testList)) {
            errorFunc("Params is invalid.");
            return false;
        }

        var request = {
            type: "POST",
            url: "./Handler/CRedisTest.asmx/TestInstances_json",
            data: { "data": JSON.stringify(testList) },
            dataType: 'text',
            //async: false,
            cache: false,
            beforeSend: function (xhr) {
                if ($.isFunction(beforeSendFunc)) beforeSendFunc(xhr);
            },
            complete: function (xhr, textStatus) {
                if (xhr.status == 200) {
                    var d = $.parseJSON(xhr.responseText);
                    if ($.isFunction(successFunc)) successFunc(d);
                }
                else {
                    if ($.isFunction(errorFunc)) errorFunc(xhr);
                    else {
                        ModalAlert("Error", xhr.status + ": " + xhr.statusText, "OK");
                    }
                }
            }
        };
        $.ajax(request);
    };
    CacheHandler.prototype.credis_testGroupList = function (testList, options, beforeSendFunc, successFunc, errorFunc) {
        //if (!$.isFunction(errorFunc)) errorFunc = SendDataError;
        if (!$.isArray(testList)) {
            errorFunc("Params is invalid.");
            return false;
        }

        var request = {
            type: "POST",
            url: "./Handler/CRedisTest.asmx/TestGroups_json",
            data: { "data": JSON.stringify(testList) },
            dataType: 'text',
            //async: false,
            cache: false,
            beforeSend: function (xhr) {
                if ($.isFunction(beforeSendFunc)) beforeSendFunc(xhr);
            },
            complete: function (xhr, textStatus) {
                if (xhr.status == 200) {
                    var d = $.parseJSON(xhr.responseText);
                    if ($.isFunction(successFunc)) successFunc(d);
                }
                else {
                    if ($.isFunction(errorFunc)) errorFunc(xhr);
                    else {
                        ModalAlert("Error", xhr.status + ": " + xhr.statusText, "OK");
                    }
                }
            }
        };
        $.ajax(request);
    };


    $.cacheHandler = function (options) {
        return new CacheHandler(options);
    };

    var SendDataError = function (data, errorFunc) {
        var status = data.status;
        var errorMessage = "Unknown Error!";
        if ($.isPlainObject(status) && (status.exception || status.statusCode)) {
            if (status.exception) {
                errorMessage = "[ErrorCode]：{0}<br/>{1}".Format(status.exception.errorCode, status.exception.message.htmlEncode());
            }
            if (status.statusCode != 0 && status.statusDescription) {
                errorMessage = "[StatusCode]：{0}<br/>{1}".Format(status.statusCode, status.statusDescription.htmlEncode());
            }
        }
        else {//data=xhr
            errorMessage = data.statusText;
        }
        if ($.isFunction(errorFunc)) errorFunc(errorMessage);
        else alert(errorMessage);
    }
})(jQuery);