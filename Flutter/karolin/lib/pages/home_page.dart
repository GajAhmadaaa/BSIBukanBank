import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/services/auth_service.dart';
import 'package:karolin/widgets/service_card.dart';
import 'package:karolin/widgets/car_card.dart';
import 'package:karolin/widgets/testimonial_card.dart';

import 'package:karolin/widgets/main_drawer.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final AuthService _authService = AuthService();
  bool _isLoggedIn = false;

  @override
  void initState() {
    super.initState();
    _checkLoginStatus();
  }

  Future<void> _checkLoginStatus() async {
    final token = await _authService.getToken();
    if (mounted) {
      setState(() {
        _isLoggedIn = token != null;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Karolin - Dealer Mitsubishi',
          style: TextStyle(
            fontSize: 18,
          ),
        ),
        backgroundColor: Colors.blue,
        foregroundColor: Colors.white,
        actions: [
          IconButton(
            icon: const Icon(Icons.notifications),
            onPressed: () {
              if (context.mounted) {
                context.push('/notification');
              }
            },
          ),
          IconButton(
            icon: const Icon(Icons.shopping_cart),
            onPressed: () {
              if (context.mounted) {
                context.push('/cart/pending');
              }
            },
          ),
          IconButton(
            icon: const Icon(Icons.person),
            onPressed: () {
              if (context.mounted) {
                if (_isLoggedIn) {
                  context.push('/profile');
                } else {
                  context.push('/login');
                }
              }
            },
          ),
        ],
      ),
      drawer: const MainDrawer(),
      body: SingleChildScrollView(
        child: Column(
          children: [
            // Hero Section
            Container(
              decoration: const BoxDecoration(
                gradient: LinearGradient(
                  colors: [Colors.blue, Colors.blueAccent],
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                ),
              ),
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Selamat Datang di Dealer Mitsubishi',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Temukan mobil Mitsubishi impian Anda dengan penawaran terbaik dan layanan berkualitas tinggi.',
                    style: TextStyle(
                      fontSize: 14,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 16),
                  Row(
                    children: [
                      ElevatedButton(
                        onPressed: () {
                          // Navigasi ke halaman order
                        },
                        style: ElevatedButton.styleFrom(
                          backgroundColor: Colors.white,
                          foregroundColor: Colors.blue,
                          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(4),
                          ),
                        ),
                        child: const Text(
                          'Order Mobil',
                          style: TextStyle(
                            fontSize: 14,
                          ),
                        ),
                      ),
                      const SizedBox(width: 10),
                      OutlinedButton(
                        onPressed: () {
                          // Navigasi ke halaman test drive
                        },
                        style: OutlinedButton.styleFrom(
                          side: const BorderSide(color: Colors.white),
                          foregroundColor: Colors.white,
                          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(4),
                          ),
                        ),
                        child: const Text(
                          'Jadwal Test Drive',
                          style: TextStyle(
                            fontSize: 14,
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
            
            // Services Section
            Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Layanan Kami',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Kami menyediakan berbagai layanan untuk memenuhi kebutuhan mobil Mitsubishi Anda',
                    style: TextStyle(
                      fontSize: 14,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 16),
                  IntrinsicHeight(
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        Flexible(
                          child: ServiceCard(
                          icon: Icons.directions_car,
                          title: 'Penjualan Mobil Mitsubishi',
                          description:
                              'Berbagai pilihan mobil Mitsubishi baru dengan kualitas terjamin dan harga kompetitif.',
                          color: Colors.blue,
                        ),
                      ),
                      const SizedBox(width: 15),
                      Flexible(
                        child: ServiceCard(
                          icon: Icons.build,
                          title: 'Servis & Perbaikan',
                          description:
                              'Layanan servis profesional dengan teknisi berpengalaman dan suku cadang asli Mitsubishi.',
                          color: Colors.green,
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 20),
                  ServiceCard(
                    icon: Icons.speed,
                    title: 'Test Drive',
                    description:
                        'Jadwalkan test drive untuk merasakan langsung kenyamanan dan performa mobil Mitsubishi pilihan Anda.',
                    color: Colors.orange,
                    fullWidth: true,
                  ),
                ],
              ),
            ),
            
            // Featured Cars Section
            Container(
              color: Colors.grey[100],
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Mobil Pilihan',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Temukan mobil impian Anda dari Mitsubishi',
                    style: TextStyle(
                      fontSize: 14,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 16),
                  IntrinsicHeight(
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        Flexible(
                          child: CarCard(
                          imageUrl: 'assets/images/xpander.jpg',
                          title: 'Mitsubishi Xpander',
                          description: 'MPV keluarga dengan kapasitas 7 penumpang dan fitur lengkap.',
                          price: 'Rp 250.000.000',
                        ),
                      ),
                      const SizedBox(width: 15),
                      Flexible(
                        child: CarCard(
                          imageUrl: 'assets/images/pajero_sport.jpg',
                          title: 'Mitsubishi Pajero Sport',
                          description: 'SUV tangguh dengan performa off-road yang handal.',
                          price: 'Rp 400.000.000',
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 20),
                  IntrinsicHeight(
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        Flexible(
                          child: CarCard(
                          imageUrl: 'assets/images/mirage.jpg',
                          title: 'Mitsubishi Mirage',
                          description: 'Hatchback hemat bahan bakar dengan desain modern.',
                          price: 'Rp 150.000.000',
                        ),
                      ),
                      const SizedBox(width: 15),
                      Flexible(
                        child: CarCard(
                          imageUrl: 'assets/images/destinator.jpg',
                          title: 'Mitsubishi Destinator',
                          description: 'SUV crossover dengan performa tinggi dan kenyamanan premium.',
                          price: 'Rp 350.000.000',
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 20),
                  Center(
                    child: ElevatedButton(
                      onPressed: () {
                        // Navigasi ke halaman semua mobil
                      },
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.blue,
                        foregroundColor: Colors.white,
                        padding: const EdgeInsets.symmetric(
                          horizontal: 24,
                          vertical: 12,
                        ),
                      ),
                      child: const Text(
                        'Lihat Semua Mobil',
                        style: TextStyle(
                          fontSize: 14,
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
            
            // Testimonial Section
            Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Apa Kata Customer Kami',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Ulasan dari customer yang telah mempercayai layanan kami',
                    style: TextStyle(
                      fontSize: 14,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 16),
                  Row(
                    children: [
                      Flexible(
                        child: TestimonialCard(
                          initials: 'BS',
                          name: 'Budi Santoso',
                          rating: 5,
                          comment:
                              'Pelayanan sangat memuaskan! Saya membeli Mitsubishi Xpander dan sangat puas dengan pelayanan serta kualitas mobilnya.',
                        ),
                      ),
                      const SizedBox(width: 15),
                      Flexible(
                        child: TestimonialCard(
                          initials: 'SR',
                          name: 'Siti Rahmawati',
                          rating: 4,
                          comment:
                              'Servis di sini sangat profesional. Teknisi berpengalaman dan harga sangat terjangkau dibanding bengkel lain.',
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
            // Footer Section
            Container(
              color: Colors.grey[100],
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Karolin - Dealer Mitsubishi',
                    style: TextStyle(
                      fontSize: 16,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Jl. Merdeka No. 123, Jakarta',
                    style: TextStyle(
                      fontSize: 14,
                    ),
                  ),
                  const Text(
                    'Telepon: (021) 123-4567',
                    style: TextStyle(
                      fontSize: 14,
                    ),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    '© 2025 Karolin - Dealer Mitsubishi',
                    style: TextStyle(
                      fontSize: 12,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Row(
                    children: [
                      TextButton(
                        onPressed: () {
                          // Navigasi ke halaman kebijakan privasi
                        },
                        style: TextButton.styleFrom(
                          padding: const EdgeInsets.all(0),
                          minimumSize: const Size(0, 0),
                        ),
                        child: const Text(
                          'Kebijakan Privasi',
                          style: TextStyle(
                            fontSize: 12,
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}