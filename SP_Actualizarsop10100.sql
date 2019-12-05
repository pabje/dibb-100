--Validando el procedimiento almacenado
IF OBJECT_ID('SP_ACTUALIZARSOP10100')IS NOT NULL
BEGIN
	DROP PROCEDURE SP_ACTUALIZARSOP10100
END
GO
--Creando el procedimiento almacenado
CREATE PROCEDURE SP_ACTUALIZARSOP10100(@NUMFAC CHAR(6),@SERNUM CHAR(6),@MENS VARCHAR(100) OUTPUT)
AS
	BEGIN TRY		
		IF EXISTS(SELECT *FROM sop10100 WHERE SOPTYPE = 3 AND SOPNUMBE = @SERNUM)
		BEGIN
			UPDATE sop10100 SET cstponbr = @NUMFAC WHERE soptype = 3 AND SOPNUMBE = @SERNUM			
			SET @MENS = 'LA FACTURA FUE ACTUALIZADA CORRECTAMENTE'
		END
		ELSE			
			SET @MENS = 'NO SE ENCUENTRA EN LA TABLA LA FACTURA: '
	END TRY
	BEGIN CATCH		
		SET @MENS = 'OCURRIO UN ERROR AL MODIFICAR'
	END CATCH
GO