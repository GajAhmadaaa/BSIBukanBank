import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/models/sales_agreement.dart';
import 'package:karolin/services/order_service.dart';
import 'package:karolin/services/auth_service.dart';

class UnpaidPage extends StatefulWidget {
  const UnpaidPage({super.key});

  @override
  State<UnpaidPage> createState() => _UnpaidPageState();
}

class _UnpaidPageState extends State<UnpaidPage> {
  final OrderService _orderService = OrderService();
  final AuthService _authService = AuthService();
  late Future<List<SalesAgreement>> _unpaidAgreements;

  @override
  void initState() {
    super.initState();
    _loadUnpaidAgreements();
  }

  Future<void> _loadUnpaidAgreements() async {
    final customerId = await _authService.getCustomerId();
    if (customerId != null) {
      setState(() {
        _unpaidAgreements = _orderService.getUnpaidAgreements(customerId);
      });
    } else {
      // Handle case where customer ID is not available
      setState(() {
        _unpaidAgreements = Future.error('Customer ID not found');
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<SalesAgreement>>(
      future: _unpaidAgreements,
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
                Icon(Icons.money_off, size: 80, color: Colors.grey),
                SizedBox(height: 16),
                Text(
                  'No unpaid agreements.',
                  style: TextStyle(fontSize: 18, color: Colors.grey),
                ),
                Text(
                  'Agreements awaiting payment will appear here.',
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
