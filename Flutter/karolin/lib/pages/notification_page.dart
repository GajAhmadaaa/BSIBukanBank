import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:karolin/models/notification.dart';
import 'package:karolin/services/auth_service.dart';
import 'package:karolin/services/notification_service.dart';

class NotificationPage extends StatefulWidget {
  const NotificationPage({super.key});

  @override
  State<NotificationPage> createState() => _NotificationPageState();
}

class _NotificationPageState extends State<NotificationPage> {
  final NotificationService _notificationService = NotificationService();
  final AuthService _authService = AuthService();
  late Future<List<CustomerNotification>> _notifications;
  final int _customerId = 1; // Placeholder customer ID
  bool _isLoggedIn = false;

  @override
  void initState() {
    super.initState();
    _checkLoginStatusAndLoadNotifications();
  }

  Future<void> _checkLoginStatusAndLoadNotifications() async {
    final token = await _authService.getToken();
    if (token != null) {
      setState(() {
        _isLoggedIn = true;
        _notifications = _notificationService.getNotifications(_customerId);
      });
    } else {
      setState(() {
        _isLoggedIn = false;
      });
    }
  }

  void _refreshNotifications() {
    if (_isLoggedIn) {
      setState(() {
        _notifications = _notificationService.getNotifications(_customerId);
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Notifications'),
        leading: IconButton( // Add back button
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            if (context.canPop()) {
              context.pop();
            } else {
              context.go('/home');
            }
          },
        ),
        actions: [
          if (_isLoggedIn)
            IconButton(
              icon: const Icon(Icons.refresh),
              onPressed: _refreshNotifications,
            ),
        ],
      ),
      body: _isLoggedIn
          ? FutureBuilder<List<CustomerNotification>>(
              future: _notifications,
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
                        Icon(Icons.notifications_off, size: 80, color: Colors.grey),
                        SizedBox(height: 16),
                        Text(
                          'No notifications yet.',
                          style: TextStyle(fontSize: 18, color: Colors.grey),
                        ),
                        Text(
                          'We\'ll let you know when there\'s something new!',
                          style: TextStyle(fontSize: 14, color: Colors.grey),
                          textAlign: TextAlign.center,
                        ),
                      ],
                    ),
                  );
                } else {
                  final notifications = snapshot.data!;
                  return ListView.builder(
                    itemCount: notifications.length,
                    itemBuilder: (context, index) {
                      final notification = notifications[index];
                      return ListTile(
                        leading: Icon(
                          notification.isRead ? Icons.mark_email_read : Icons.mark_email_unread,
                          color: notification.isRead ? Colors.grey : Theme.of(context).primaryColor,
                        ),
                        title: Text(notification.notificationType),
                        subtitle: Text(notification.message),
                        trailing: Text(
                          '${notification.createdDate.day}/${notification.createdDate.month}/${notification.createdDate.year}',
                        ),
                        onTap: () {
                          // Mark as read and then navigate if applicable
                          if (!notification.isRead) {
                            _notificationService.markAsRead(notification.customerNotificationId).then((_) {
                              _refreshNotifications();
                            });
                          }
                          
                          if (notification.loid != null) {
                            context.push('/order/${notification.loid}');
                          }
                        },
                      );
                    },
                  );
                }
              },
            )
          : Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Text('Please login to view notifications.'),
                  const SizedBox(height: 24),
                  ElevatedButton(
                    onPressed: () => context.go('/login'),
                    child: const Text('Login'),
                  ),
                ],
              ),
            ),
    );
  }
}