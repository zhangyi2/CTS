<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ctrip.Framework.ApplicationFx.CTS.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CTS - www.ctripcorp.com</title>
    <link href="Content/Site.css" rel="stylesheet" />
    <%--<link href="Content/RequestUrl.css" rel="stylesheet" />--%>
    <script src="Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.10.4.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content-wrapper">
        <div class="navbar">
			<div class="container">
				<a class="brand" href="#">
                    CTS
					<%--<img src="../static/logo.png" class="logo" height="45" width="120">--%>
				</a>
				<div class="nav-collapse">
                    <div>
                        <ul id="nav" class="nav">
                            <li class="active">
                                <a link-to="CRedisInstanceTest.aspx?v=<%= ConfigurationManager.AppSettings["IncludeVersion"] %> href="#" target="board-iframe">CRedisTest</a>
                            </li>
                            <li>
                                <a link-to="CRedisIndex.aspx?v=<%= ConfigurationManager.AppSettings["IncludeVersion"] %> href="#" target="board-iframe">ConfigService</a>
                            </li>
                            <li>
                                <a link-to="CacheBoard.aspx?v=<%= ConfigurationManager.AppSettings["IncludeVersion"] %> href="#" target="board-iframe">CacheBoard</a>
                            </li>
                        </ul>
                    </div>
                    <ul id="auth" class="nav pull-right">
                        <li class="dropdown">
                            <a data-toggle="dropdown" class="dropdown-toggle" style="text-decoration: none;" href="#">
                                <span id="username"><%= this.CurrentUserName %></span>
                                <b class="caret"></b>
                            </a>
                            <ul class="dropdown-menu" style="display:none">
                                <li style="color: #ccc; padding-left: 15px; font-weight: bold;"><span><%= this.CurrentUserName %></span></li>
                                <li class="divider"></li>
                                <li></li>
                                <li><a  href="Logout.aspx">注销</a></li>
                            </ul>
                        </li>
                    </ul>
				</div>
			</div>
		</div>
        <div id="main-body" class="content-wrapper main-content clear-fix">
            <iframe id="board-iframe" name="board-iframe" src="CRedisInstanceTest.aspx?v=<%= ConfigurationManager.AppSettings["IncludeVersion"] %>"></iframe>
        </div>
        </div>
        <script type="text/javascript">
            var body_status = { "auth-dropdown-menu-display": "none" };
            $(document).ready(function () {
                $("#nav li a").click(function () {
                    var ele = $(this);
                    var linkTo = ele.attr("link-to");
                    if (!ele.parent(0).is(".active") && linkTo != "#" && linkTo != "") {
                        $("#nav li").removeClass("active");
                        ele.parent(0).addClass("active");
                        document.getElementById("board-iframe").src = linkTo;
                    }
                    return false;
                });
                $("#auth a.dropdown-toggle").click(function () {
                    $("#auth ul.dropdown-menu").css("display", "block");
                    setTimeout(function () {
                        body_status["auth-dropdown-menu-display"] = "";
                    }, 500)
                });

                //$("#auth ul.dropdown-menu").focusout(function () {
                //    setTimeout(AuthDropdownMenuFocusout, 500);
                //});
                $("body").click(function () {
                    AuthDropdownMenuFocusout();
                });
                $(window).resize(function () {
                    $("#board-iframe").outerHeight($(window).innerHeight());
                });

                $(window).resize();
            })
            function AuthDropdownMenuFocusout() {
                if (body_status["auth-dropdown-menu-display"] == "") {
                    $("#auth ul.dropdown-menu").css("display", "none");
                    body_status["auth-dropdown-menu-display"] = "none";
                }
            }
        </script>
    </form>
</body>
</html>
