USE LesEgais;
GO

CREATE TABLE WoodDeals (
	DealNumber nvarchar(28) PRIMARY KEY,
	SellerName nvarchar(max),
	SellerInn nvarchar(12),
	BuyerName nvarchar(max),
	BuyerInn nvarchar(12),
	DealDate date,
	WoodVolumeBuyer decimal(18, 2),
	WoodVolumeSeller decimal(18, 2)
);
GO