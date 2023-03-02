USE LesEgais;
GO

CREATE PROCEDURE [dbo].[sp_UpsertDeal]
	@dealNumber nvarchar(28),
	@sellerName nvarchar(max),
	@sellerInn nvarchar(12),
	@buyerName nvarchar(max),
	@buyerInn nvarchar(12),
	@dealDate date,
	@woodVolumeBuyer decimal(18, 2),
	@woodVolumeSeller decimal(18, 2)
AS
	BEGIN TRANSACTION;

	UPDATE WoodDeals WITH (UPDLOCK, SERIALIZABLE)
	SET SellerName = @sellerName, SellerInn = @sellerInn, BuyerName = @buyerName, BuyerInn = @buyerInn,
		DealDate = @dealDate, WoodVolumeBuyer = @woodVolumeBuyer, WoodVolumeSeller = @woodVolumeSeller
	WHERE DealNumber = @dealNumber;

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO WoodDeals (DealNumber, SellerName, SellerInn, BuyerName, BuyerInn, DealDate, WoodVolumeBuyer, WoodVolumeSeller)
		VALUES (@dealNumber, @sellerName, @sellerInn, @buyerName, @buyerInn, @dealDate, @woodVolumeBuyer, @woodVolumeSeller)
	END

	COMMIT TRANSACTION;
GO