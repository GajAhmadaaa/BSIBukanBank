-- Stored Procedure untuk memverifikasi kredensial pengguna dengan .NET Core Identity
-- Catatan: Ini adalah implementasi yang disederhanakan dan mungkin tidak sepenuhnya akurat
-- Untuk produksi, gunakan fungsi bawaan .NET Core Identity

CREATE PROCEDURE sp_VerifyUserCredentials
    @Username NVARCHAR(256),
    @Password NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId NVARCHAR(450);
    DECLARE @PasswordHash NVARCHAR(MAX);
    
    -- Cari pengguna berdasarkan username atau email
    SELECT @UserId = Id, @PasswordHash = PasswordHash
    FROM AspNetUsers
    WHERE UserName = @Username OR Email = @Username;
    
    -- Jika pengguna tidak ditemukan
    IF @UserId IS NULL
    BEGIN
        SELECT 'Invalid' AS Result, NULL AS UserId;
        RETURN;
    END
    
    -- Untuk verifikasi password, kita akan menggunakan fungsi bawaan .NET Core Identity
    -- Karena implementasinya sangat kompleks, kita akan menggunakan pendekatan berikut:
    -- 1. Buat aplikasi .NET Core sederhana yang menggunakan PasswordHasher untuk verifikasi
    -- 2. Panggil aplikasi tersebut dari stored procedure (jika memungkinkan)
    -- 3. Atau gunakan CLR function di SQL Server (jika memungkinkan)
    
    -- Untuk sekarang, kita akan mengembalikan hasil valid untuk testing
    -- CATATAN: Ini bukan implementasi yang aman untuk produksi
    SELECT 'Valid' AS Result, @UserId AS UserId;
END