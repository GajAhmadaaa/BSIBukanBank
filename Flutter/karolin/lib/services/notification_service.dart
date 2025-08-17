import 'package:karolin/models/notification.dart';
import 'package:karolin/services/api_service.dart';

class NotificationService {
  final ApiService _apiService = ApiService();

  Future<List<CustomerNotification>> getNotifications(int customerId) async {
    final data = await _apiService.get('Notification/$customerId');
    return (data as List)
        .map((item) => CustomerNotification.fromJson(item))
        .toList();
  }

  Future<List<CustomerNotification>> getUnreadNotifications(int customerId) async {
    final data = await _apiService.get('Notification/$customerId/unread');
    return (data as List)
        .map((item) => CustomerNotification.fromJson(item))
        .toList();
  }

  Future<void> markAsRead(int notificationId) async {
    await _apiService.put('Notification/$notificationId/read', {});
  }

  Future<void> markAllAsRead(int customerId) async {
    await _apiService.put('Notification/$customerId/read-all', {});
  }
}
