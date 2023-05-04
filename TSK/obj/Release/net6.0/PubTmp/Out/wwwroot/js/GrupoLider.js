function button1Clicked() {
    var popup = $("#popup").dxPopup("instance");
    $.ajax({
        type: "GET",
        url: "@Url.Action("GrupoUsuarioPopup", "ReporteDetalle")", // Cambia esta url por la url de tu vista
        success: function (data) {
            popup.content(data);
            popup.show();
        }
    });
}

function button2Clicked() {
    var popup = $("#popup").dxPopup("instance");
    popup.show();
}

function button3Clicked() {
    var popup = $("#popup").dxPopup("instance");
    popup.show();
}

function button4Clicked() {
    var popup = $("#popup").dxPopup("instance");
    popup.show();
}
