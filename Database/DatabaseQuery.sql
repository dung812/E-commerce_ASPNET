-- Web bán quần áo nam
USE master
GO
IF EXISTS(SELECT NAME FROM sysdatabases WHERE NAME = 'menfashion')
BEGIN
	DROP DATABASE [menfashion]
END
GO
CREATE DATABASE [menfashion] 
GO
USE [menfashion]
GO
-- Tạo bảng
CREATE TABLE [dbo].[Product](
	[productId] INT IDENTITY PRIMARY KEY([productId]),
	[productName] NVARCHAR(250) NOT NULL,
	[image]	VARCHAR(2000),
	[price] INT DEFAULT(0),
	[discount] INT DEFAULT(0),
	[description] NVARCHAR(2000),
	[quanlity] INT DEFAULT(0),
	[brand] NVARCHAR(250) DEFAULT('No brand'),
	[dateCreate] DATETIME DEFAULT(GETDATE()),
	[status] BIT,
	[categoryId] INT NOT NULL,
	[userName] VARCHAR(50) NOT NULL 
)
GO
CREATE TABLE [dbo].[ProductCategory](
	[categoryId] INT IDENTITY PRIMARY KEY([categoryId]),
	[categoryName] NVARCHAR(250) NOT NULL,
	[note] nvarchar(128),
)
GO
CREATE TABLE [dbo].[Member](
	[userName]	VARCHAR(50) PRIMARY KEY([userName]),
	[password]	VARCHAR(50) NOT NULL,
	[firstName] NVARCHAR(250),
	[lastName]	NVARCHAR(250),
	[email]		VARCHAR(50),
	[birthday]	DATE,
	[gender] bit,
	[phone]		VARCHAR(20),
	[RegisteredDate] DATETIME DEFAULT(GETDATE()),
	[address] NVARCHAR(250),
	[avatar] NVARCHAR(2000) DEFAULT('avatar.jpg'),
	[status] BIT,
	[roleId] INT NOT NULL
)
GO
CREATE TABLE [dbo].[Role](
	[roleId] INT IDENTITY PRIMARY KEY([roleId]),
	[roleName] NVARCHAR(50) NOT NULL,
	[note] nvarchar(130)
)
GO
CREATE TABLE [dbo].[Customer](
	[customerId] INT IDENTITY PRIMARY KEY([customerId]),
	[firstName] NVARCHAR(250),
	[lastName] NVARCHAR(250) NOT NULL,
	[email]		VARCHAR(50) NOT NULL,
	[phone] VARCHAR(20),
 	[address] NVARCHAR(250),
)
GO
CREATE TABLE [dbo].[Invoince](
	[invoinceNo] VARCHAR(50) PRIMARY KEY([invoinceNo]),
	[dateOrder] DATETIME DEFAULT(GETDATE()),
	[status] BIT,
	[deliveryStatus] BIT,
	[deliveryDate] DATETIME,
	[totalMoney] INT NOT NULL,
	[userName] VARCHAR(50),
	[customerId] INT	
)
GO
CREATE TABLE [dbo].[InvoinceDetail](
	[invoinceNo] VARCHAR(50) PRIMARY KEY([invoinceNo],[productId]),
	[productId] INT,
	[quanlityProduct] INT DEFAULT(0),
	[unitPrice] INT, -- Lưu thông tin giá bán vì giá có thể bị thay đổi
	[totalPrice] INT DEFAULT(0),
	[totalDiscount] INT DEFAULT(0),
)
GO
CREATE TABLE [dbo].[Article](
	[articleId] INT IDENTITY PRIMARY KEY([articleId]),
	[title] NVARCHAR(250) NOT NULL,
	[shortDescription] NVARCHAR(2000),
	[image] NVARCHAR(2000),
	[publicDate] DATETIME DEFAULT(GETDATE()),
	[content] NVARCHAR(max),
	[status] BIT,
	[userName] VARCHAR(50) NOT NULL,
	[categoryId] INT NOT NULL
)
GO
CREATE TABLE [dbo].[Contact](
	[id] INT IDENTITY PRIMARY KEY([id]),
	[dateContact] DATETIME DEFAULT(GETDATE()),
	[name] NVARCHAR(250),
	[email] VARCHAR(50) NOT NULL,
	[message] NVARCHAR(2000)
)
GO
CREATE TABLE [dbo].[Slide](
	[id] INT IDENTITY PRIMARY KEY([id]),
	[dateCreate] DATETIME DEFAULT(GETDATE()),
	[name] NVARCHAR(50),
	[description] NVARCHAR(250),
	[url] VARCHAR(50),
	[status] BIT 
)
GO
-- Tạo khóa ngoại
ALTER TABLE  [dbo].[Product] ADD CONSTRAINT FK_PRODUCT_MEMBER FOREIGN KEY([userName]) REFERENCES dbo.[Member]([userName])
ALTER TABLE  [dbo].[Product] ADD CONSTRAINT FK_PRODUCT_CATEGORY FOREIGN KEY(categoryId) REFERENCES dbo.[productCategory]([categoryId])
ALTER TABLE  [dbo].[Member]  ADD CONSTRAINT FK_MEMBER_ROLE FOREIGN KEY([roleId]) REFERENCES dbo.[Role]([roleId])
ALTER TABLE  [dbo].[Article] ADD CONSTRAINT FK_POST_MEMBER FOREIGN KEY([userName]) REFERENCES dbo.[Member] ([userName])
ALTER TABLE  [dbo].[Article] ADD CONSTRAINT FK_POST_CATEGORY FOREIGN KEY([categoryId]) REFERENCES dbo.[productCategory]([categoryId])
ALTER TABLE  [dbo].[Invoince] ADD CONSTRAINT FK_INVOINCE_MEMBER FOREIGN KEY([userName]) REFERENCES dbo.[Member] ([userName])
ALTER TABLE  [dbo].[Invoince] ADD CONSTRAINT FK_INVOINCE_CUSTOMER FOREIGN KEY([customerId]) REFERENCES dbo.[Customer]([customerId])
ALTER TABLE  [dbo].[InvoinceDetail] ADD CONSTRAINT FK_INVOINCEDETAIL_INVOINCE FOREIGN KEY([invoinceNo]) REFERENCES dbo.Invoince([invoinceNo])
ALTER TABLE  [dbo].[InvoinceDetail] ADD CONSTRAINT FK_INVOINCEDETAIL_PRODUCT FOREIGN KEY([productId]) REFERENCES dbo.Product([productId])

