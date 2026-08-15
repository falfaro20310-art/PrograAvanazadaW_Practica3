$(document).ready(function () {
    $("#CodigoCompra").change(function () {
        var codigoCompra = $(this).val();
        if (codigoCompra === "") {
            $("#SaldoAnterior").val("");
            return;
        }

        $.ajax({
            url: "/Registro/ObtenerSaldo",
            type: "GET",
            data: { codigoCompra: codigoCompra },
            success: function (saldo) {
                $("#SaldoAnterior").val(saldo.toFixed(2));
            },
            error: function () {
                alert("No se pudo obtener el saldo de la compra seleccionada.");
                $("#SaldoAnterior").val("");
            }
        });
    });
    $("form").submit(function (e) {
        var saldoAnterior = parseFloat($("#SaldoAnterior").val());
        var abono = parseFloat($("#Abono").val());

        if (isNaN(abono) || abono <= 0) {
            alert("Debe ingresar un monto de abono válido.");
            e.preventDefault();
            return false;
        }

        if (abono > saldoAnterior) {
            alert("El abono no puede ser mayor al saldo anterior.");
            e.preventDefault();
            return false;
        }
    });

});