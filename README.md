# Karolin - FinalProject.MVC

Selamat datang di Karolin, sebuah aplikasi web berbasis ASP.NET Core MVC yang dirancang untuk mengelola operasional dealer mobil.

## Fitur Utama

- **Manajemen Customer**: Pendaftaran, pengelolaan, dan pemeliharaan data customer
- **Penjadwalan Test Drive**: Sistem untuk menjadwalkan dan mengelola sesi test drive
- **Showroom Virtual**: Menampilkan berbagai model mobil yang tersedia
- **Landing Page Profesional**: Tampilan menarik untuk menarik customer

## Teknologi yang Digunakan

- ASP.NET Core MVC
- Entity Framework Core
- Bootstrap 5
- HTML5 & CSS3
- C#

## Struktur Project

```
FinalProject.MVC/
├── Controllers/
│   ├── HomeController.cs
│   ├── CustomerController.cs
│   └── TestDriveController.cs
├── Models/
├── ViewModels/
├── Views/
│   ├── Home/
│   ├── Customer/
│   ├── TestDrive/
│   └── Shared/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
└── Program.cs
```

## Cara Menjalankan Aplikasi

1. Pastikan Anda telah menginstal .NET 9 SDK
2. Buka project di Visual Studio atau IDE pilihan Anda
3. Jalankan perintah `dotnet restore` untuk mengunduh dependencies
4. Jalankan perintah `dotnet run` untuk memulai aplikasi
5. Akses aplikasi melalui browser di `https://localhost:7027` atau `http://localhost:5275`

## Kontribusi

Project ini dikembangkan sebagai bagian dari FinalProject untuk BSI Bukan Bank.

## Lisensi

MIT License 
