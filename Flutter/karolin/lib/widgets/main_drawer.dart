import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/services/auth_service.dart';

class MainDrawer extends StatefulWidget {
  const MainDrawer({super.key});

  @override
  State<MainDrawer> createState() => _MainDrawerState();
}

class _MainDrawerState extends State<MainDrawer> {
  final AuthService _authService = AuthService();
  bool _isLoggedIn = false;

  @override
  void initState() {
    super.initState();
    _checkLoginStatus();
  }

  Future<void> _checkLoginStatus() async {
    final token = await _authService.getToken();
    setState(() {
      _isLoggedIn = token != null;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Drawer(
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
                fontSize: 20,
              ),
            ),
          ),
          ListTile(
            leading: const Icon(Icons.home),
            title: const Text('Home'),
            onTap: () {
              context.go('/home');
            },
            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
          ),
          ListTile(
            leading: const Icon(Icons.notifications),
            title: const Text('Notifications'),
            onTap: () {
              context.go('/notification');
            },
            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
          ),
          ListTile(
            leading: const Icon(Icons.shopping_cart),
            title: const Text('Cart'),
            onTap: () {
              context.go('/cart');
            },
            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
          ),
          const Divider(),
          if (_isLoggedIn) ...[
            ListTile(
              leading: const Icon(Icons.person),
              title: const Text('Profile'),
              onTap: () {
                context.go('/profile');
              },
              contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
            ),
            ListTile(
              leading: const Icon(Icons.logout),
              title: const Text('Logout'),
              onTap: () async {
                await _authService.logout();
                if (context.mounted) {
                  context.go('/login');
                }
              },
              contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
            ),
          ] else ...[
            ListTile(
              leading: const Icon(Icons.login),
              title: const Text('Login'),
              onTap: () {
                context.go('/login');
              },
              contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
            ),
            ListTile(
              leading: const Icon(Icons.person_add),
              title: const Text('Register'),
              onTap: () {
                context.go('/register');
              },
              contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
            ),
          ],
        ],
      ),
    );
  }
}
