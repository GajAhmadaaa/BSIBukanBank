import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:intl/intl.dart';
import 'package:karolin/models/order.dart';
import 'package:karolin/models/sales_agreement.dart';
import 'package:karolin/services/auth_service.dart';
import 'package:karolin/services/order_service.dart';

class OrderStatusPage extends StatefulWidget {
  final int orderId;
  final String orderType; // Added orderType

  const OrderStatusPage({super.key, required this.orderId, required this.orderType}); // Modified constructor

  @override
  State<OrderStatusPage> createState() => _OrderStatusPageState();
}

class _OrderStatusPageState extends State<OrderStatusPage> {
  final OrderService _orderService = OrderService();
  final AuthService _authService = AuthService();
  late Future<dynamic> _order; // Could be LetterOfIntent or SalesAgreement
  bool _isLoggedIn = false;
  // bool _isSalesAgreement = false; // Removed
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
        // Use orderType to fetch the correct data
        if (widget.orderType == 'loi') {
          _order = _orderService.getLOIById(widget.orderId);
        } else if (widget.orderType == 'sa') {
          _order = _orderService.getSalesAgreementById(widget.orderId);
        } else {
          // Handle unknown type, perhaps set _order to an error Future
          _order = Future.error('Unknown order type: ${widget.orderType}');
        }
      });
    } else {
      setState(() {
        _isLoggedIn = false;
      });
    }
  }

  Future<void> _convertLOIToAgreement(int loiId) async {
    try {
      // Show loading indicator
      showDialog(
        context: context,
        barrierDismissible: false,
        builder: (BuildContext context) {
          return const AlertDialog(
            content: Row(
              children: [
                CircularProgressIndicator(),
                SizedBox(width: 20),
                Text("Converting to Agreement..."),
              ],
            ),
          );
        },
      );

      // Call the service to convert LOI to Agreement
      final SalesAgreement agreement = await _orderService.convertLOIToAgreement(loiId);
      
      // Hide loading indicator
      Navigator.of(context).pop();
      
      // Show success message and navigate to pending orders page
      if (context.mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Successfully converted to agreement')),
        );

        // Navigate to unpaid orders page
        context.go('/cart/unpaid');
      }
    } catch (error) {
      // Hide loading indicator
      Navigator.of(context).pop();
      
      // Show error message
      if (context.mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Failed to convert to agreement: $error')),
        );
      }
    }
  }
  
  Future<void> _markAgreementAsPaid(int agreementId) async {
    try {
      // Show loading indicator
      showDialog(
        context: context,
        barrierDismissible: false,
        builder: (BuildContext context) {
          return const AlertDialog(
            content: Row(
              children: [
                CircularProgressIndicator(),
                SizedBox(width: 20),
                Text("Processing payment..."),
              ],
            ),
          );
        },
      );

      // Get the current agreement data first
      final SalesAgreement currentAgreement = await _orderService.getSalesAgreementById(agreementId);
      
      // Process payment first
      await _orderService.processPayment(agreementId, currentAgreement.totalAmount ?? 0);
      
      // Then mark agreement as paid
      final SalesAgreement updatedAgreement = await _orderService.markAgreementAsPaid(agreementId);
      
      // Hide loading indicator
      Navigator.of(context).pop();
      
      // Refresh the page to show updated status
      setState(() {
        _order = Future.value(updatedAgreement);
      });
      
      // Show success message
      if (context.mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Payment processed successfully')),
        );
      }
    } catch (error) {
      // Hide loading indicator
      Navigator.of(context).pop();
      
      // Show error message
      if (context.mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Failed to process payment: $error')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('${widget.orderType == 'sa' ? 'Agreement' : 'Order'} #${widget.orderId}'), // Modified title
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            if (context.canPop()) {
              context.pop();
            } else {
              context.go('/home');
            }
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
    final totalAmount = order.details.fold<double>(
        0, (sum, item) => sum + item.agreedPrice - (item.discount ?? 0));
    final currencyFormat = NumberFormat.currency(locale: 'id_ID', symbol: 'Rp ', decimalDigits: 0);

    return SingleChildScrollView(
      padding: const EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Card(
            elevation: 4,
            margin: const EdgeInsets.symmetric(vertical: 8.0),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Order #${order.id}',
                    style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 16),
                  _buildDetailRow('Status', '', customWidget: _buildStatusChip(order.status)), // Modified
                  _buildDetailRow('Date', _dateFormat.format(order.loidate)),
                  _buildDetailRow('Customer ID', order.customerId.toString()),
                  _buildDetailRow('Sales Person ID', order.salesPersonId.toString()),
                  if (order.paymentMethod != null)
                    _buildDetailRow('Payment Method', order.paymentMethod!),
                  if (order.note != null)
                    _buildDetailRow('Note', order.note!),
                  const Divider(height: 32),
                  _buildDetailRow(
                    'Total Amount',
                    currencyFormat.format(totalAmount),
                    isBold: true,
                  ),
                  const SizedBox(height: 16),
                  Center(
                    child: ElevatedButton(
                      onPressed: order.status.toLowerCase() == 'readyforagreement' 
                          ? () {
                              _convertLOIToAgreement(order.id);
                            }
                          : null,
                      child: const Text('Proceed to Agreement'),
                    ),
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 24),
          Text(
            'Items',
            style: Theme.of(context).textTheme.titleLarge,
          ),
          const SizedBox(height: 8),
          ListView.builder(
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
            itemCount: order.details.length,
            itemBuilder: (context, index) {
              final detail = order.details[index];
              final itemTotal = detail.agreedPrice - (detail.discount ?? 0);
              return Card(
                margin: const EdgeInsets.symmetric(vertical: 4.0),
                child: ListTile(
                  title: Text(detail.carName,
                      style: const TextStyle(fontWeight: FontWeight.bold)),
                  subtitle: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('Price: ${currencyFormat.format(detail.agreedPrice)}'),
                      Text('Discount: ${currencyFormat.format(detail.discount ?? 0)}'),
                    ],
                  ),
                  trailing: Text(
                    currencyFormat.format(itemTotal),
                    style: const TextStyle(
                        fontSize: 16, fontWeight: FontWeight.bold),
                  ),
                ),
              );
            },
          ),
          const SizedBox(height: 24),
        ],
      ),
    );
  }

  Widget _buildSAView(SalesAgreement agreement) {
    final currencyFormat = NumberFormat.currency(locale: 'id_ID', symbol: 'Rp ', decimalDigits: 0);
    return SingleChildScrollView(
      padding: const EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Card(
            elevation: 4,
            margin: const EdgeInsets.symmetric(vertical: 8.0),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Agreement #${agreement.id}',
                    style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 16),
                  _buildDetailRow('Status', '', customWidget: _buildStatusChip(agreement.status ?? 'Unknown')),
                  _buildDetailRow('Date', _dateFormat.format(agreement.transactionDate)),
                  _buildDetailRow('Dealer ID', agreement.dealerId.toString()),
                  _buildDetailRow('Customer ID', agreement.customerId.toString()),
                  _buildDetailRow('Sales Person ID', agreement.salesPersonId.toString()),
                  _buildDetailRow('LOI ID', agreement.loiid?.toString() ?? '0'),
                  const Divider(height: 32),
                  _buildDetailRow(
                    'Total Amount',
                    currencyFormat.format(agreement.totalAmount ?? 0),
                    isBold: true,
                  ),
                  const SizedBox(height: 16),
                  Center(
                    child: ElevatedButton(
                      onPressed: agreement.status?.toLowerCase() == 'unpaid' 
                          ? () {
                              _markAgreementAsPaid(agreement.id);
                            }
                          : null,
                      child: const Text('Proceed Payment'),
                    ),
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 24),
          Text(
            'Items',
            style: Theme.of(context).textTheme.titleLarge,
          ),
          const SizedBox(height: 8),
          ListView.builder(
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
            itemCount: agreement.details.length,
            itemBuilder: (context, index) {
              final detail = agreement.details[index];
              final itemTotal = detail.agreedPrice - (detail.discount ?? 0);
              return Card(
                margin: const EdgeInsets.symmetric(vertical: 4.0),
                child: ListTile(
                  title: Text(detail.carName,
                      style: const TextStyle(fontWeight: FontWeight.bold)),
                  subtitle: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('Price: ${currencyFormat.format(detail.agreedPrice)}'),
                      if (detail.discount != null && detail.discount! > 0)
                        Text('Discount: ${currencyFormat.format(detail.discount ?? 0)}'),
                      if (detail.note != null)
                        Text('Note: ${detail.note}'),
                    ],
                  ),
                  trailing: Text(
                    currencyFormat.format(itemTotal),
                    style: const TextStyle(
                        fontSize: 16, fontWeight: FontWeight.bold),
                  ),
                ),
              );
            },
          ),
          const SizedBox(height: 24),
        ],
      ),
    );
  }

  // New helper method for status chip
  Widget _buildStatusChip(String status) {
    Color backgroundColor;
    Color textColor = Colors.white; // Default text color

    switch (status.toLowerCase()) {
      case 'pending':
        backgroundColor = Colors.orange;
        break;
      case 'readyforagreement':
        backgroundColor = Colors.blue;
        break;
      case 'unpaid':
        backgroundColor = Colors.red;
        break;
      case 'paid':
        backgroundColor = Colors.green;
        break;
      case 'converted': // For LOI status after conversion
        backgroundColor = Colors.purple;
        break;
      default:
        backgroundColor = Colors.grey;
        break;
    }

    return Chip(
      label: Text(status, style: TextStyle(color: textColor, fontWeight: FontWeight.bold)),
      backgroundColor: backgroundColor,
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      labelPadding: EdgeInsets.zero,
    );
  }

  // Modified _buildDetailRow to accept customWidget
  Widget _buildDetailRow(String label, String value, {bool isBold = false, Widget? customWidget}) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text('$label:', style: Theme.of(context).textTheme.bodyMedium?.copyWith(color: Colors.grey[600])),
          customWidget ?? Text(
            value,
            style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                fontWeight: isBold ? FontWeight.bold : FontWeight.normal,
            ),
          ),
        ],
      ),
    );
  }
}