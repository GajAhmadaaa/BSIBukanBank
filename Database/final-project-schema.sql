-- FINAL PROJECT: AUTO CAR DEALERSHIP SCHEMA

CREATE DATABASE FinalProject;
GO

USE FinalProject;
GO

-- 1. Dealer
CREATE TABLE Dealer (
    DealerID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    City VARCHAR(50) NOT NULL,
    Address VARCHAR(200),
    PhoneNumber VARCHAR(20) CHECK (PhoneNumber LIKE '[0-9]%')
);

-- 2. Customer
CREATE TABLE Customer (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(20),
    Email VARCHAR(100) UNIQUE,
    Address VARCHAR(200)
);

-- 3. SalesPerson
CREATE TABLE SalesPerson (
    SalesPersonID INT IDENTITY(1,1) PRIMARY KEY,
    DealerID INT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    FOREIGN KEY (DealerID) REFERENCES Dealer(DealerID)
);

-- 4. Car
CREATE TABLE Car (
    CarID INT IDENTITY(1,1) PRIMARY KEY,
    Model VARCHAR(100) NOT NULL,
    CarType VARCHAR(50) NOT NULL,
    BasePrice MONEY NOT NULL,
    Year INT,
    Color VARCHAR(30)
);

-- 5. ConsultHistory
CREATE TABLE ConsultHistory (
    ConsultHistoryID INT IDENTITY(1,1) PRIMARY KEY,
    DealerID INT NOT NULL,
    CustomerID INT NOT NULL,
    SalesPersonID INT NOT NULL,
    CarID INT,
    Budget MONEY,
    PaymentMethod VARCHAR(20),
    ConsultationDate DATETIME NOT NULL,
    Note VARCHAR(200),
    FOREIGN KEY (DealerID) REFERENCES Dealer(DealerID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (SalesPersonID) REFERENCES SalesPerson(SalesPersonID),
    FOREIGN KEY (CarID) REFERENCES Car(CarID)
);

-- 6. TestDrive
CREATE TABLE TestDrive (
    TestDriveID INT IDENTITY(1,1) PRIMARY KEY,
    DealerID INT NOT NULL,
    CustomerID INT NOT NULL,
    SalesPersonID INT NOT NULL,
    CarID INT NOT NULL,
    ConsultHistoryID INT,
    TestDriveDate DATETIME NOT NULL,
    Note VARCHAR(200),
    FOREIGN KEY (DealerID) REFERENCES Dealer(DealerID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (SalesPersonID) REFERENCES SalesPerson(SalesPersonID),
    FOREIGN KEY (CarID) REFERENCES Car(CarID),
    FOREIGN KEY (ConsultHistoryID) REFERENCES ConsultHistory(ConsultHistoryID)
);

-- 7. LetterOfIntent (LOI) - Header
CREATE TABLE LetterOfIntent (
    LOIID INT IDENTITY(1,1) PRIMARY KEY,
    DealerID INT NOT NULL,
    CustomerID INT NOT NULL,
    SalesPersonID INT NOT NULL,
    ConsultHistoryID INT,
    TestDriveID INT,
    LOIDate DATETIME NOT NULL,
    PaymentMethod VARCHAR(20),
    Note VARCHAR(200),
    Status VARCHAR(20), -- Kolom baru untuk status LOI
    FOREIGN KEY (DealerID) REFERENCES Dealer(DealerID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (SalesPersonID) REFERENCES SalesPerson(SalesPersonID),
    FOREIGN KEY (ConsultHistoryID) REFERENCES ConsultHistory(ConsultHistoryID),
    FOREIGN KEY (TestDriveID) REFERENCES TestDrive(TestDriveID)
);

-- 8. LetterOfIntentDetail
CREATE TABLE LetterOfIntentDetail (
    LOIDetailID INT IDENTITY(1,1) PRIMARY KEY,
    LOIID INT NOT NULL,
    CarID INT NOT NULL,
    AgreedPrice MONEY NOT NULL,
    Discount MONEY,
    DownPayment MONEY,
    Note VARCHAR(200),
    FOREIGN KEY (LOIID) REFERENCES LetterOfIntent(LOIID),
    FOREIGN KEY (CarID) REFERENCES Car(CarID)
);

-- 9. Booking
CREATE TABLE Booking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    LOIID INT NOT NULL,
    BookingFee MONEY NOT NULL,
    BookingDate DATETIME NOT NULL,
    Status VARCHAR(20),
    FOREIGN KEY (LOIID) REFERENCES LetterOfIntent(LOIID)
);

-- 10. LeasingCompany
CREATE TABLE LeasingCompany (
    LeasingCompanyID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Address VARCHAR(200),
    PhoneNumber VARCHAR(20)
);

-- 11. CreditApplication
CREATE TABLE CreditApplication (
    CreditAppID INT IDENTITY(1,1) PRIMARY KEY,
    LOIID INT NOT NULL,
    LeasingCompanyID INT,
    ApplicationDate DATETIME NOT NULL,
    Status VARCHAR(20),
    FOREIGN KEY (LOIID) REFERENCES LetterOfIntent(LOIID),
    FOREIGN KEY (LeasingCompanyID) REFERENCES LeasingCompany(LeasingCompanyID)
);

-- 12. CreditDocument
CREATE TABLE CreditDocument (
    CreditDocumentID INT IDENTITY(1,1) PRIMARY KEY,
    CreditAppID INT NOT NULL,
    DocumentType VARCHAR(50) NOT NULL,
    DocumentPath VARCHAR(200),
    UploadDate DATETIME,
    FOREIGN KEY (CreditAppID) REFERENCES CreditApplication(CreditAppID)
);

-- 13. SalesAgreement - Header
CREATE TABLE SalesAgreement (
    SalesAgreementID INT IDENTITY(1,1) PRIMARY KEY,
    DealerID INT NOT NULL,
    CustomerID INT NOT NULL,
    SalesPersonID INT NOT NULL,
    LOIID INT,
    TransactionDate DATETIME NOT NULL,
    TotalAmount MONEY,
    Status VARCHAR(20),
    FOREIGN KEY (DealerID) REFERENCES Dealer(DealerID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (SalesPersonID) REFERENCES SalesPerson(SalesPersonID),
    FOREIGN KEY (LOIID) REFERENCES LetterOfIntent(LOIID)
);

-- 14. SalesAgreementDetail
CREATE TABLE SalesAgreementDetail (
    SalesAgreementDetailID INT IDENTITY(1,1) PRIMARY KEY,
    SalesAgreementID INT NOT NULL,
    LOIDetailID INT,
    CarID INT NOT NULL,
    Price MONEY NOT NULL,
    Discount MONEY,
    Note VARCHAR(200),
    FOREIGN KEY (SalesAgreementID) REFERENCES SalesAgreement(SalesAgreementID),
    FOREIGN KEY (LOIDetailID) REFERENCES LetterOfIntentDetail(LOIDetailID),
    FOREIGN KEY (CarID) REFERENCES Car(CarID)
);

-- 15. PaymentHistory
CREATE TABLE PaymentHistory (
    PaymentHistoryID INT IDENTITY(1,1) PRIMARY KEY,
    SalesAgreementID INT,
    CreditAppID INT,
    PaymentAmount MONEY NOT NULL,
    PaymentDate DATETIME NOT NULL,
    PaymentType VARCHAR(20),
    FOREIGN KEY (SalesAgreementID) REFERENCES SalesAgreement(SalesAgreementID),
    FOREIGN KEY (CreditAppID) REFERENCES CreditApplication(CreditAppID)
);

-- 16. VehicleRegistration
CREATE TABLE VehicleRegistration (
    VehicleRegistrationID INT IDENTITY(1,1) PRIMARY KEY,
    SalesAgreementDetailID INT NOT NULL,
    RegistrationNumber VARCHAR(20),
    OwnershipBookNumber VARCHAR(20),
    TaxStatus VARCHAR(20),
    InsuranceStatus VARCHAR(20),
    FOREIGN KEY (SalesAgreementDetailID) REFERENCES SalesAgreementDetail(SalesAgreementDetailID)
);

-- 17. CarDelivery
CREATE TABLE CarDelivery (
    CarDeliveryID INT IDENTITY(1,1) PRIMARY KEY,
    SalesAgreementID INT NOT NULL,
    DeliveryDate DATETIME NOT NULL,
    Status VARCHAR(20),
    FOREIGN KEY (SalesAgreementID) REFERENCES SalesAgreement(SalesAgreementID)
);

-- 18. CarDeliverySchedule
CREATE TABLE CarDeliverySchedule (
    CarDeliveryScheduleID INT IDENTITY(1,1) PRIMARY KEY,
    CarDeliveryID INT NOT NULL,
    ScheduledDate DATETIME NOT NULL,
    Note VARCHAR(200),
    FOREIGN KEY (CarDeliveryID) REFERENCES CarDelivery(CarDeliveryID)
);

-- 19. PreDeliveryInspection
CREATE TABLE PreDeliveryInspection (
    PreDeliveryInspectionID INT IDENTITY(1,1) PRIMARY KEY,
    CarDeliveryID INT NOT NULL,
    InspectionDate DATETIME NOT NULL,
    InspectorName VARCHAR(100),
    Note VARCHAR(200),
    FOREIGN KEY (CarDeliveryID) REFERENCES CarDelivery(CarDeliveryID)
);

-- 20. ServiceHistory
CREATE TABLE ServiceHistory (
    ServiceID INT IDENTITY(1,1) PRIMARY KEY,
    SalesAgreementDetailID INT NOT NULL,
    ServiceDate DATETIME NOT NULL,
    ServiceType VARCHAR(50),
    Note VARCHAR(200),
    FOREIGN KEY (SalesAgreementDetailID) REFERENCES SalesAgreementDetail(SalesAgreementDetailID)
);

-- 21. CustomerComplaint
CREATE TABLE CustomerComplaint (
    CustomerComplaintID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    SalesAgreementID INT,
    ComplaintDate DATETIME NOT NULL,
    Description VARCHAR(200),
    Status VARCHAR(20),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (SalesAgreementID) REFERENCES SalesAgreement(SalesAgreementID)
);

-- 22. WarrantyClaim
CREATE TABLE WarrantyClaim (
    WarrantyClaimID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    SalesAgreementDetailID INT NOT NULL,
    ClaimDate DATETIME NOT NULL,
    Description VARCHAR(200),
    Status VARCHAR(20),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (SalesAgreementDetailID) REFERENCES SalesAgreementDetail(SalesAgreementDetailID)
);

-- 23. DealerInventory
CREATE TABLE DealerInventory (
    DealerInventoryID INT IDENTITY(1,1) PRIMARY KEY,
    DealerID INT NOT NULL,
    CarID INT NOT NULL,
    Stock INT NOT NULL,
    Price MONEY NOT NULL,
    DiscountPercent FLOAT NOT NULL DEFAULT 0,
    FeePercent FLOAT NOT NULL DEFAULT 0,
    FOREIGN KEY (DealerID) REFERENCES Dealer(DealerID),
    FOREIGN KEY (CarID) REFERENCES Car(CarID),
    CONSTRAINT UQ_DealerInventory_DealerCar UNIQUE (DealerID, CarID)
);

-- 24. InventoryTransfer
CREATE TABLE InventoryTransfer (
    InventoryTransferID INT IDENTITY(1,1) PRIMARY KEY,
    FromDealerID INT NOT NULL,
    ToDealerID INT NOT NULL,
    CarID INT NOT NULL,
    Quantity INT NOT NULL,
    MutationDate DATETIME NOT NULL,
    FOREIGN KEY (FromDealerID) REFERENCES Dealer(DealerID),
    FOREIGN KEY (ToDealerID) REFERENCES Dealer(DealerID),
    FOREIGN KEY (CarID) REFERENCES Car(CarID)
);

-- 25. CustomerFeedback
CREATE TABLE CustomerFeedback (
    CustomerFeedbackID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    SalesAgreementID INT,
    FeedbackDate DATETIME NOT NULL,
    Rating INT,
    Comment VARCHAR(200),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (SalesAgreementID) REFERENCES SalesAgreement(SalesAgreementID)
);

-- 26. CustomerNotification (Tabel baru)
CREATE TABLE CustomerNotification (
    CustomerNotificationID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    LOIID INT NULL,
    SalesAgreementID INT NULL,
    NotificationType VARCHAR(50) NOT NULL,
    Message VARCHAR(500) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ReadDate DATETIME NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (LOIID) REFERENCES LetterOfIntent(LOIID),
    FOREIGN KEY (SalesAgreementID) REFERENCES SalesAgreement(SalesAgreementID)
);
GO