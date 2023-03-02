USE LesEgais;
GO

CREATE TABLE WoodDeals (
	DealNumber nvarchar(28) NOT NULL,
	SellerName nvarchar(max) NOT NULL,
	SellerInn nvarchar(12),
	BuyerName nvarchar(max) NOT NULL,
	BuyerInn nvarchar(12),
	DealDate date NOT NULL,
	WoodVolumeBuyer decimal(18, 2) NOT NULL,
	WoodVolumeSeller decimal(18, 2) NOT NULL,
	PRIMARY KEY CLUSTERED (DealNumber)
);
GO