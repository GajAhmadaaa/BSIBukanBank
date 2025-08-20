import 'package:karolin/models/order.dart';
import 'package:karolin/models/sales_agreement.dart';
import 'package:karolin/services/api_service.dart';

class OrderService {
  final ApiService _apiService = ApiService();

  Future<LetterOfIntent> getOrderById(int orderId) async {
    final data = await _apiService.get('LetterOfIntent/$orderId');
    return LetterOfIntent.fromJson(data);
  }

  // Methods for SalesAgreement (Unpaid/Paid orders)
  Future<List<SalesAgreement>> getUnpaidAgreements(int customerId) async {
    final data = await _apiService.get('SalesAgreement/customer/$customerId/unpaid');
    return (data as List)
        .map((item) => SalesAgreement.fromJson(item))
        .toList();
  }
  
  Future<List<SalesAgreement>> getPaidAgreements(int customerId) async {
    final data = await _apiService.get('SalesAgreement/customer/$customerId/paid');
    return (data as List)
        .map((item) => SalesAgreement.fromJson(item))
        .toList();
  }
  
  // Method for Pending orders (still using LOI)
  Future<List<LetterOfIntent>> getPendingOrders(int customerId) async {
    // For now, we'll get LOI with status Pending as pending
    final data = await _apiService.get('LetterOfIntent/customer/$customerId/pending');
    return (data as List)
        .map((item) => LetterOfIntent.fromJson(item))
        .toList();
  }
}