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
  
  // More efficient methods that directly call API endpoints for specific statuses
  Future<List<LetterOfIntent>> getPendingOrders(int customerId) async {
    final data = await _apiService.get('LetterOfIntent/customer/$customerId/pending');
    return (data as List)
        .map((item) => LetterOfIntent.fromJson(item))
        .toList();
  }
  
  Future<List<LetterOfIntent>> getUnpaidOrders(int customerId) async {
    final data = await _apiService.get('LetterOfIntent/customer/$customerId/unpaid');
    return (data as List)
        .map((item) => LetterOfIntent.fromJson(item))
        .toList();
  }
  
  Future<List<LetterOfIntent>> getPaidOrders(int customerId) async {
    final data = await _apiService.get('LetterOfIntent/customer/$customerId/paid');
    return (data as List)
        .map((item) => LetterOfIntent.fromJson(item))
        .toList();
  }
}