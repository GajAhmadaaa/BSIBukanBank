import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/models/order.dart';
import 'package:karolin/services/order_service.dart';

class PendingPage extends StatefulWidget {
  const PendingPage({super.key});

  @override
  State<PendingPage> createState() => _PendingPageState();
}

class _PendingPageState extends State<PendingPage> {
  final OrderService _orderService = OrderService();
  late Future<List<LetterOfIntent>> _pendingOrders;
  final int _customerId = 1; // Placeholder customer ID

  @override
  void initState() {
    super.initState();
    _loadPendingOrders();
  }

  Future<void> _loadPendingOrders() async {
    setState(() {
      _pendingOrders = _orderService.getOrdersByStatus(
        _customerId,
        ['PendingStock', 'ReadyForAgreement'], // Statuses for pending orders
      );
    });
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<LetterOfIntent>>(
      future: _pendingOrders,
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
                Icon(Icons.hourglass_empty, size: 80, color: Colors.grey),
                SizedBox(height: 16),
                Text(
                  'No pending orders.',
                  style: TextStyle(fontSize: 18, color: Colors.grey),
                ),
                Text(
                  'Your pending orders will appear here.',
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
