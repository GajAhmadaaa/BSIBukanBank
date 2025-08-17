import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/models/order.dart';
import 'package:karolin/services/auth_service.dart';
import 'package:karolin/services/order_service.dart';

class OrderStatusPage extends StatefulWidget {
  final int orderId;

  const OrderStatusPage({super.key, required this.orderId});

  @override
  State<OrderStatusPage> createState() => _OrderStatusPageState();
}

class _OrderStatusPageState extends State<OrderStatusPage> {
  final OrderService _orderService = OrderService();
  final AuthService _authService = AuthService();
  late Future<LetterOfIntent> _order;
  bool _isLoggedIn = false;

  @override
  void initState() {
    super.initState();
    _checkLoginStatusAndLoadOrder();
  }

  Future<void> _checkLoginStatusAndLoadOrder() async {
    final token = await _authService.getToken();
    if (token != null) {
      setState(() {
        _isLoggedIn = true;
        _order = _orderService.getOrderById(widget.orderId);
      });
    } else {
      setState(() {
        _isLoggedIn = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Order #${widget.orderId}'),
        leading: IconButton( // Add back button
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            context.pop(); // Pop the current route
          },
        ),
      ),
      body: _isLoggedIn
          ? FutureBuilder<LetterOfIntent>(
              future: _order,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                } else if (snapshot.hasError) {
                  return Center(child: Text('Error: ${snapshot.error}'));
                } else if (!snapshot.hasData) {
                  return const Center(child: Text('Order not found.'));
                } else {
                  final order = snapshot.data!;
                  return Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('Order ID: ${order.id}', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        Text('Status: ${order.status}'),
                        const SizedBox(height: 8),
                        Text('Date: ${order.loidate.toLocal()}'),
                        const SizedBox(height: 16),
                        const Text('Details:', style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
                        Expanded(
                          child: ListView.builder(
                            itemCount: order.details.length,
                            itemBuilder: (context, index) {
                              final detail = order.details[index];
                              return Card(
                                margin: const EdgeInsets.symmetric(vertical: 8.0),
                                child: ListTile(
                                  title: Text(detail.carName),
                                  subtitle: Text('Price: \$${detail.agreedPrice.toStringAsFixed(2)}'),
                                  trailing: Text('Discount: \$${detail.discount?.toStringAsFixed(2) ?? '0.00'}'),
                                ),
                              );
                            },
                          ),
                        ),
                      ],
                    ),
                  );
                }
              },
            )
          : Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                const Text('Please login to view order details.'),
                const SizedBox(height: 24),
                ElevatedButton(
                  onPressed: () => context.go('/login'),
                  child: const Text('Login'),
                ),
              ],
            ),
    );
  }
}