--Insert dữ liệu
-- [category]
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES (N'T-shirt')
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES (N'Shirt')
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES (N'Jacket')
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES (N'Short')
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES (N'Pant')
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES (N'Shoes')
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES ('Accessories')
INSERT INTO [dbo].[ProductCategory] ( [categoryName]) VALUES (N'Grooming')

-- [Role]
INSERT INTO [dbo].[Role] ([roleName]) VALUES (N'Manager')
INSERT INTO [dbo].[Role] ([roleName]) VALUES (N'Employee')
INSERT INTO [dbo].[Role] ([roleName]) VALUES (N'Customer')
-- [User]
INSERT INTO [dbo].[Member] ([userName], [password], [firstName], [lastName], [email], [birthday], [identityNumber], [phone], [dateOfJoin], [address], [avatar], [status], [roleId]) VALUES (N'admin', N'21232f297a57a5a743894a0e4a801fc3', N'Nguyễn', N'Dũng', N'knguyn16@gmail.com', CAST(N'2001-12-08' AS Date), N'215516721', N'0706437167', CAST(N'2021-08-15T22:33:25.773' AS DateTime), N'Quy Nhơn, Bình Định', N'~/Content/img/avatar/avatar.jpg', 1, 1)
-- [Product]
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Reebok Club C Revenge Vintage Sneaker', N'~/Content/img/product/59752436_010_d.jpg', 80, 40, N'Retro-inspired sneakers by Reebok, the Club C Revenge GV7609 sneaker features a leather & suede upper with lightweight padding and cushioning throughout. Fitted with a lace closure to the front and finished with a textured outsole.', 50, N'Reebok', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 6, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Mad Rabbit Tattoo Balm', N'~/Content/img/product/62746706_000_b.jpg', 16, 0, N'Revitalize, replenish and preserve your body art with this tattoo care balm from Mad Rabbit. Made with seven all-natural, clean ingredients like shea butter, sweet almond oil and cocoa butter to nourish the skin and help with tissue regeneration, in a non-greasy or oily finish. Effective on both new and old tattoos, and perfect for black, grey and color designs.', 50, N'Mad Rabbit', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 8, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Happy Nuts Comfort Cream', N'~/Content/img/product/60727732_000_b.jpg', 14, 0, N'Keep it all fresh and comfy with this lightweight cream from Happy Nuts. Odor-neutralizing formula that goes on smooth and dries to a silky powder finish. Made with natural ingredients. Talc free.', 50, N'Happy Nuts', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 8, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Ballsy All The Feels Organic Personal Lubricant', N'~/Content/img/product/63566228_000_b.jpg', 12, 0, N'By using only the highest-grade extracts including aloe, quinoa, hemp, flax and oat, All The Feels is enriched with vitamins and antioxidants that moisturize and protect your most intimate areas while leaving your skin feeling soft and never sticky so all you have to worry about is having fun.', 50, N'Ballsy', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 8, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'BDG Ribbed Boxer Brief', N'~/Content/img/product/61460689_044_b.jpg', 18, 0, N'Fitted ribbed knit boxer briefs by the essential BDG label. Features a classic look and stretch fabrication with elasticated waistband. Finished with logo text at the leg. Only at Urban Outfitters.', 50, N'BDG', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 7, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Tie-Dye Corduroy Bucket Hat', N'~/Content/img/product/60803079_095_b.jpg', 19, 40, N'Corduroy bucket hat from Urban Outfitters with a classic look. Tie-dye wash effect hat with a flat top and stitched all-around brim.', 50, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 7, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Ray-Ban Wayfarer Sunglasses', N'~/Content/img/product/60627494_001_b.jpg', 154, 0, N'Classic sunglasses by Ray-Ban in the brand’s signature Wayfarer silhouette. Features a plastic frame with molded nose bridge and finished with tinted UV-blocking lenses.', 50, N'Ray-Ban', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 7, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Fjallraven High Coast Pocket Pack Sling Bag', N'~/Content/img/product/59670893_072_b.jpg', 35, 10, N'Essential sling bag by Fjallraven with a zip closure and ideal for stowing the everyday essentials while on the go! Topped with an additional pocket to the front. Finished with an adjustable webbing shoulder strap.', 50, N'Fjallraven', CAST(N'2021-08-15T22:33:25.787' AS DateTime), 1, 7, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Nike Air Tech Mini Crossbody Pouch', N'~/Content/img/product/60326949_001_b.jpg', 25, 0, N'Stow the essential in this zip pouch by Nike. Features organizational pockets & topped with an outer woven label. Finished with an adjustable webbing shoulder strap.', 50, N'Nike', CAST(N'2021-08-15T22:33:25.787' AS DateTime), 1, 7, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Koa Anti-Pollution SPF 45+ Tinted Sunscreen', N'~/Content/img/product/60056637_000_b.jpg', 27, 0, N'Super-lightweight, mineral broad-spectrum daily sunscreen by Koa featuring proprietary technology to fight pollution-induced free radicals.', 50, N'Koa', CAST(N'2021-08-15T22:33:25.787' AS DateTime), 1, 8, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'BDG Bandana Print Volley Short', N'~/Content/img/product/61819884_040_b.jpg', 49, 10, N'Printed paisley pattern shorts by BDG with pockets at the sides & back. Fitted with an elasticated waistband & adjustable tie closure. Only at Urban Outfitters.', 50, N'BDG', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 4, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Recycled Cotton 5” Lounge Short', N'~/Content/img/product/59345256_083_b.jpg', 29, 0, N'Everyday sport short by Urban Outfitters with a recycled cotton blend fabrication. Features an elasticated waistband, pockets at the waist & patch pocket to the back. Cut with a 5” inseam and finished with notched hems.', 50, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 4, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Lucien 5" Washed Awning Stripe Sweatshort', N'~/Content/img/product/61109468_041_b.jpg', 39, 0, N'Washed cotton-blend shorts by Urban Outfitters with an allover stripe pattern. Knit sweat shorts featuring pockets to the sides and back, fitted with an elastic waistband and adjustable drawcord.', 50, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 4, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Nike Trail Flex Stride Short', N'~/Content/img/product/58981671_023_b.jpg', 65, 10, N'Athletic shorts by Nike with a pull-on style & adjustable tie at the waistband. Finished with pockets at the sides and back.', 50, N'Nike', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 4, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Corduroy Volley Short', N'~/Content/img/product/60156387_045_b.jpg', 54, 30, N'Classic volley short by Urban Outfitters with a cotton cord fabrication. Fitted with an elasticated waistband with adjustable tie accent. Includes pockets to the sides & a patch pocket at the back. Cut with a 5” inseam.', 50, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 4, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Crocs Classic Clog', N'~/Content/img/product/54296074_031_b.jpg', 50, 0, N'Original clogs from Crocs constructed from Croslite synthetic material. Water friendly, non-marking and lightweight, these clogs offer ventilation ports throughout the upper and padded toe and footbed for prime comfort levels. Finished with branded heel strap for a secure fit.', 50, N'Crocs', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 6, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Teva Original Universal Urban Sandal', N'~/Content/img/product/48957484_001_b.jpg', 50, 0, N'Classically comfy Universal sandal from the experts at Teva. Cushy, molded footbed offers extra arch support while adjustable faded nylon straps provide a secure fit. Finished with a textured bottom for optimal traction. In 1984 a Grand Canyon river guide created an invitation for adventure by fusing a velcro watch strap with a flip flop - from that invention came the Teva Original, a sandal created to "strap in and go" for the spontaneous + adventurous with a go-anywhere and do-anything mentality.', 50, N'Teva', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 6, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'New Balance 574 Sneaker', N'~/Content/img/product/59468900_015_b.jpg', 80, 70, N'Core classic 574 sneaker by New Balance in a lightweight construction with suede and mesh upper. ENCAP midsole technology provides support while the EVA foam midsole provides cushioning. Finished on a lugged rubber outsole.', 50, N'New Balance', CAST(N'2021-08-15T22:33:25.783' AS DateTime), 1, 6, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Fatboy Sea Salt Pomade', N'~/Content/img/product/43817436_000_b.jpg', 21, 10, N'Beachy waves without the crunch thanks to this innovative sea salt pomade from Fatboy. Provides flexibility with the texture + finish of your favorite sea salt sprays, while hydrating with a blend of coconut oil and shea butter.', 50, N'Fatboy', CAST(N'2021-08-15T22:33:25.787' AS DateTime), 1, 8, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'BRAVO SIERRA Lip Balm', N'~/Content/img/product/59361063_000_b.jpg', 6, 0, N'Protect and repair lips with this non-toxic balm enriched with murumuru butter – the vegetable-based alternative to silicones, rich in omega 3-6-9 fatty acids. Lightweight and non-greasy with a matte finish. Vegan and cruelty-free with no fragrance, flavor, parabens, silicones, sulfates, phthalates, PEGs or phenoxyethanol. Verified by the Environmental Working Group.', 50, N'BRAVO SIERRA', CAST(N'2021-08-15T22:33:25.787' AS DateTime), 1, 8, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'The North Face Black Box Tee', N'~/Content/img/product/59454488_030_b.jpg', 40, 0, N'Graphic tee by The North Face cut in a standard fit. 100% cotton t-shirt with short sleeves & a ribbed crew neck. Topped with a logo patch accent at the sleeve.', 100, N'The North Face', CAST(N'2021-08-15T22:33:25.773' AS DateTime), 1, 1, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Patagonia P-6 Logo Organic Cotton Tee', N'~/Content/img/product/60451887_081_b.jpg', 36, 20, N'Organic cotton t-shirt by Patagonia cut in a standard fit with short sleeves and crew neck. Topped with logo detailing printed at the left chest & to the back.', 100, N'Patagonia', CAST(N'2021-08-15T22:33:25.773' AS DateTime), 1, 1, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Hanes Beefy T X Rob Engvall Graphic Tee', N'~/Content/img/product/62412523_066_b.jpg', 36, 20, N'Short sleeve Hanes tee from the Artist Collection topped with Rob Engvall art prints to the front and back. Crafted from 100% cotton and cut in a standard fit with a ribbed knit crew neck. Only at Urban Outfitters.', 100, N'Hanes', CAST(N'2021-08-15T22:33:25.773' AS DateTime), 1, 1, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'BDG Waffle Check Overshirt', N'~/Content/img/product/61794277_053_b.jpg', 69, 0, N'Long sleeve button-down shirt by BDG. Features an allover waffle check pattern. Crafted from 100% cotton in a standard fit with flap pockets at the chest and a tab collar. Only at Urban Outfitters.', 100, N'BDG', CAST(N'2021-08-15T22:33:25.773' AS DateTime), 1, 2, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Standard Cloth Waffle Stitch Polo Shirt', N'~/Content/img/product/61633210_021_d.jpg', 69, 20, N'Knit polo shirt by Standard Cloth with a button placket to the front and striped tipping at the tab collar. Ribbed fabrication crafted from 100% cotton with short sleeves. Only at Urban Outfitters.', 100, N'Standard Cloth', CAST(N'2021-08-15T22:33:25.773' AS DateTime), 1, 2, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Standard Cloth Foundation Jogger Pant', N'~/Content/img/product/58201609_001_b.jpg', 39, 5, N'Wear-everywhere joggers from Standard Cloth, cut in a slim fit. Featuring a thick elastic waistband with adjustable drawstring, front slip pockets and tonally embroidered icon at the left hip. Finished with a single back patch pocket and rib-knit ankle cuffs.', 50, N'Standard Cloth', CAST(N'2021-08-15T22:33:25.773' AS DateTime), 1, 5, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Wide Wale Corduroy Beach Pant', N'~/Content/img/product/58916743_055_b.jpg', 59, 40, N'Pull-on cord pants from Urban Outfitters. Beachy silhouette offers a wide leg with cropped hem, drawstring closure at elastic waistband and 3-pocket styling.', 50, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.777' AS DateTime), 1, 5, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Grateful Dead Dancing Bears Sweatpant', N'~/Content/img/product/60770393_001_b.jpg', 69, 30, N'Grateful Dead sweatpants with a classic cotton poly fabrication cut in a relaxed fit & topped with a dancing bears motif. Fitted with an elasticated waistband and ankle cuffs.', 50, N'Washed Black', CAST(N'2021-08-15T22:33:25.777' AS DateTime), 1, 5, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Nike Challenger OG Sneaker', N'~/Content/img/product/58879487_001_b.jpg', 90, 0, N'The classic athletic sneaker by Nike with a low-profile upper and lace-up front. Features a leather & textile upper with essential Nike swoosh detailing to the sides. Finished with a grippy rubber outsole.', 50, N'Nike', CAST(N'2021-08-15T22:33:25.777' AS DateTime), 1, 6, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Reebok UO Exclusive Club C 85 Sneaker', N'~/Content/img/product/48679617_010_b.jpg', 70, 0, N'Amp up your sneaker style with this Reebok rendition of the classic Club C DV3894 kick. Low-cut design with a soft leather upper, die-cut EVA midsole and molded sockliner for cushioned support. Features a timeless Reebok window box logo at quarter panel and ultra-padded tongue. Set on a high-abrasion rubber outsole. Get it only at UO!', 50, N'Reebok', CAST(N'2021-08-15T22:33:25.777' AS DateTime), 1, 6, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Dr. Martens 2976 Bex Chelsea Boots', N'~/Content/img/product/56996697_001_b.jpg', 160, 0, N'Classic Chealsea style boots by Dr. Martens featuring a smooth leather upper with elasticated side panel accents set atop a bold lugged sole. Includes a pull tab at the heel and finished with contrast outstitch detailing.', 50, N'Dr. Martens', CAST(N'2021-08-15T22:33:25.777' AS DateTime), 1, 6, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Quarter-Zip Polo Shirt', N'~/Content/img/product/61165650_030_d.jpg', 49, 20, N'Sporty stripe polo style shirt with a 100% cotton shirt. Short sleeve style in a standard fit with a partial zip placket to the front and tab collar.', 100, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.777' AS DateTime), 1, 2, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Without Walls Gauze Stripe Shirt', N'~/Content/img/product/60891710_009_d.jpg', 49, 0, N'Stripe pattern shirt by Without Walls with a cotton fabrication. Short sleeve style with a button-down front, notched collar and left chest pocket. Only at Urban Outfitters.', 100, N'Without Walls', CAST(N'2021-08-15T22:33:25.777' AS DateTime), 1, 2, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Ecru Stripe Grandpa Collar Shirt', N'~/Content/img/product/73715476_011_b.jpg', 69, 0, N'Mix-and-match striped shirt, made from a soft and textured fabrication. With a grandpa-style collar, a button placket, drop shoulders, long sleeves and buttoned cuffs. Finished with a patch pocket to chest with embroidered motif.', 100, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 2, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Nike Sportswear M2Z Tee', N'~/Content/img/product/60337201_011_b.jpg', 40, 0, N'T-shirt by Nike topped with a graphic to the front. Crafted from 100% cotton in a standard fit with short sleeves & a ribbed knit crew neck.', 100, N'Nike', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 1, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'ULTRA GAME Los Angeles Lakers Vintage Collegiate Text Tee', N'~/Content/img/product/61322335_001_b.jpg', 39, 0, N'Los Angeles Lakers tee by ULTRA GAME topped with a vintage look graphic printed to the front. Short sleeve cotton tee cut in a standard fit with classic crew neck.', 100, N'ULTRA GAME', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 1, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Hanes Beefy T X Rob Engvall Graphic Tee', N'~/Content/img/product/62412473_010_b.jpg', 35, 0, N'Short sleeve Hanes tee from the Artist Collection topped with Rob Engvall art prints to the front and back. Crafted from 100% cotton and cut in a standard fit with a ribbed knit crew neck. Only at Urban Outfitters.', 100, N'Hanes', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 1, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Big Corduroy Work Shirt', N'~/Content/img/product/55150924_033_b.jpg', 69, 10, N'Big corduroy work shirt from Urban Outfitters. Cut in a slightly oversized silhouette, this cotton cord shirt offers a full-length button-front closure, pointed collar and single patch pocket at the chest. Complete with long sleeves with adjustable button cuffs, yoked back and a split, rounded hemline.', 100, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 3, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Urban Renewal Remnants Upcycled Quilt Applique Linen Chore Coat', N'~/Content/img/product/62337837_012_b.jpg', 149, 10, N'Chore coat topped with upcycled quilt patch accents at the pocket and back. Features a button placket to the front & tab collar. Each is unique & will vary from what is pictured.', 100, N'Urban Renewal', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 3, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'Patagonia Classic Retro-X Fleece Jacket', N'~/Content/img/product/56909161_014_b.jpg', 199, 0, N'Warm and windproof, this Retro-X jacket from Patagonia is your new staple fleece jacket. Windproof membrane bonded between ¼"-pile sherpa fleece exterior and a moisture-wicking, highly air-permeable warp-knit mesh features a full-zip front with internal wind flap, Y-Joint sleeves for added mobility and vertical zippered chest pocket in a contrast hue. Finished with zippered handwarmers lined with brushed-polyester mesh.', 100, N'Patagonia', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 3, N'admin')
INSERT INTO [dbo].[Product] ([productName], [image], [price], [discount], [description], [quanlity], [brand], [dateCreate], [status], [categoryId], [userName]) VALUES (N'UO Lucien 5" Vintage Wash Short', N'~/Content/img/product/59291740_045_b.jpg', 39, 0, N'Garment dye sweat shorts from Urban Outfitters with a faded wash effect. In a relaxed fit with adjustable drawstring at the waist and pockets to the sides.', 50, N'Urban Outfitters', CAST(N'2021-08-15T22:33:25.780' AS DateTime), 1, 4, N'admin')

