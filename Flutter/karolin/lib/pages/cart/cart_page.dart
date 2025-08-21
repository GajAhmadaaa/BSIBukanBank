import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/services/auth_service.dart';

class CartPage extends StatefulWidget {
  final Widget child;
  final GoRouterState state;

  const CartPage({super.key, required this.child, required this.state});

  @override
  State<CartPage> createState() => _CartPageState();
}

class _CartPageState extends State<CartPage> {
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

  int _getCurrentIndex() {
    final location = widget.state.uri.toString();
    if (location == '/cart/pending' || location.endsWith('/cart/pending')) {
      return 0;
    }
    if (location.endsWith('/cart/unpaid')) {
      return 1;
    }
    if (location.endsWith('/cart/paid')) {
      return 2;
    }
    return 0;
  }

  @override
  Widget build(BuildContext context) {
    return PopScope(
      canPop: true, // biarin dia bisa pop
      onPopInvokedWithResult: (didPop, result) {
        if (!didPop) {
          // kalau masih belum pop, arahkan manual
          if (context.mounted) {
            context.go('/');
          }
        }
      },
      child: Scaffold(
        appBar: AppBar(
          title: const Text('Cart'),
          leading: BackButton(
            onPressed: () {
              if (context.mounted) {
                context.go('/home');
              }
            },
          ),
        ),
        body: _isLoggedIn
            ? widget.child
            : Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const Text('Please login to view your cart.'),
                    const SizedBox(height: 24),
                    ElevatedButton(
                      onPressed: () {
                        if (context.mounted) {
                          context.push('/login');
                        }
                      },
                      child: const Text('Login'),
                    ),
                  ],
                ),
              ),
        bottomNavigationBar: _isLoggedIn
            ? BottomNavigationBar(
                currentIndex: _getCurrentIndex(),
                items: const [
                  BottomNavigationBarItem(
                    icon: Icon(Icons.pending),
                    label: 'Pending',
                  ),
                  BottomNavigationBarItem(
                    icon: Icon(Icons.payment),
                    label: 'Unpaid',
                  ),
                  BottomNavigationBarItem(
                    icon: Icon(Icons.check_circle),
                    label: 'Paid',
                  ),
                ],
                onTap: (index) {
                  switch (index) {
                    case 0:
                      if (context.mounted) {
                        context.go('/cart/pending');
                      }
                      break;
                    case 1:
                      if (context.mounted) {
                        context.go('/cart/unpaid');
                      }
                      break;
                    case 2:
                      if (context.mounted) {
                        context.go('/cart/paid');
                      }
                      break;
                  }
                },
              )
            : null,
      ),
    );
  }
}

