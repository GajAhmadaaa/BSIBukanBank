import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/models/order.dart';
import 'package:karolin/services/order_service.dart';
import 'package:karolin/services/auth_service.dart';
import 'package:karolin/widgets/status_chip.dart';

class PendingPage extends StatefulWidget {
  const PendingPage({super.key});

  @override
  State<PendingPage> createState() => _PendingPageState();
}

class _PendingPageState extends State<PendingPage> {
  final OrderService _orderService = OrderService();
  final AuthService _authService = AuthService();
  late Future<List<LetterOfIntent>> _pendingOrders;

  @override
  void initState() {
    super.initState();
    _pendingOrders = Future.value([]); // Initialize with empty list to prevent LateInitializationError
    _loadPendingOrders();
  }

  Future<void> _loadPendingOrders() async {
    final customerId = await _authService.getCustomerId();
    if (customerId != null) {
      setState(() {
        _pendingOrders = _orderService.getPendingOrders(customerId);
      });
    } else {
      // Handle case where customer ID is not available
      setState(() {
        _pendingOrders = Future.value([]); // Return empty list instead of error
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<LetterOfIntent>>(
      future: _pendingOrders,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Center(child: CircularProgressIndicator());
        } else if (snapshot.hasError) {
          return Center(
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Icon(Icons.error, size: 80, color: Colors.red),
                  const SizedBox(height: 16),
                  Text(
                    'Error: ${snapshot.error}',
                    style: const TextStyle(fontSize: 18, color: Colors.red),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 16),
                  const Text(
                    'An error occurred while loading your orders. Please try again.',
                    style: TextStyle(fontSize: 14, color: Colors.grey),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 24),
                  ElevatedButton(
                    onPressed: _loadPendingOrders,
                    child: const Text('Retry'),
                  ),
                ],
              ),
            ),
          );
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
                  subtitle: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        children: [
                          const Text('Status: '),
                          buildStatusChip(order.status),
                        ],
                      ),
                      Text('Date: ${order.loidate.toLocal()}'),
                    ],
                  ),
                  trailing: const Icon(Icons.arrow_forward_ios),
                  onTap: () {
                    if (context.mounted) {
                      context.push('/order/loi/${order.id}');
                    }
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