--[Post]
INSERT INTO [dbo].[Article] ([title], [shortDescription], [image], [publicDate], [content], [status], [userName], [categoryId]) VALUES (N'Summer Shirting: The Complete Short Sleeve Button Up Guide', N'', N'~/Content/img/blog/Short-sleeve-shirt-1160x677.jpg', CAST(N'2021-08-15T22:33:25.787' AS DateTime), N'For all its benefits, summer can be a tricky season to dress for. Not in terms of effort – after all, what could be easier than throwing on a pair of shorts and a top? – more that it can be difficult to get creative when limited to such a small selection of garments.Winter brings with it an abundance of sartorial avenues for exploration. We can layer, experiment with textures and use outerwear to our advantage. In summer the options are far fewer. But there is one handy tool you have at your disposal, provided you know how to use it to full effect.This summery take on the classic button-up is a style-conscious man’s best friend in the warmer months. It can lend a dressier edge to otherwise sloppy poolside ensembles, spice up uninspired outfits via appealing colours and eye-catching patterns, and is one of the few garments through which it’s acceptable to convey a little touch of humour and irony. All-over tropical island print? Don’t mind if we do.', 1, N'admin', 2)
INSERT INTO [dbo].[Article] ([title], [shortDescription], [image], [publicDate], [content], [status], [userName], [categoryId]) VALUES (N'High-Fashion Scents: The Greatest Designer Fragrances Of All Time', N'', N'~/Content/img/blog/designer-fragrances-2-1160x677.jpg', CAST(N'2021-08-15T22:33:25.787' AS DateTime), N'Eau de Cologne – the style and type of concentration – was invented by Johann Maria Farina in 1709, while synthetic ingredients have been commonly used since the late 18th Century. And designer scents? “The men’s fashion market was born in the late 1950s, with the iconic launch of Monsieur de Givenchy,” says perfumer Azzi Glasser, founder of The Perfumer’s Story. “Brut by Fabergé (1968) brought in the famous fougère accord, which established the character for men’s fragrances and the start of many others,” says Glasser, who has created fragrances for Topman, Agent Provocateur and Bella Freud, and bespoke scents for many Hollywood actors.', 1, N'admin', 7)
INSERT INTO [dbo].[Article] ([title], [shortDescription], [image], [publicDate], [content], [status], [userName], [categoryId]) VALUES (N'Win A Marloe Watch Company Watch', N'', N'~/Content/img/blog/Marloe-Watch-Company-1160x677.jpg', CAST(N'2021-08-15T22:33:25.787' AS DateTime), N'While today Marloe Watch Company is located up in Perth, Scotland, it was in Marlow that founder Oliver Goffe first discovered the downsides of many a quartz timepiece. Sure, they keep near-perfect time, but open up the back when you need to change the battery and you’ll find out that they often don’t exactly feel ‘prestige’, despite the sometimes prestige price tag.', 1, N'admin', 7)
INSERT INTO [dbo].[Article] ([title], [shortDescription], [image], [publicDate], [content], [status], [userName], [categoryId]) VALUES (N'Low-Tech Kicks: Why The Humble Tennis Shoe Will Always Be In Fashion', N'', N'~/Content/img/blog/Adidas-Stan-Smith-1160x677.jpg', CAST(N'2021-08-15T22:33:25.790' AS DateTime), N'One hundred and twenty years ago this year, one Charles Taylor was born. Taylor, a one-time basketball player, had been a shoe salesman for most of his life. And yet his legacy was to establish a multi-billion dollar industry. During the 1930s, in one of the earliest instances of celebrity sponsorship, Charles, better known as ‘Chuck’, lent his name to a minimalistic high-top basketball boot that had been launched in 1917. And the result – the Chuck Taylor All-Star – was arguably the first classic, must-have sneaker.', 1, N'admin', 6)


-- Tạo VIEW: Thống kê tổng doanh thu  theo từng ngày:
            /*(Ngày, tổng doanh thu)*/
IF EXISTS (SELECT NAME FROM sys.views WHERE NAME = 'vDoanhThuTheoNgay')
	DROP VIEW vDoanhThuTheoNgay
GO
CREATE VIEW vDoanhThuTheoNgay AS
SELECT ISNULL(CONVERT(VARCHAR(10), dateOrder, 103),-1) AS dateOrder, sum(totalMoney) AS income
FROM dbo.Invoince hd 
GROUP BY CONVERT(VARCHAR(10), dateOrder, 103)
GO
-- TAỌ VIEW: Danh sách hóa đơn bán trong ngày
IF EXISTS (SELECT name FROM sys.views WHERE name = 'vHoaDonTrongNgay')
	DROP VIEW vHoaDonTrongNgay
GO
CREATE VIEW vHoaDonTrongNgay AS
SELECT *
FROM dbo.Invoince
WHERE DATEPART(day,dateOrder) = DATEPART(day,getdate())
GO
