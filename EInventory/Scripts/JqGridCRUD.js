var lastsel;
$(function () {

    $("#grid").jqGrid({
        colNames: ['Title', 'First Name', 'Last Name'],
        colModel: [
                        { name: 'Title', index: 'Title', sortable: false, align: 'left', width: '200',
                            editable: true, edittype: 'text'

                        },
                        { name: 'FirstName', index: 'FirstName', sortable: false, align: 'left', width: '200',
                            editable: true, edittype: 'text'

                        },
                        { name: 'LastName', index: 'LastName', sortable: false, align: 'left', width: '200',
                            editable: true, edittype: 'text'

                        }

                      ],
        pager: jQuery('#pager'),
        sortname: 'FirstName',
        rowNum: 10,
        rowList: [10, 20, 25],
        sortorder: "",
        height: 225,
        viewrecords: true,
        rownumbers: true,
        caption: 'Contacts',
        imgpath: '/Content/jqGridCss/smoothness/images',
        width: 750,
        url: "/Home/GridDemoData",
        editurl: "/Home/PerformCRUDAction",
        datatype: 'json',
        mtype: 'GET',
        onCellSelect: function (rowid, iCol, aData) {


            if (rowid && rowid !== lastsel) {

                if (lastsel)
                    jQuery('#grid').jqGrid('restoreRow', lastsel);
                jQuery('#grid').jqGrid('editRow', rowid, true);
                lastsel = rowid;
            }
        }
    })
    jQuery("#grid").jqGrid('navGrid', '#pager', { edit: false, add: true, del: true, search: false, refresh: true },
            { closeOnEscape: true, reloadAfterSubmit: true, closeAfterEdit: true, left: 400, top: 300 },
            { closeOnEscape: true, reloadAfterSubmit: true, closeAfterAdd: true, left: 450, top: 300, width: 520 },
            { closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300 });

});

