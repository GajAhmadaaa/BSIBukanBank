import 'package:flutter/material.dart';
import 'package:karolin/services/notification_service.dart';
import 'package:karolin/models/notification.dart';

class NotificationPopupService {
  static final NotificationPopupService _instance = NotificationPopupService._internal();
  factory NotificationPopupService() => _instance;
  NotificationPopupService._internal();

  final NotificationService _notificationService = NotificationService();
  int? _lastCheckedCustomerId;
  Set<int> _shownNotificationIds = {};
  bool _isChecking = false;

  /// Check for new notifications and show popup
  Future<void> checkAndShowNotifications(BuildContext context, int customerId) async {
    // Prevent multiple simultaneous checks
    if (_isChecking) return;
    _isChecking = true;

    try {
      // Only check if customer changed or periodically
      if (_lastCheckedCustomerId != customerId) {
        _lastCheckedCustomerId = customerId;
        _shownNotificationIds.clear();
      }

      final unreadNotifications = await _notificationService.getUnreadNotifications(customerId);
      
      // Filter out notifications that have already been shown
      final newNotifications = unreadNotifications
          .where((notification) => !_shownNotificationIds.contains(notification.customerNotificationId))
          .toList();

      // Show popup for each new notification (with a delay between popups)
      for (int i = 0; i < newNotifications.length; i++) {
        if (context.mounted) {
          await Future.delayed(Duration(milliseconds: i * 1000)); // Stagger popups
          _showNotificationPopup(context, newNotifications[i]);
          _shownNotificationIds.add(newNotifications[i].customerNotificationId);
        }
      }
    } catch (e) {
      // Silently fail to avoid interrupting user experience
      print('Error checking notifications: $e');
    } finally {
      _isChecking = false;
    }
  }

  /// Show a single notification popup
  void _showNotificationPopup(BuildContext context, CustomerNotification notification) {
    // Make sure we're still in a valid context
    if (!context.mounted) return;

    // Use ScaffoldMessenger to show snackbar
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(
              notification.notificationType,
              style: const TextStyle(fontWeight: FontWeight.bold, color: Colors.white),
            ),
            const SizedBox(height: 4),
            Text(
              notification.message,
              style: const TextStyle(color: Colors.white70),
              maxLines: 2,
              overflow: TextOverflow.ellipsis,
            ),
          ],
        ),
        backgroundColor: Colors.blue.shade700,
        duration: const Duration(seconds: 4),
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(8),
        ),
        margin: const EdgeInsets.all(16),
        padding: const EdgeInsets.all(16),
      ),
    );
  }

  /// Clear the cache when user logs out
  void clearCache() {
    _lastCheckedCustomerId = null;
    _shownNotificationIds.clear();
    _isChecking = false;
  }
}