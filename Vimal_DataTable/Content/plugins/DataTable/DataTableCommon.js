var settings;
var self;
(function ($) {
    $.fn.createGrid = function (options) {
        var defaults = { Url: '/Home/GetGridData', PagerInfo: true, SearchParams: {}, RecordPerPage: 10, DataType: 'POST', Columns: [], Mode: '', FixClause: '', SortColumn: '0', SortOrder: 'asc', ExportIcon: true, ColumnSelection: true, IsAddShow: true, OnAdd: fnAdd, GrdLabels: JSON.stringify({ Show: "Showing", To: "to", Of: "of", Entries: "entries", Search: "Search", First: "first", Last: "last", Next: "next", Previous: "previous", SortAsc: "activate to sort column ascending", SortDesc: "activate to sort column descending", Add: "Add", ExportTo: "Export To ", Excel: "Excel", Pdf: "Pdf", Csv: "Csv", Word: "Word" }), DrawCallback: fn_drawCallback };
        settings = $.extend({}, defaults, options);
        self = this;
        var labels = JSON.parse(settings.GrdLabels);
        $(this).DataTable({
            "processing": true,
            "oLanguage": { "sProcessing": "", "sInfo": labels.Show + " _START_ " + labels.To + " _END_ " + labels.Of + " _TOTAL_ " + labels.Entries, "sLengthMenu": labels.Show + " _MENU_ " + labels.Entries, "sSearch": labels.Search, "sPaginate": { "first": labels.First, "last": labels.Last, "next": labels.Next, "previous": labels.Previous }, "sAria": { "sortAscending": ": " + labels.SortAsc, "sortDescending": ": " + labels.SortDesc } },
            "serverSide": true,
            "ajax": {
                "type": settings.DataType, "url": settings.Url,
                "data": {
                    'SearchParams': settings.SearchParams,
                    'mode': settings.Mode,
                    'FixClause': settings.FixClause
                },
                'dataType': 'json'
                //'async' : false
            },
            "columns": settings.Columns,
            "pageLength": settings.RecordPerPage,
            //  "bInfo": settings.PagerInfo ? true : false,
            "pagingType": "simple_numbers",
            "order": [[settings.SortColumn, '' + settings.SortOrder + '']],
            dom: 'lfrti<"toolbar">p',
            "drawCallback": function (settings) {
                if (fn_drawCallback)
                    setTimeout(function () {
                        fn_drawCallback(settings);
                    }, 1000);
            },
            initComplete: function () {
                var table = $('#' + $(this).attr("id") + '').DataTable();
                var s = '';
                var s1 = '';
                if (settings.ExportIcon) {
                    s1 += "<a onclick=\"Export(1,'" + $(this).attr("id") + "')\" data-tooltip=\"true\" title=\"" + labels.ExportTo + " " + labels.Excel + "\" class='dt-button buttons-collection buttons-colvis btn btn-white btn-primary btn-bold'><i class='fa fa-file-excel-o bigger-110 green'></i></a>\
                        <a onclick=\"Export(2,'" + $(this).attr("id") + "')\" data-tooltip=\"true\" title=\"" + labels.ExportTo + " " + labels.Pdf + "\" class='dt-button buttons-collection buttons-colvis btn btn-white btn-primary btn-bold'><i class='fa fa-file-pdf-o bigger-110 red'></i></a>\
                        <a onclick=\"Export(4,'" + $(this).attr("id") + "')\" data-tooltip=\"true\" title=\"" + labels.ExportTo + " " + labels.Word + "\" class='dt-button buttons-collection buttons-colvis btn btn-white btn-primary btn-bold'><i class='fa fa-file-word-o bigger-110 grey'></i></a>";
                }

                if (settings.IsAddShow) {
                    s += "<a href=\"javascript:void(0);\" id=\"btnAddNew\" data-tooltip=\"true\" title=\"" + labels.Add + "\" class=\"dt-button buttons-collection buttons-colvis btn btn-white btn-primary btn-bold lebel_add\" ><i class=\"fa fa-plus bigger-110 green\"></i></a>"
                }
                if (settings.ColumnSelection) {
                    s1 += '&nbsp;<div class="dropup" style="display: inline-block;" ><a data-tooltip=\"true\" title=\'' + labels.show_hide_column + '\' id="dLabel1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" class="dt-button buttons-collection buttons-colvis btn btn-white btn-primary btn-bold d-block " data-uib-tooltip=\'' + labels.show_hide_column + '\' ><i class="fa fa-search bigger-110 blue"></i> </a><ul style="max-height:520%;overflow-y:auto;z-index:999;position: absolute;" class="dropdown-menu" aria-labelledby="dLabel1"><iframe id=\"exportFram\" name=\"exportFram\" width=\"0\" height=\"0\" src=\"/ExportData.aspx\" style=\"visibility: hidden;\"></iframe><li class=\"mt-checkbox-list\">';
                    for (var i = 0; i < table.columns().count(); i++) {

                        var c = table.column(i).visible() == false ? "" : "checked";
                        if (!($(table.column(i).header()).text().match("Id"))) {
                            s1 += "<label class=\"mt-checkbox mt-checkbox-outline\">" + $(table.column(i).header()).text() + "<input type=\"checkbox\" " + c + " onchange=\"ShowHideColumn(this,'" + $(this).attr("id") + "');\" coldata='" + $(table.column(i).header()).text() + "' /><span></span></label>";
                        }
                    }
                    s1 += "</li></ul></div>";
                }
                $("#" + $(this).attr("id") + "").parent().find("div.toolbar").html(s1);
                $("#" + $(this).attr("id") + "").parent().find("div.dataTables_filter").append(s);
                $('[data-tooltip="true"]').tooltip({
                    container: 'body',
                    placement: 'top'
                });
                $("#btnAddNew").on("click", function () {
                    settings.OnAdd();
                })
                $("#" + $(this).attr("id") + " tr th").each(function (e, index) {
                    $("#" + $(self).attr("id") + " tr td:nth-child(" + (e + 1) + ")").attr('title', $(this).text());;
                });
                if (settings.DrawCallback)
                    settings.DrawCallback(settings);
            }
        });
        $('div.dataTables_filter').addClass('xs-mb-5');
        $('div.dataTables_filter input').addClass('form-control input-sm input-small input-inline');
        $('div.dataTables_length select').addClass('form-control input-sm input-xsmall input-inline');
        $("#" + $(this).attr("id") + "").on('order.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 1000);
            }
        });
        $("#" + $(this).attr("id") + "").on('page.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 1000);
            }
        });
        $("#" + $(this).attr("id") + "").on('search.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 1000);            
            }
        });

    }
}(jQuery));

