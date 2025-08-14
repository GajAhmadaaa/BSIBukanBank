import 'package:flutter/material.dart';

class HomePage extends StatelessWidget {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Karolin - Dealer Mitsubishi'),
        backgroundColor: Colors.blue,
        foregroundColor: Colors.white,
        actions: [
          IconButton(
            icon: const Icon(Icons.person),
            onPressed: () {
              // Nanti akan dihubungkan dengan halaman login
            },
          ),
        ],
      ),
      drawer: Drawer(
        child: ListView(
          padding: EdgeInsets.zero,
          children: [
            const DrawerHeader(
              decoration: BoxDecoration(
                color: Colors.blue,
              ),
              child: Text(
                'Karolin Dealer',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 24,
                ),
              ),
            ),
            ListTile(
              leading: const Icon(Icons.home),
              title: const Text('Home'),
              onTap: () {
                Navigator.pop(context);
              },
            ),
            ListTile(
              leading: const Icon(Icons.phone),
              title: const Text('Contact'),
              onTap: () {
                // Navigasi ke halaman contact
              },
            ),
            ListTile(
              leading: const Icon(Icons.people),
              title: const Text('Customer'),
              onTap: () {
                // Navigasi ke halaman customer
              },
            ),
            ListTile(
              leading: const Icon(Icons.shopping_cart),
              title: const Text('Orders'),
              onTap: () {
                // Navigasi ke halaman orders
              },
            ),
            ListTile(
              leading: const Icon(Icons.speed),
              title: const Text('Test Drive'),
              onTap: () {
                // Navigasi ke halaman test drive
              },
            ),
          ],
        ),
      ),
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
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Selamat Datang di Dealer Mitsubishi',
                    style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 10),
                  const Text(
                    'Temukan mobil Mitsubishi impian Anda dengan penawaran terbaik dan layanan berkualitas tinggi.',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 20),
                  Row(
                    children: [
                      ElevatedButton(
                        onPressed: () {
                          // Navigasi ke halaman order
                        },
                        style: ElevatedButton.styleFrom(
                          backgroundColor: Colors.white,
                          foregroundColor: Colors.blue,
                        ),
                        child: const Text('Order Mobil'),
                      ),
                      const SizedBox(width: 10),
                      OutlinedButton(
                        onPressed: () {
                          // Navigasi ke halaman test drive
                        },
                        style: OutlinedButton.styleFrom(
                          side: const BorderSide(color: Colors.white),
                          foregroundColor: Colors.white,
                        ),
                        child: const Text('Jadwal Test Drive'),
                      ),
                    ],
                  ),
                ],
              ),
            ),
            
            // Services Section
            Padding(
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Layanan Kami',
                    style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 10),
                  const Text(
                    'Kami menyediakan berbagai layanan untuk memenuhi kebutuhan mobil Mitsubishi Anda',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 20),
                  Row(
                    children: [
                      _buildServiceCard(
                        icon: Icons.directions_car,
                        title: 'Penjualan Mobil Mitsubishi',
                        description:
                            'Berbagai pilihan mobil Mitsubishi baru dengan kualitas terjamin dan harga kompetitif.',
                        color: Colors.blue,
                      ),
                      const SizedBox(width: 20),
                      _buildServiceCard(
                        icon: Icons.build,
                        title: 'Servis & Perbaikan',
                        description:
                            'Layanan servis profesional dengan teknisi berpengalaman dan suku cadang asli Mitsubishi.',
                        color: Colors.green,
                      ),
                    ],
                  ),
                  const SizedBox(height: 20),
                  _buildServiceCard(
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
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Mobil Pilihan',
                    style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 10),
                  const Text(
                    'Temukan mobil impian Anda dari Mitsubishi',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 20),
                  Row(
                    children: [
                      _buildCarCard(
                        imageUrl: 'assets/images/xpander.jpg',
                        title: 'Mitsubishi Xpander',
                        description: 'MPV keluarga dengan kapasitas 7 penumpang dan fitur lengkap.',
                        price: 'Rp 250.000.000',
                      ),
                      const SizedBox(width: 20),
                      _buildCarCard(
                        imageUrl: 'assets/images/pajero_sport.jpg',
                        title: 'Mitsubishi Pajero Sport',
                        description: 'SUV tangguh dengan performa off-road yang handal.',
                        price: 'Rp 400.000.000',
                      ),
                    ],
                  ),
                  const SizedBox(height: 20),
                  Row(
                    children: [
                      _buildCarCard(
                        imageUrl: 'assets/images/mirage.jpg',
                        title: 'Mitsubishi Mirage',
                        description: 'Hatchback hemat bahan bakar dengan desain modern.',
                        price: 'Rp 150.000.000',
                      ),
                      const SizedBox(width: 20),
                      _buildCarCard(
                        imageUrl: 'assets/images/destinator.jpg',
                        title: 'Mitsubishi Destinator',
                        description: 'SUV crossover dengan performa tinggi dan kenyamanan premium.',
                        price: 'Rp 350.000.000',
                      ),
                    ],
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
                          horizontal: 30,
                          vertical: 15,
                        ),
                      ),
                      child: const Text(
                        'Lihat Semua Mobil',
                        style: TextStyle(
                          fontSize: 16,
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
            
            // Testimonial Section
            Padding(
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Apa Kata Customer Kami',
                    style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 10),
                  const Text(
                    'Ulasan dari customer yang telah mempercayai layanan kami',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 20),
                  Row(
                    children: [
                      _buildTestimonialCard(
                        initials: 'BS',
                        name: 'Budi Santoso',
                        rating: 5,
                        comment:
                            'Pelayanan sangat memuaskan! Saya membeli Mitsubishi Xpander dan sangat puas dengan pelayanan serta kualitas mobilnya.',
                      ),
                      const SizedBox(width: 20),
                      _buildTestimonialCard(
                        initials: 'SR',
                        name: 'Siti Rahmawati',
                        rating: 4,
                        comment:
                            'Servis di sini sangat profesional. Teknisi berpengalaman dan harga sangat terjangkau dibanding bengkel lain.',
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
      bottomNavigationBar: BottomAppBar(
        child: Container(
          padding: const EdgeInsets.all(20),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Text(
                'Karolin - Dealer Mitsubishi',
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                ),
              ),
              const SizedBox(height: 10),
              const Text('Jl. Merdeka No. 123, Jakarta'),
              const Text('Telepon: (021) 123-4567'),
              const SizedBox(height: 10),
              const Text('© 2025 Karolin - Dealer Mitsubishi'),
              const SizedBox(height: 10),
              Row(
                children: [
                  TextButton(
                    onPressed: () {
                      // Navigasi ke halaman kebijakan privasi
                    },
                    child: const Text('Kebijakan Privasi'),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildServiceCard({
    required IconData icon,
    required String title,
    required String description,
    required Color color,
    bool fullWidth = false,
  }) {
    return Container(
      width: fullWidth ? double.infinity : null,
      padding: const EdgeInsets.all(20),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(10),
        boxShadow: [
          BoxShadow(
            color: Colors.grey.withOpacity(0.2),
            spreadRadius: 2,
            blurRadius: 5,
            offset: const Offset(0, 3),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            padding: const EdgeInsets.all(10),
            decoration: BoxDecoration(
              color: color.withOpacity(0.1),
              borderRadius: BorderRadius.circular(10),
            ),
            child: Icon(
              icon,
              color: color,
              size: 30,
            ),
          ),
          const SizedBox(height: 15),
          Text(
            title,
            style: const TextStyle(
              fontSize: 18,
              fontWeight: FontWeight.bold,
            ),
          ),
          const SizedBox(height: 10),
          Text(
            description,
            style: const TextStyle(
              fontSize: 14,
              color: Colors.grey,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildCarCard({
    required String imageUrl,
    required String title,
    required String description,
    required String price,
  }) {
    return Expanded(
      child: Container(
        padding: const EdgeInsets.all(10),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(10),
          boxShadow: [
            BoxShadow(
              color: Colors.grey.withOpacity(0.2),
              spreadRadius: 2,
              blurRadius: 5,
              offset: const Offset(0, 3),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            ClipRRect(
              borderRadius: BorderRadius.circular(5),
              child: Image.asset(
                imageUrl,
                height: 100,
                width: double.infinity,
                fit: BoxFit.cover,
              ),
            ),
            const SizedBox(height: 10),
            Text(
              title,
              style: const TextStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(height: 5),
            Text(
              description,
              style: const TextStyle(
                fontSize: 12,
                color: Colors.grey,
              ),
            ),
            const SizedBox(height: 10),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  price,
                  style: const TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.bold,
                    color: Colors.blue,
                  ),
                ),
                TextButton(
                  onPressed: () {
                    // Lihat detail mobil
                  },
                  child: const Text('Detail'),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildTestimonialCard({
    required String initials,
    required String name,
    required int rating,
    required String comment,
  }) {
    return Expanded(
      child: Container(
        padding: const EdgeInsets.all(15),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(10),
          boxShadow: [
            BoxShadow(
              color: Colors.grey.withOpacity(0.2),
              spreadRadius: 2,
              blurRadius: 5,
              offset: const Offset(0, 3),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  width: 50,
                  height: 50,
                  decoration: const BoxDecoration(
                    color: Colors.blue,
                    shape: BoxShape.circle,
                  ),
                  child: Center(
                    child: Text(
                      initials,
                      style: const TextStyle(
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 10),
                Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      name,
                      style: const TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Row(
                      children: List.generate(
                        5,
                        (index) => Icon(
                          index < rating ? Icons.star : Icons.star_border,
                          color: Colors.orange,
                          size: 16,
                        ),
                      ),
                    ),
                  ],
                ),
              ],
            ),
            const SizedBox(height: 15),
            Text(
              comment,
              style: const TextStyle(
                fontSize: 14,
                color: Colors.grey,
              ),
            ),
          ],
        ),
      ),
    );
  }
}