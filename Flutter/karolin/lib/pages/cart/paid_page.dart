import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/models/sales_agreement.dart';
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
  late Future<List<SalesAgreement>> _paidAgreements;

  @override
  void initState() {
    super.initState();
    _paidAgreements = Future.value([]); // Initialize with empty list to prevent LateInitializationError
    _loadPaidAgreements();
  }

  Future<void> _loadPaidAgreements() async {
    final customerId = await _authService.getCustomerId();
    if (customerId != null) {
      setState(() {
        _paidAgreements = _orderService.getPaidAgreements(customerId);
      });
    } else {
      // Handle case where customer ID is not available
      setState(() {
        _paidAgreements = Future.value([]); // Return empty list instead of error
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<SalesAgreement>>(
      future: _paidAgreements,
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
                    'An error occurred while loading your agreements. Please try again.',
                    style: TextStyle(fontSize: 14, color: Colors.grey),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 24),
                  ElevatedButton(
                    onPressed: _loadPaidAgreements,
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
                Icon(Icons.check_circle_outline, size: 80, color: Colors.grey),
                SizedBox(height: 16),
                Text(
                  'No paid agreements.',
                  style: TextStyle(fontSize: 18, color: Colors.grey),
                ),
                Text(
                  'Completed agreements will appear here.',
                  style: TextStyle(fontSize: 14, color: Colors.grey),
                  textAlign: TextAlign.center,
                ),
              ],
            ),
          );
        } else {
          final agreements = snapshot.data!;
          return ListView.builder(
            itemCount: agreements.length,
            itemBuilder: (context, index) {
              final agreement = agreements[index];
              return Card(
                margin: const EdgeInsets.symmetric(vertical: 8.0, horizontal: 16.0),
                child: ListTile(
                  title: Text('Agreement ID: ${agreement.id}'),
                  subtitle: Text('Status: ${agreement.status}\nDate: ${agreement.transactionDate.toLocal()}'),
                  trailing: const Icon(Icons.arrow_forward_ios),
                  onTap: () {
                    context.push('/order/${agreement.id}');
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
