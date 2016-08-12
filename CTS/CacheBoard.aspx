<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CacheBoard.aspx.cs" Inherits="ctrip.Framework.ApplicationFx.CTS.CacheBorad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CTS- www.ctripcorp.com</title>
    <link href="<%=GetIncludeUrl("Content/themes/base/jquery-ui-1.10.4.css")%>" rel="stylesheet" />
    <link href="<%=GetIncludeUrl("Content/Site.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=GetIncludeUrl("Content/common.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=GetIncludeUrl("Content/board.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=GetIncludeUrl("Content/bootstrap-modal.css")%>" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.10.4.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-migrate-git.min.js" type="text/javascript"></script>

    <script src="<%=GetIncludeUrl("Scripts/common.js")%>" type="text/javascript"></script>
    <script src="<%=GetIncludeUrl("Scripts/loadconfig.js")%>" type="text/javascript"></script>
</head>
<body>
    <div id="main" class="content">
        <section id="query" style="display: block;">
            <div class="content-wrapper">
            </div>
        </section>
        <section id="report" style="display: block;">
            <div class="content-wrapper">
                <div class="top-view stat">
                    <div class="top-view-container">
                    <div class="view-name">分类统计</div>
                    <table class="top-view-list">
                        <thead>
                            <tr>
                                <th>产品线</th>
                                <th title="点击排序">集群</th>
                                <th title="点击排序">实例</th>
                                <th title="点击排序">命中率</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><a href="#" title="点击查看报表">团购</a></td>
                                <td><a href="#" title="集群列表">1</a></td>
                                <td><a href="#" title="实例列表">12</a></td>
                                <td>99.18%</td>
                            </tr>
                            <tr>
                                <td><a href="#">度假</a></td>
                                <td><a href="#">14</a></td>
                                <td><a href="#">99</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                            <tr>
                                <td><a href="#">test</a></td>
                                <td><a href="#">1</a></td>
                                <td><a href="#">2</a></td>
                                <td>97.8%</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr><td colspan="4"><span class="view-action expand">展开全部</span></td></tr>
                        </tfoot>
                    </table>
                        <%--<div class="actions"><a href="#">查看全部</a></div>--%>
                    </div>
                    <div class="top-view-container">
                    <div class="view-name">异常计数</div>
                    <table class="top-view-list">
                        <thead>
                            <tr>
                                <th>集群</th>
                                <th>异常数</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><a href="#" title="点击查看报表">cluster1</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">2836</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">cluster2</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">2473</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">cluster3</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1858</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1675</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1675</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1675</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1675</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1675</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1675</a></td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert"><a href="#" title="点击查看clog">1675</a></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr><td colspan="2"><a href="#">查看全部</a></td></tr>
                        </tfoot>
                    </table>
                        <%--<div class="actions"><a href="#">查看全部</a></div>--%>
                    </div>
                </div>
                <div class="top-view alert">
                    <div class="top-view-container">
                    <div class="view-name">集群报警</div>
                    <table class="top-view-list">
                        <thead>
                            <tr>
                                <th>集群</th>
                                <th title="点击排序">内存使用率</th>
                                <th title="点击排序">延时ms</th>
                                <th title="点击排序">命中率</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><a href="#" title="点击查看报表">cluster1</a></td>
                                <td class="alert">90.25%</td>
                                <td>0.35</td>
                                <td>26.18%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">cluster2</a></td>
                                <td>48.18%</td>
                                <td>0.15</td>
                                <td class="alert">4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">cluster2</a></td>
                                <td>48.18%</td>
                                <td class="alert">912.61</td>
                                <td>4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>48.18%</td>
                                <td class="alert">912.61</td>
                                <td>4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>48.18%</td>
                                <td class="alert">912.61</td>
                                <td>4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>48.18%</td>
                                <td class="alert">912.61</td>
                                <td>4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert">48.18%</td>
                                <td>912.61</td>
                                <td>4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>48.18%</td>
                                <td class="alert">912.61</td>
                                <td>4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td class="alert">48.18%</td>
                                <td>912.61</td>
                                <td>4.7%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>48.18%</td>
                                <td>0.15</td>
                                <td class="alert">4.7%</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr><td colspan="4"><a href="#">查看全部</a></td></tr>
                        </tfoot>
                    </table>
                        <%--<div class="actions"><a href="#">查看全部</a></div>--%>
                    </div>
                    <div class="top-view-container">
                    <div class="view-name">服务器报警</div>
                    <table class="top-view-list">
                        <thead>
                            <tr>
                                <th>Server</th>
                                <th title="点击排序">使用内存</th>
                                <th title="点击排序">总内存</th>
                                <th title="点击排序">使用率</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><a href="#" title="点击查看报表">192.168.1.45</a></td>
                                <td>5758M</td>
                                <td>6144M</td>
                                <td class="alert">90.25%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>5758M</td>
                                <td>6144M</td>
                                <td class="alert">90.25%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>5758M</td>
                                <td>6144M</td>
                                <td class="alert">90.25%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>5758M</td>
                                <td>6144M</td>
                                <td class="alert">90.25%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>5758M</td>
                                <td>6144M</td>
                                <td class="alert">90.25%</td>
                            </tr>
                            <tr>
                                <td><a href="#" title="点击查看报表">test</a></td>
                                <td>5758M</td>
                                <td>6144M</td>
                                <td class="alert">90.25%</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr><td colspan="4"><a href="#">查看全部</a></td></tr>
                        </tfoot>
                    </table>
                        <%--<div class="actions"><a href="#">查看全部</a></div>--%>
                    </div>
                </div>
            </div>
        </section>
        <section id="monitor" style="display: block;">
            <div class="content-wrapper">
            </div>
        </section>
    </div>
</body>
</html>
