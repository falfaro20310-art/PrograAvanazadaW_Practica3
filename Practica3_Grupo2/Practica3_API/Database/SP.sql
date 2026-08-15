USE [PracticaS13]
GO

CREATE PROCEDURE sp_ListarProductos
AS
BEGIN
    SELECT 
        Id_Compra AS CodigoCompra,
        Descripcion,
        Precio,
        Saldo,
        Estado
    FROM Principal
    ORDER BY 
        CASE WHEN Estado = 'Pendiente' THEN 0 ELSE 1 END,
        Id_Compra;
END

GO
CREATE PROCEDURE sp_ListarPendientes
AS
BEGIN
    SELECT 
        Id_Compra AS CodigoCompra,
        Descripcion,
        Precio,
        Saldo,
        Estado
    FROM Principal
    WHERE Estado = 'Pendiente'
    ORDER BY Id_Compra;
END

GO
CREATE PROCEDURE sp_ObtenerSaldoAnterior
    @CodigoCompra BIGINT
AS
BEGIN
    SELECT Saldo
    FROM Principal
    WHERE Id_Compra = @CodigoCompra;
END

GO
CREATE PROCEDURE sp_RegistrarAbono
    @CodigoCompra BIGINT,
    @Monto DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SaldoActual DECIMAL(18,5);

    SELECT @SaldoActual = Saldo
    FROM Principal
    WHERE Id_Compra = @CodigoCompra;

    IF @SaldoActual IS NULL
    BEGIN
        RAISERROR('No existe la compra indicada.', 16, 1);
        RETURN;
    END

    IF @Monto > @SaldoActual
    BEGIN
        RAISERROR('El abono no puede ser mayor al saldo pendiente.', 16, 1);
        RETURN;
    END

    INSERT INTO Abonos (Id_Compra, Monto, Fecha)
    VALUES (@CodigoCompra, @Monto, GETDATE());

    UPDATE Principal
    SET Saldo = Saldo - @Monto
    WHERE Id_Compra = @CodigoCompra;

    UPDATE Principal
    SET Estado = 'Cancelado'
    WHERE Id_Compra = @CodigoCompra AND Saldo = 0;

    SELECT Saldo, Estado
    FROM Principal
    WHERE Id_Compra = @CodigoCompra;
END