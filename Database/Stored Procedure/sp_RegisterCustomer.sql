CREATE PROCEDURE sp_RegisterCustomer
    @Name VARCHAR(100),
    @PhoneNumber VARCHAR(20),
    @Email VARCHAR(100),
    @Address VARCHAR(200)
AS
BEGIN
    INSERT INTO Customer (Name, PhoneNumber, Email, Address)
    VALUES (@Name, @PhoneNumber, @Email, @Address);

    SELECT SCOPE_IDENTITY() AS CustomerID;
END
GO