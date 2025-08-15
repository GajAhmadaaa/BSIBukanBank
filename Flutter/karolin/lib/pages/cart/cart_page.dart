import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

class CartPage extends StatelessWidget {
  final Widget child;

  const CartPage({super.key, required this.child});

  int _getCurrentIndex(BuildContext context) {
    final location = GoRouterState.of(context).uri.toString();
    if (location.endsWith('/cart') || location == '/cart') return 0;
    if (location.endsWith('/cart/unpaid')) return 1;
    if (location.endsWith('/cart/paid')) return 2;
    return 0;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: child,
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _getCurrentIndex(context),
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
              context.go('/cart');
              break;
            case 1:
              context.go('/cart/unpaid');
              break;
            case 2:
              context.go('/cart/paid');
              break;
          }
        },
      ),
    );
  }
}
