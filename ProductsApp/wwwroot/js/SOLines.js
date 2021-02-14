
    var columnDefs = [
        {headerName: "No", field: "line_No" },
        {headerName: "Description", field: "description"},
        {headerName: "Quantity", field: "quantity"},
        {headerName: "Price", field: "unit_Price"},
    ];

    var gridOptions = {
        columnDefs: columnDefs,
        rowSelection: 'multiple',
        domLayout: 'autoHeight',

        defaultColDef: {
        editable: false,
            enableRowGroup: true,
            enablePivot: true,
            enableValue: true,
            sortable: true,
            resizable: true,
            filter: true,
            flex: 1
        },
        suppressRowClickSelection: true,
        pagination: true,
        getRowHeight: function (params) {
            return 35;
        },
        rowModelType: 'infinite',
       // animateRows: true,
        paginationPageSize: 3,
        cacheOverflowSize: 2,
      // maxConcurrentDatasourceRequests: 2,
       // infiniteInitialRowCount: 1,
        maxBlocksInCache: 2,
        cacheBlockSize: 3,
        enableColResize: true,
       // suppressMenuHide: true,

    };
    var dataSourceTest = '';
    document.addEventListener('DOMContentLoaded', function () {
        var gridDiv = document.querySelector('#SOLineGrid');
        new agGrid.Grid(gridDiv, gridOptions);
        dataSourceTest = {
        //  rowCount: null, // behave as infinite scroll
        getRows: function (params) {
                var model = {
        No: document.getElementById('No').value,
                };
                loadPaginatedGridWithDelay('/SalesOrders/DetailsAjax', 'GET', model, gridOptions, params);

            },
            getRowHeight: function (params) {
                return 35;
            },
        };
        gridOptions.api.showNoRowsOverlay();
        gridOptions.api.setDatasource(dataSourceTest);
    });


    function refresh(e) {
        gridOptions.api.setDatasource(dataSourceTest);
    }
