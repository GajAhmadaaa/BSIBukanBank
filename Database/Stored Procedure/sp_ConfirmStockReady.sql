-- =============================================
-- Author:		<Sales System Development Team>
-- Create date: <Current Date>
-- Description:	Stored Procedure untuk mengkonfirmasi kesiapan stok dan mengubah status LOI menjadi ReadyForAgreement
-- =============================================
CREATE PROCEDURE [dbo].[sp_ConfirmStockReady]
	@LOIID int
AS
BEGIN
	SET NOCOUNT ON;

	-- Update LOI status to ReadyForAgreement
	UPDATE LetterOfIntent 
	SET Status = 'ReadyForAgreement'
	WHERE LOIID = @LOIID AND Status = 'PendingStock'
	
	-- Return number of rows affected
	SELECT @@ROWCOUNT AS RowsAffected
END