function HideLoading() {
    $(".dataTables_processing").hide();
}

function fn_drawCallback() {

}

function ShowHideColumn(obj, tbl) {
    var table = $('#' + tbl.trim() + '').DataTable();
    for (var i = 0; i < table.columns().count(); i++) {
        if ($(table.column(i).header()).text() == $(obj).attr("coldata")) {
            table.column(i).visible(obj.checked)
        }
    }
}

function fnAdd() {

}

function Export(obj, tbl) {
    var table = $('#' + tbl.trim() + '').DataTable();
    var type = 'excel';
    if (obj === 2)
        type = 'pdf';
    else if (obj === 4)
        type = 'word';
    var winName = 'MyWindow';
    var winURL = '/ExportData.aspx';
    var windowoption = 'resizable=yes,height=600,width=800,location=0,menubar=0,scrollbars=1';
    var params1 = jQuery('#' + tbl + '').DataTable().ajax.params();
    var params = { 'SearchParams': params1.SearchParams, 'search[value]': params1.search.value, 'order[0][column]': params1.order[0].column, 'order[0][dir]': params1.order[0].dir, 'start': "-2", 'mode': params1.mode, 'FixClause': params1.FixClause, 'type': type, 'columns': params1.columns };
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", winURL);
    form.setAttribute("target", winName);
    for (var i in params) {
        if (params.hasOwnProperty(i)) {
            var input;
            if (i === "columns") {
                input = document.createElement('input');
                input.type = 'hidden';
                input.name = 'Columns';
                var cols = "";
                for (c = 0; c < table.columns().count(); c++) {
                    if (table.column(c).visible() != false && isNaN(table.column(c).dataSrc()))
                        cols += cols == "" ? table.column(c).dataSrc() : "," + table.column(c).dataSrc();
                }
                input.value = cols;
                form.appendChild(input);
            }
            else {
                input = document.createElement('input');
                input.type = 'hidden';
                input.name = i;
                input.value = typeof params[i] == "object" ? JSON.stringify(params[i]) : params[i];
                form.appendChild(input);
            }
        }
    }
    document.body.appendChild(form);
    form.target = "exportFram";
    form.submit();
    document.body.removeChild(form);
}

function ConvertDateddMMyyyy(data) {
    if (data == null) return '';
    var r = /\/Date\(([0-9]+)\)\//gi
    var matches = data.match(r);
    if (matches == null) return '1/1/1950';
    var result = matches.toString().substring(6, 19);
    var epochMilliseconds = result.replace(
        /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
        '$1');
    var b = new Date(parseInt(epochMilliseconds));
    var c = new Date(b.toString());
    var curr_date = c.getDate();
    var curr_month = c.getMonth() + 1;
    var curr_year = c.getFullYear();
    var curr_h = c.getHours();
    var curr_m = c.getMinutes();
    var curr_s = c.getSeconds();
    var curr_offset = c.getTimezoneOffset() / 60
    //var d = curr_month.toString() + '/' + curr_date + '/' + curr_year;
    var d = curr_date + '/' + curr_month.toString() + '/' + curr_year;
    return d;
}