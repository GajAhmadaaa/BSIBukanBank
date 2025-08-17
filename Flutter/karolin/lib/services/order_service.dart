import 'package:karolin/models/order.dart';
import 'package:karolin/services/api_service.dart';

class OrderService {
  final ApiService _apiService = ApiService();

  Future<LetterOfIntent> getOrderById(int orderId) async {
    final data = await _apiService.get('LetterOfIntent/$orderId');
    return LetterOfIntent.fromJson(data);
  }

  Future<List<LetterOfIntent>> getOrdersForCustomer(int customerId) async {
    // Assuming an endpoint to get all orders for a customer
    final data = await _apiService.get('LetterOfIntent/customer/$customerId');
    return (data as List)
        .map((item) => LetterOfIntent.fromJson(item))
        .toList();
  }

  Future<List<LetterOfIntent>> getOrdersByStatus(int customerId, List<String> statuses) async {
    final allOrders = await getOrdersForCustomer(customerId);
    return allOrders.where((order) => statuses.contains(order.status)).toList();
  }
}