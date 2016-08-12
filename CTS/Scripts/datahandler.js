(function($) {
    var dataHandler_default_settings = {
        "curVersion": "3.0",
        "curUser": { Name: "Unknown", Department: "Unknown" },
        "history-count": 10,
        "testcase-count": 10,
        //以下仅无DB时测试用
        "fromDB": true,
        "local_history_newid": 1,
        "local_testcase_newid": 1,
        "local_package_newid": 1
    };

    var DataHandler = function(options)
    {
        this.settings = $.extend({}, dataHandler_default_settings, options);
        return true;
    };
    DataHandler.prototype.checkVersion = function (refreshFunc) {
        var version = localStorage.getItem(storageKey.Version) || "";
        if (this.settings.curVersion > version) {
            if ($.isFunction(refreshFunc)) refreshFunc(version, this.settings.curVersion);
        }
    }
    DataHandler.prototype.loadHistory = function (successFunc, errorFunc) {
        var hisVersion = localStorage.getItem(storageKey.Version) || "";
        var loadKey = storageKey.ReqHistory;
        if (hisVersion <= "1.0") {//from local storage
            var historyStr = localStorage.getItem(loadKey);
            if (!historyStr) {
                loadKey = "ReqHistory";
                historyStr = localStorage.getItem(loadKey);
            }
            var reqHistoryList = JSON.parse(historyStr ? unescape(historyStr) : "[]");

            if (curVersion != hisVersion) {
                reqHistoryList = this._versionUp(reqHistoryList, hisVersion);
                reqHistoryList = _mergeItems(reqHistoryList, "asc", "name", null);
                if (curVersion > "1.0") {
                    if (reqHistoryList.length > 0) {
                        //save to mysql
                        this.saveHistory(reqHistoryList, function (data) {
                            localStorage.removeItem(loadKey);
                            localStorage.setItem(storageKey.Version, curVersion);
                            if ($.isFunction(successFunc)) successFunc({ "reqHistoryList": reqHistoryList });
                        }, function (errorMessage) {
                            if ($.isFunction(errorFunc)) errorFunc(errorMessage, reqHistoryList);
                            else alert(errorMessage);
                        })
                    }
                    else {
                        localStorage.removeItem(loadKey);
                        localStorage.setItem(storageKey.Version, curVersion);
                        if ($.isFunction(successFunc)) successFunc({ "reqHistoryList": reqHistoryList });
                    }
                }
                else {//save to local storage
                    localStorage.setItem(storageKey.ReqHistory, escape(JSON.stringify(reqHistoryList)));
                    localStorage.removeItem(loadKey);
                    localStorage.setItem(storageKey.Version, curVersion);
                    if ($.isFunction(successFunc)) successFunc({ "reqHistoryList": reqHistoryList });
                }
            }
        }
        else {//from mysql
            //local test
            if (this.settings.fromDB == false) {
                if ($.isFunction(successFunc)) successFunc({ "reqHistoryList": [], "reqCollectionList": [] });
                return;
            }

            SendData({
                "url": "./Handler/HistoryManager.asmx/GetRecentHistoryByUser",
                "data": { "userName": this.settings.curUser.Name, "topN": settings["history-count"] }
            }, null, function (data) {
                var reqHistoryList = data.data;
                for (var i = 0; i < reqHistoryList.length; i++) {
                    _preHandle(reqHistoryList[i]);
                }
                reqHistoryList = _mergeItems(reqHistoryList, "desc", "name", [
                    function (d) { return d.dbInfo.datachange_lasttime; }
                ]);
                dataHandler.loadCollection(function (hisInfoList) {
                    if ($.isFunction(successFunc)) successFunc({ "reqHistoryList": reqHistoryList, "reqCollectionList": hisInfoList });
                }, errorFunc);
            }, function (data) {
                SendDataError(data, errorFunc);
            });
        }
    };
    DataHandler.prototype.loadCollection = function (successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            if ($.isFunction(successFunc)) successFunc([]);
            return;
        }

        SendData({
            "url": "./Handler/HistoryManager.asmx/GetHistoryInfoListByUser",
            "data": { "userName": this.settings.curUser.Name, "orderBy": "" }//默认以host name排序
        }, null, function (data) {
            var hisInfoList = data.data;
            for (var i = 0; i < hisInfoList.length; i++) {
                hisInfoList[i].name = _getReqName(hisInfoList[i]);
            }
            hisInfoList = _mergeItems(hisInfoList, "asc", "name", null);
            if ($.isFunction(successFunc)) successFunc(hisInfoList);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.loadTestCase = function (successFunc, errorFunc) {
        if (curVersion != hisVersion) localStorage.setItem(storageKey.Version, curVersion);
        if (curVersion > "2.0") {
            //local test
            if (this.settings.fromDB == false) {
                if ($.isFunction(successFunc)) successFunc({ "testCaseList": [], "testCaseInfoList": [] });
                return;
            }

            SendData({
                "url": "./Handler/TestCaseManager.asmx/GetRecentTestCaseByUser",
                "data": { "userName": this.settings.curUser.Name, "topN": settings["testcase-count"] }
            }, null, function (data) {
                var testCaseList = data.data;
                for (var i = 0; i < testCaseList.length; i++) {
                    _preHandle2(testCaseList[i]);
                }
                testCaseList = _mergeItems(testCaseList, "desc", null, [
                    function (d) { return d.dbInfo.datachange_lasttime; }
                ]);
                dataHandler.loadTestCaseInfo(function (testCaseInfoList) {
                    if ($.isFunction(successFunc)) successFunc({ "testCaseList": testCaseList, "testCaseInfoList": testCaseInfoList });
                }, errorFunc);
            }, function (data) {
                SendDataError(data, errorFunc);
            });
        }
        else {
            if ($.isFunction(successFunc)) successFunc({ "testCaseList": [], "testCaseInfoList": [] });
        }
    };
    DataHandler.prototype.loadTestCaseInfo = function (successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            if ($.isFunction(successFunc)) successFunc([]);
            return;
        }

        SendData({
            "url": "./Handler/TestCaseManager.asmx/GetTestCaseInfoListByUser",
            "data": { "userName": this.settings.curUser.Name, "orderBy": "" }//默认以package name排序
        }, null, function (data) {
            var caseInfoList = data.data;
            for (var i = 0; i < caseInfoList.length; i++) {
                caseInfoList[i].reqName = _getReqName(caseInfoList[i]);
                caseInfoList[i].hostName = caseInfoList[i].urlData.host;
            }
            caseInfoList = _mergeItems(caseInfoList, "asc", null, "name");
            if ($.isFunction(successFunc)) successFunc(caseInfoList);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.loadPackage = function (successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            if ($.isFunction(successFunc)) successFunc([]);
            return;
        }

        SendData({
            "url": "./Handler/PackageManager.asmx/GetTestPackageListByUser",
            "data": { "userName": this.settings.curUser.Name, "orderBy": "" }//默认以package name排序
        }, null, function (data) {
            var packageList = data.data;
            packageList = _mergeItems(packageList, "asc", null, function (item) {
                return item.baseInfo.name;
            }, null);
            if ($.isFunction(successFunc)) successFunc(packageList);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };

    DataHandler.prototype._dbinfoCheck = function (data, type) {
        if (data.dbInfo) return data;
        type = type || "reqHistory";
        $.extend(data, {
            "dbInfo": {
                "create_user": dataHandler.settings.curUser.Name,
                "create_time": data.createTime || new Date(),
                "datachange_lasttime": new Date()
            }
        });
        if (type == "reqHistory") {
            $.extend(data.dbInfo, {
                "req_history_id": 0,
                //"collection_id": 0,
                "host_name": data.info.urlData.host,
                "dataread_lasttime": new Date(),
            });
            delete data.createTime;
        }
        else if (type == "testCase") {
            $.extend(data.dbInfo, {
                "test_case_id": data.baseInfo.testCaseId || 0,
                //"collection_id": 0,
                "is_public": false,
                "is_deleted": false,
                "dataread_lasttime": new Date(),
            });
            if (data.info) {
                $.extend(data.dbInfo, { "host_name": data.info.urlData.host });
            }
        }
        else if (type == "testPackage") {
            $.extend(data.dbInfo, {
                "package_id": data.baseInfo.packageId || 0,
                //"collection_id": 0,
                "is_public": false,
                "is_deleted": false,
            });
        }
        return data;
    };

    //版本升级后的数据兼容预处理
    DataHandler.prototype._versionUp = function (data, dataVersion) {
        var widget = this;

        var convertData = [];
        if ($.isArray(data)) {
            if (!dataVersion) {//原始版本
                if (widget.settings.curVersion == "1.0") return _versionUp_0_1(data);
                else if (widget.settings.curVersion == "2.0") return _versionUp_1_2(_versionUp_0_1(data));
            }
            else if (dataVersion == "1.0" && widget.settings.curVersion == "2.0") {
                return _versionUp_1_2(data);
            }
        }
        return data;
    };

    var _versionUp_0_1 = function (data) {
        var convertData = [];
        for (var i = 0; i < data.length; i++) {
            var d = {
                "name": _getReqName(data[i].info),
                "info": {
                    "requestUri": data[i].info.requestUri,
                    "urlData": data[i].info.urlData,
                    "method": data[i].info.method,
                    "contentType": data[i].info.contentType,
                },
                createTime: data[i].createTime || new Date()
            };
            if (data[i].info.headers) $.extend(d.info, { "headers": data[i].info.headers });
            if (data[i].info.postData) $.extend(d.info, { "postData": data[i].info.postData });
            if (data[i].info.requestIp) {
                $.extend(d.info, { "requestIp": data[i].info.requestIp });
                $.extend(d, { "bindIp": data[i].info.requestIp });
            }
            var ipList = data[i].ipList || data[i].info.ipList;
            if (ipList) {
                if (d.bindIp) {
                    var isFound = false;
                    for (var j = 0; j < ipList.length; j++) {
                        if (ipList[j].replace("*", "") == d.bindIp) {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound) ipList.push(d.bindIp);
                }
                $.extend(d, { "ipList": ipList });
            }
            else if (d.bindIp) $.extend(d, { "ipList": [d.bindIp] });
            $.extend(d, { "version": "1.0" });

            var isConflict = false;
            for (var k = 0; k < convertData.length; k++) {
                if (convertData[k].name == d.name) {//冲突取后面的值
                    convertData[k] = d;
                    isConflict = true;
                    break;
                }
            }
            if (!isConflict) convertData.push(d);
        }
        return convertData;
    };
    var _versionUp_1_2 = function (data) {
        //reqHistory={
        //    name:req_name,
        //    [bindIp]:bind_ip,
        //    [ipList]:ip_list.tolist,
        //    info:getReqInfoById(),
        //        info:{
        //                requestUri:request_uri, urlData:{...}, method:method, contentType:content_type, postData:{...}, 
        //                      postData:{ dataMode: data_mode, data:post_data, [inputData, dataLanguage] }
        //                  [requestIp, headers:headers,] +author:author
        //        }
        //    -createTime,
        //    +dbInfo:{
        //        +req_history_id
        //        [+collection_id]
        //        +host_name
        //        +create_user
        //        +create_time
        //        +dataread_lasttime
        //        +datachange_lasttime
        //    }
        //}
        for (var i = 0; i < data.length; i++) {
            var d = data[i];
            dataHandler._dbinfoCheck(data[i]);
            //delete d.info.requestIp;
            $.extend(d.info, { "author": "" });
            $.extend(d, { "version": "2.0" });
        }
        return data;
    };
    //reqHistory预处理
    var _preHandle = function (req) {
        req.info.urlData = MatchUrl(req.info.requestUri);
        req.info.requestIp = req.bindIp;
        req.name = _getReqName(req.info);
    };
    //testCase预处理
    var _preHandle2 = function (tc) {
        tc.info.urlData = MatchUrl(tc.info.requestUri);
        tc.baseInfo.reqName = _getReqName(tc.info);
        tc.baseInfo.hostName = tc.info.urlData.host;
    };

    DataHandler.prototype._getReqName = function (method, requestUri) {
        if (!requestUri) return "";
        var urlData = MatchUrl(requestUri);
        var reqname = method + " ";
        if (urlData.protocol != "http") reqname += urlData.protocol + "://";
        reqname += urlData.host;
        if (urlData.port != "80") reqname += ":" + urlData.port;
        reqname += urlData.path;
        return reqname;
    };

    DataHandler.prototype._convertToCollection = function (req) {
        return {
            "name": req.name || _getReqName(req.info),
            "method": req.info.method,
            "requestUri": req.info.requestUri,
            "bindIp": req.bindIp,
            "hostName": req.info.urlData.host,
            "historyId": req.dbInfo.req_history_id
        };
    };
    DataHandler.prototype._convertToTestCaseCollection = function (tc) {
        var tcInfo = {
            "name": tc.baseInfo.name,
            "description": tc.baseInfo.description,
            "bindIps": tc.baseInfo.bindIps,
            "packageIds": tc.baseInfo.packageIds,
            "testCaseId": tc.baseInfo.testCaseId
        };
        if (tc.info) {
            $.extend(tcInfo, {
                "reqName": _getReqName(tc.info),
                "method": tc.info.method,
                "requestUri": tc.info.requestUri,
                "hostName": tc.info.urlData.host
            });
        }
        return tcInfo;
    };


    /*
        ReqHistory events
    */
    DataHandler.prototype.saveHistory = function (reqHistoryList, successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            var localId = reqHistoryList[0].dbInfo.historyId;
            if (localId == 0) {
                reqHistoryList[0].dbInfo.historyId = this.settings.local_history_newid;
                this.settings.local_history_newid += 1;
            }
            successFunc(reqHistoryList);
            return;
        }

        SendData({
            "url": "./Handler/HistoryManager.asmx/SaveReqHistoryList",
            "data": { "reqHistoryList_json": JSON.stringify(reqHistoryList), "userName": this.settings.curUser.Name }
        }, null, function (data) {
            var ret = data.data;
            var saved = true;
            if (ret && $.isArray(ret) && ret.length == reqHistoryList.length) {
                for (var i = 0; i < reqHistoryList.length; i++) {
                    if (ret[i] <= 0) saved = false;
                    else if (!(reqHistoryList[i].dbInfo && reqHistoryList[i].dbInfo.req_history_id > 0)) {//新增，则更新ID值
                        $.extend(reqHistoryList[i].dbInfo, { "req_history_id": ret[i] });
                    }
                }
            }
            if (!saved) {
                var errorMessage = "Save history not success: <br/>Some request history was not saved.";
                if ($.isFunction(errorFunc)) errorFunc(errorMessage);
                else alert(errorMessage);
            }
            else if ($.isFunction(successFunc)) successFunc(reqHistoryList);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.deleteHistory = function (id, successFunc, errorFunc) {
        SendData({
            "url": "./Handler/HistoryManager.asmx/DeleteHistory",
            "data": { "id": id }
        }, null, function (data) {
            if ($.isFunction(successFunc)) successFunc(data.data);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.updateHistoryDate = function (req, colDate, successFunc, errorFunc) {
        var id = req.dbInfo.req_history_id;
        if (id > 0) {
            SendData({
                "url": "./Handler/HistoryManager.asmx/UpdateHistoryDate",
                "data": { "id": id, "colDate": colDate, "time": new Date().format('yyyy-MM-dd HH:mm:ss') }
            }, null, function (data) {
                if ($.isFunction(successFunc)) successFunc(data.data);
            }, function (data) {
                SendDataError(data, errorFunc);
            });
        }
    };
    DataHandler.prototype.getHistory = function (id, successFunc, errorFunc) {
        SendData({
            "url": "./Handler/HistoryManager.asmx/GetHistory",
            "data": { "id": id, "updateReadLastTime": true }
        }, null, function (data) {
            var req = data.data;
            _preHandle(req);
            if ($.isFunction(successFunc)) successFunc(req);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };


    /*
        Test case & package events
    */
    DataHandler.prototype.saveTestCase = function (testCaseList, successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            var localId = testCaseList[0].baseInfo.testCaseId;
            if (localId == 0) {
                testCaseList[0].baseInfo.testCaseId = this.settings.local_testcase_newid;
                testCaseList[0].dbInfo.test_case_id = this.settings.local_testcase_newid;
                this.settings.local_testcase_newid += 1;
            }
            successFunc(testCaseList);
            return;
        }

        SendData({
            "url": "./Handler/TestCaseManager.asmx/SaveTestCaseList",
            "data": { "testCaseList_json": JSON.stringify(testCaseList), "userName": this.settings.curUser.Name }
        }, null, function (data) {
            var ret = data.data;
            var saved = true;
            if (ret && $.isArray(ret) && ret.length == testCaseList.length) {
                for (var i = 0; i < testCaseList.length; i++) {
                    if (ret[i] <= 0) saved = false;
                    else if (!(testCaseList[i].dbInfo && testCaseList[i].dbInfo.test_case_id > 0)) {//新增，则更新ID值
                        $.extend(testCaseList[i].baseInfo, { "testCaseId": ret[i] });
                        $.extend(testCaseList[i].dbInfo, { "test_case_id": ret[i] });
                    }
                }
            }
            if (!saved) {
                var errorMessage = "Save test case not success: <br/>Some test case was not saved.";
                if ($.isFunction(errorFunc)) errorFunc(errorMessage);
                else alert(errorMessage);
            }
            else if ($.isFunction(successFunc)) successFunc(testCaseList);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.savePackage = function (testPackageList, successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            var localId = testPackageList[0].baseInfo.packageId;
            if (localId == 0) {
                testPackageList[0].baseInfo.packageId = this.settings.local_package_newid;
                testPackageList[0].dbInfo.package_id = this.settings.local_package_newid;
                this.settings.local_package_newid += 1;
            }
            successFunc(testPackageList);
            return;
        }

        SendData({
            "url": "./Handler/PackageManager.asmx/SaveTestPackageList",
            "data": { "testPackageList_json": JSON.stringify(testPackageList), "userName": this.settings.curUser.Name }
        }, null, function (data) {
            var ret = data.data;
            var saved = true;
            if (ret && $.isArray(ret) && ret.length == testPackageList.length) {
                for (var i = 0; i < testPackageList.length; i++) {
                    if (ret[i] <= 0) saved = false;
                    else if (!(testPackageList[i].dbInfo && testPackageList[i].dbInfo.package_id > 0)) {//新增，则更新ID值
                        $.extend(testPackageList[i].baseInfo, { "packageId": ret[i] });
                        $.extend(testPackageList[i].dbInfo, { "package_id": ret[i] });
                    }
                }
            }
            if (!saved) {
                var errorMessage = "Save test package not success: <br/>Some test package was not saved.";
                if ($.isFunction(errorFunc)) errorFunc(errorMessage);
                else alert(errorMessage);
            }
            else if ($.isFunction(successFunc)) successFunc(testPackageList);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.markDeleteTestCase = function (id, successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            return;
        }

        SendData({
            "url": "./Handler/TestCaseManager.asmx/MarkDeleteTestCase",
            "data": { "id": id }
        }, null, function (data) {
            if ($.isFunction(successFunc)) successFunc(data.data);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.markDeleteTestPackage = function (id, successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            return;
        }

        SendData({
            "url": "./Handler/PackageManager.asmx/MarkDeleteTestPackage",
            "data": { "id": id }
        }, null, function (data) {
            if ($.isFunction(successFunc)) successFunc(data.data);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.updateTestCaseDate = function (req, colDate, successFunc, errorFunc) {
        var id = req.dbInfo.test_case_id;
        if (id > 0) {
            //local test
            if (this.settings.fromDB == false) {
                return;
            }

            SendData({
                "url": "./Handler/TestCaseManager.asmx/UpdateTestCaseDate",
                "data": { "id": id, "colDate": colDate, "time": new Date().format('yyyy-MM-dd HH:mm:ss') }
            }, null, function (data) {
                if ($.isFunction(successFunc)) successFunc(data.data);
            }, function (data) {
                SendDataError(data, errorFunc);
            });
        }
    };
    DataHandler.prototype.getTestCase = function (id, successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            return;
        }

        SendData({
            "url": "./Handler/TestCaseManager.asmx/GetTestCase",
            "data": { "id": id, "updateReadLastTime": true }
        }, null, function (data) {
            var tc = data.data;
            _preHandle2(tc);
            if ($.isFunction(successFunc)) successFunc(tc);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };
    DataHandler.prototype.getPackage = function (id, successFunc, errorFunc) {
        //local test
        if (this.settings.fromDB == false) {
            return;
        }

        SendData({
            "url": "./Handler/PackageManager.asmx/GetTestPackage",
            "data": { "id": id }
        }, null, function (data) {
            var tp = data.data;
            if ($.isFunction(successFunc)) successFunc(tp);
        }, function (data) {
            SendDataError(data, errorFunc);
        });
    };


    $.dataHandler = function (options) {
        return new DataHandler(options);
    };

    var SendDataError = function(data, errorFunc){
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
