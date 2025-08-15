import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

class LoginPage extends StatelessWidget {
  const LoginPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text('Login Page'),
            const SizedBox(height: 24),
            ElevatedButton(
              onPressed: () {
                context.pushReplacement('/profile');
              },
              child: const Text('Login'),
            ),
          ],
        ),
      ),
    );
  }
}
