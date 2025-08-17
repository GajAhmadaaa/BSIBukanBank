import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/services/auth_service.dart';

class ProfilePage extends StatefulWidget {
  const ProfilePage({super.key});

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  final AuthService _authService = AuthService();
  String? _userEmail;
  bool _isLoggedIn = false;

  @override
  void initState() {
    super.initState();
    _checkLoginStatus();
  }

  Future<void> _checkLoginStatus() async {
    final token = await _authService.getToken();
    if (token != null) {
      final email = await _authService.getUserEmail();
      setState(() {
        _userEmail = email;
        _isLoggedIn = true;
      });
    }
  }

  Future<void> _logout() async {
    await _authService.logout();
    if (mounted) {
      context.go('/login');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Profile'),
        leading: IconButton( // Add back button
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            if (context.canPop()) {
              context.pop();
            } else {
              context.go('/home');
            }
          },
        ),
      ),
      body: Center(
        child: _isLoggedIn
            ? Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text('Welcome, ${_userEmail ?? ''}'),
                  const SizedBox(height: 24),
                  ElevatedButton(
                    onPressed: _logout,
                    child: const Text('Logout'),
                  ),
                ],
              )
            : Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Text('You are not logged in.'),
                  const SizedBox(height: 24),
                  ElevatedButton(
                    onPressed: () => context.go('/login'),
                    child: const Text('Login'),
                  ),
                ],
              ),
      ),
    );
  }
}