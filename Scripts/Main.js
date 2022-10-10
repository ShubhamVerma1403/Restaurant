

$("#btnItem").click(function () {
    $("#audit-content").hide();
    
    $("#item-content").toggle();
});

$("#btnAudit").click(function () {
    $("#item-content").hide();
    
    $("#audit-content").toggle();

    $("#audit-content").kendoGrid({
        height: 530,
        toolbar: ["search"],
        columns: [
            { field: "FirstName" },
            { field: "OrderId" },
            { field: "ItemName" },
            { field: "ActionType" },
            { field: "FieldName" },
            { field: "OldValue" },
            { field: "NewValue" },
            { field: "AccessedDTTM" }
        ],
        dataSource: {
            type: "aspnetmvc-ajax",
            transport: {
                idField: "ID",
                read: {
                    url: "/api/Values/GetAuditDetail",
                    type: "GET"
                }
            },
            schema: {
                data: "Data",
                model: {
                    id: "ID",
                    fields: {
                        ID: { type: "number" },
                        FirstName: { type: "string" },
                        OrderId: { type: "number" },
                        ItemName: { type: "string" },
                        ActionType: { type: "string" },
                        FieldName: { type: "string" },
                        OldValue: { type: "string" },
                        NewValue: { type: "string" },
                        AccessedDTTM: { type: "string" }
                    }
                }
            },
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            serverGrouping: true,
            serverAggregates: true,
        },
        pageable: true,
        navigatable: true
    })
});


