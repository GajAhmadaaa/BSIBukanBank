import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/models/order.dart';
import 'package:karolin/services/order_service.dart';
import 'package:karolin/services/auth_service.dart';

class PaidPage extends StatefulWidget {
  const PaidPage({super.key});

  @override
  State<PaidPage> createState() => _PaidPageState();
}

class _PaidPageState extends State<PaidPage> {
  final OrderService _orderService = OrderService();
  final AuthService _authService = AuthService();
  late Future<List<LetterOfIntent>> _paidOrders;

  @override
  void initState() {
    super.initState();
    _loadPaidOrders();
  }

  Future<void> _loadPaidOrders() async {
    final customerId = await _authService.getCustomerId();
    if (customerId != null) {
      setState(() {
        _paidOrders = _orderService.getPaidOrders(customerId);
      });
    } else {
      // Handle case where customer ID is not available
      setState(() {
        _paidOrders = Future.error('Customer ID not found');
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<LetterOfIntent>>(
      future: _paidOrders,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Center(child: CircularProgressIndicator());
        } else if (snapshot.hasError) {
          return Center(child: Text('Error: ${snapshot.error}'));
        } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
          return const Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(Icons.check_circle_outline, size: 80, color: Colors.grey),
                SizedBox(height: 16),
                Text(
                  'No paid orders.',
                  style: TextStyle(fontSize: 18, color: Colors.grey),
                ),
                Text(
                  'Completed orders will appear here.',
                  style: TextStyle(fontSize: 14, color: Colors.grey),
                  textAlign: TextAlign.center,
                ),
              ],
            ),
          );
        } else {
          final orders = snapshot.data!;
          return ListView.builder(
            itemCount: orders.length,
            itemBuilder: (context, index) {
              final order = orders[index];
              return Card(
                margin: const EdgeInsets.symmetric(vertical: 8.0, horizontal: 16.0),
                child: ListTile(
                  title: Text('Order ID: ${order.id}'),
                  subtitle: Text('Status: ${order.status}\nDate: ${order.loidate.toLocal()}'),
                  trailing: const Icon(Icons.arrow_forward_ios),
                  onTap: () {
                    context.push('/order/${order.id}');
                  },
                ),
              );
            },
          );
        }
      },
    );
  }
}
