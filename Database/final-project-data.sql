-- FINAL PROJECT: SAMPLE DATA

USE FinalProject;
GO

-- Dealer
INSERT INTO Dealer (Name, City, Address, PhoneNumber) VALUES
('Mitsu Auto Center', 'Jakarta', 'Jl. Sudirman No. 123', '021-1234567'),
('Mitsu Prima Motor', 'Surabaya', 'Jl. Basuki Rahmat No. 45', '031-7654321'),
('Mitsu Nusantara', 'Bandung', 'Jl. Asia Afrika No. 88', '022-8765432');

-- Customer
INSERT INTO Customer (Name, PhoneNumber, Email, Address) VALUES
('Budi Santoso', '081234567890', 'budi.santoso@example.com', 'Jl. Merdeka No. 1'),
('Ani Wijaya', '087654321098', 'ani.wijaya@example.com', 'Jl. Pahlawan No. 10'),
('Siti Aminah', '082112233445', 'siti.aminah@example.com', 'Jl. Diponegoro No. 5');

-- SalesPerson
INSERT INTO SalesPerson (DealerID, Name) VALUES
(1, 'Rina Marlina'),
(1, 'Agus Setiawan'),
(2, 'Dewi Lestari'),
(3, 'Fajar Pratama');

-- Car
INSERT INTO Car (Model, CarType, BasePrice, Year, Color) VALUES
('Pajero Sport', 'SUV', 500000000, 2023, 'Black'),
('Xpander', 'MPV', 250000000, 2023, 'White'),
('Triton', 'Pickup', 400000000, 2022, 'Silver'),
('Outlander', 'SUV', 350000000, 2021, 'Red');

-- LeasingCompany
INSERT INTO LeasingCompany (Name, Address, PhoneNumber) VALUES
('Mega Auto Finance', 'Jl. Gatot Subroto No. 78', '021-9876543'),
('Adira Finance', 'Jl. MT Haryono No. 12', '021-5432198'),
('BCA Finance', 'Jl. Setiabudi No. 20', '021-1122334');

-- DealerInventory
INSERT INTO DealerInventory (DealerID, CarID, Stock, Price, DiscountPercent, FeePercent) VALUES
(1, 1, 5, 500000000, 2, 1);

-- ConsultHistory
INSERT INTO ConsultHistory (DealerID, CustomerID, SalesPersonID, CarID, Budget, PaymentMethod, ConsultationDate, Note) VALUES
(1, 1, 1, 1, 600000000, 'Cash', '2025-07-01', 'Tertarik Pajero Sport');

-- TestDrive
INSERT INTO TestDrive (DealerID, CustomerID, SalesPersonID, CarID, ConsultHistoryID, TestDriveDate, Note) VALUES
(1, 1, 1, 1, 1, '2025-07-02', 'Test drive Pajero Sport');

-- LetterOfIntent
INSERT INTO LetterOfIntent (DealerID, CustomerID, SalesPersonID, ConsultHistoryID, TestDriveID, LOIDate, PaymentMethod, Note) VALUES
(1, 1, 1, 1, 1, '2025-07-03', 'Cash', 'Akan booking minggu depan');

-- LetterOfIntentDetail
INSERT INTO LetterOfIntentDetail (LOIID, CarID, AgreedPrice, Discount, DownPayment, Note) VALUES
(1, 1, 590000000, 10000000, 50000000, 'Diskon promo Juli');

-- Booking
INSERT INTO Booking (LOIID, BookingFee, BookingDate, Status) VALUES
(1, 5000000, '2025-07-04', 'Booked');

-- CreditApplication
INSERT INTO CreditApplication (LOIID, LeasingCompanyID, ApplicationDate, Status) VALUES
(1, 1, '2025-07-05', 'Pending');

-- CreditDocument
INSERT INTO CreditDocument (CreditAppID, DocumentType, DocumentPath, UploadDate) VALUES
(1, 'KTP', '/docs/ktp_budi.pdf', '2025-07-06');

-- SalesAgreement
INSERT INTO SalesAgreement (DealerID, CustomerID, SalesPersonID, LOIID, TransactionDate, TotalAmount, Status) VALUES
(1, 1, 1, 1, '2025-07-10', 590000000, 'Completed');

-- SalesAgreementDetail (added to complete sample data for SalesAgreementID 1)
INSERT INTO SalesAgreementDetail (SalesAgreementID, LOIDetailID, CarID, Price, Discount, Note) VALUES
(1, 1, 1, 590000000, 10000000, 'Detail transaksi untuk Pajero Sport');

-- PaymentHistory
INSERT INTO PaymentHistory (SalesAgreementID, PaymentAmount, PaymentDate, PaymentType) VALUES
(1, 590000000, '2025-07-11', 'Transfer');

-- VehicleRegistration
INSERT INTO VehicleRegistration (SalesAgreementDetailID, RegistrationNumber, OwnershipBookNumber, TaxStatus, InsuranceStatus) VALUES
(1, 'B1234XYZ', 'BPKB001', 'Aktif', 'Aktif');

-- CarDelivery
INSERT INTO CarDelivery (SalesAgreementID, DeliveryDate, Status) VALUES
(1, '2025-07-15', 'Delivered');

-- CarDeliverySchedule
INSERT INTO CarDeliverySchedule (CarDeliveryID, ScheduledDate, Note) VALUES
(1, '2025-07-14', 'Jadwal pengiriman sesuai permintaan');

-- PreDeliveryInspection
INSERT INTO PreDeliveryInspection (CarDeliveryID, InspectionDate, InspectorName, Note) VALUES
(1, '2025-07-14', 'Andi Saputra', 'Semua OK');

-- ServiceHistory
INSERT INTO ServiceHistory (SalesAgreementDetailID, ServiceDate, ServiceType, Note) VALUES
(1, '2025-07-25', 'Servis 1.000 km', 'Servis pertama');

-- CustomerFeedback
INSERT INTO CustomerFeedback (CustomerID, SalesAgreementID, FeedbackDate, Rating, Comment) VALUES
(1, 1, '2025-07-20', 5, 'Pelayanan sangat memuaskan');

-- CustomerComplaint
INSERT INTO CustomerComplaint (CustomerID, SalesAgreementID, ComplaintDate, Description, Status) VALUES
(1, 1, '2025-07-21', 'AC kurang dingin', 'Open');

-- WarrantyClaim
INSERT INTO WarrantyClaim (CustomerID, SalesAgreementDetailID, ClaimDate, Description, Status) VALUES
(1, 1, '2025-07-22', 'Klaim garansi AC', 'Diproses');

-- InventoryTransfer
INSERT INTO InventoryTransfer (FromDealerID, ToDealerID, CarID, Quantity, MutationDate) VALUES
(1, 2, 1, 1, '2025-07-18');