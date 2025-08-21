import 'package:karolin/models/order.dart';
import 'package:karolin/models/sales_agreement.dart';
import 'package:karolin/services/api_service.dart';

class OrderService {
  final ApiService _apiService = ApiService();

  Future<LetterOfIntent> getLOIById(int orderId) async { // Renamed
    final data = await _apiService.get('LetterOfIntent/$orderId');
    return LetterOfIntent.fromJson(data);
  }

  Future<SalesAgreement> getSalesAgreementById(int agreementId) async { // New method
    final data = await _apiService.get('SalesAgreement/$agreementId');
    return SalesAgreement.fromJson(data);
  }

  // Method to convert LOI to Sales Agreement
  Future<SalesAgreement> convertLOIToAgreement(int loiId) async {
    final data = await _apiService.post('SalesAgreement/convert-from-loi/$loiId', {});
    return SalesAgreement.fromJson(data);
  }
  
  // Method to mark Sales Agreement as paid
  Future<SalesAgreement> markAgreementAsPaid(int agreementId) async {
    final data = await _apiService.put('SalesAgreement/$agreementId/mark-as-paid', {});
    return SalesAgreement.fromJson(data);
  }
  
  // Method to process payment with Cash payment type
  Future<void> processPayment(int agreementId, double amount) async {
    final paymentData = {
      'SalesAgreementID': agreementId,
      'PaymentAmount': amount.toDouble(),
      'PaymentDate': DateTime.now().toIso8601String(),
      'PaymentType': 'Cash' // Hardcoded to Cash as per requirements
    };
    
    print('Sending payment data: $paymentData');
    await _apiService.post('Payment', paymentData);
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