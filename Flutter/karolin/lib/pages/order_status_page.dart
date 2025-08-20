import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:intl/intl.dart';
import 'package:karolin/models/order.dart';
import 'package:karolin/models/sales_agreement.dart';
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
  late Future<dynamic> _order; // Could be LetterOfIntent or SalesAgreement
  bool _isLoggedIn = false;
  bool _isSalesAgreement = false;
  final DateFormat _dateFormat = DateFormat('dd/MM/yyyy HH:mm');

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
        // For now, we'll try to get as SalesAgreement first, then fallback to LOI
        // In a real implementation, we might want to determine the type from the route or API
        _order = _orderService.getOrderById(widget.orderId); // This still gets LOI
        _isSalesAgreement = false;
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
        title: Text('${_isSalesAgreement ? 'Agreement' : 'Order'} #${widget.orderId}'),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            context.pop();
          },
        ),
      ),
      body: _isLoggedIn
          ? FutureBuilder<dynamic>(
              future: _order,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                } else if (snapshot.hasError) {
                  return Center(child: Text('Error: ${snapshot.error}'));
                } else if (!snapshot.hasData) {
                  return const Center(child: Text('Order/Agreement not found.'));
                } else {
                  if (snapshot.data is LetterOfIntent) {
                    final order = snapshot.data as LetterOfIntent;
                    return _buildLOIView(order);
                  } else if (snapshot.data is SalesAgreement) {
                    final agreement = snapshot.data as SalesAgreement;
                    return _buildSAView(agreement);
                  } else {
                    return const Center(child: Text('Unknown order type.'));
                  }
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

  Widget _buildLOIView(LetterOfIntent order) {
    return Padding(
      padding: const EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('Order ID: ${order.id}', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
          const SizedBox(height: 8),
          Text('Status: ${order.status}'),
          const SizedBox(height: 8),
          Text('Date: ${_dateFormat.format(order.loidate)}'),
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

  Widget _buildSAView(SalesAgreement agreement) {
    return Padding(
      padding: const EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('Agreement ID: ${agreement.id}', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
          const SizedBox(height: 8),
          Text('Status: ${agreement.status ?? 'Unknown'}'),
          const SizedBox(height: 8),
          Text('Date: ${_dateFormat.format(agreement.transactionDate)}'),
          const SizedBox(height: 8),
          Text('Total Amount: \$${agreement.totalAmount?.toStringAsFixed(2) ?? '0.00'}'),
          const SizedBox(height: 16),
          const Text('Details:', style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
          Expanded(
            child: ListView.builder(
              itemCount: agreement.details.length,
              itemBuilder: (context, index) {
                final detail = agreement.details[index];
                return Card(
                  margin: const EdgeInsets.symmetric(vertical: 8.0),
                  child: ListTile(
                    title: Text(detail.carName),
                    subtitle: Text('Price: \$${detail.agreedPrice.toStringAsFixed(2)}'),
                    trailing: Text('Discount: \$${detail.discount != null ? detail.discount!.toStringAsFixed(2) : '0.00'}'),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}