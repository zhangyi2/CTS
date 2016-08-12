(function ($, window, undefined) {
    $.ddUpload = $.extend({}, {
        addLog: function (id, status, str, options) {
            var d = new Date();
            var li = $('<li />', { 'class': 'upload-' + status });
            var message = '[' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds() + '] ';
            message += str;
            li.html(message);
            $(id).prepend(li);
            if (options && options.panelId) {
                var panel = $("#" + options.panelId);
                if (!panel.is(":visible")) panel.css("display", "");
            }
        },
        addFile: function (id, i, file, delFunc) {
            var i = $(id).attr('file-counter');
            if (!i || i=="0") {
                $(id).empty();
                i = 0;
            }
            var template = $('<div id="temp-file' + i + '">' +
                               '<span class="temp-file-id">#' + i + '</span> - ' + file.name + ' <span class="temp-file-size">(' + $.ddUpload.humanizeSize(file.size) + ')</span> - Status: <span class="temp-file-status">Waiting to upload</span>' +
                           '</div>');
            if ($.isFunction(delFunc)) {
                template.append($('<span class="delete"></span>').click(delFunc));
            }
            template.append('<div class="progress progress-striped active">' +
                                '<div class="progress-bar" role="progressbar" style="width: 0%;">' +
                                    '<span class="sr-only">0% Complete</span>' +
                                '</div>' +
                            '</div>');
            $(id).attr('file-counter', ++i);
            $(id).prepend(template);
        },
        delFile: function (id, pos){
            var i = $(id).attr('file-counter');
            if (!i || i == "0") {
                $(id).empty();
                i = 0;
                return;
            }
            if ($.isNumeric(pos) && pos >= 0 && $("#temp-file" + pos).length > 0) {
                $("#temp-file" + pos).remove();
                $(id).attr('file-counter', --i);
                if (i == 0) $(id).html("<span class=\"upload-note\">No Files have been selected/droped yet...</span>");
                else {
                    $.each($(id + " div[id^=temp-file]"), function (index, item) {
                        var fid = parseInt($(item).attr("id").substr(9));
                        if (fid > pos) {
                            $(item).attr("id", "temp-file" + --fid);
                            $(item).find(".temp-file-id").text("#" + fid);
                        }
                    })
                }
            }
        },
        updateFileStatus: function (i, status, message) {
            $('#temp-file' + i).find('span.temp-file-status').html(message).addClass('temp-file-status-' + status);
        },
        updateFileProgress: function (i, percent) {
            $('#temp-file' + i).find('div.progress-bar').width(percent);
            $('#temp-file' + i).find('span.sr-only').html(percent + ' Complete');
        },
        humanizeSize: function (size) {
            var i = Math.floor(Math.log(size) / Math.log(1024));
            return (size / Math.pow(1024, i)).toFixed(2) * 1 + ' ' + ['B', 'kB', 'MB', 'GB', 'TB'][i];
        }
    }, $.ddUpload);
})(jQuery, this);

