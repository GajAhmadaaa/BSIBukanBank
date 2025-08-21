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
  bool _isLoggedIn = false;

  @override
  void initState() {
    super.initState();
    _notifications = Future.value([]); // Initialize here
    _checkLoginStatusAndLoadNotifications();
  }

  Future<void> _checkLoginStatusAndLoadNotifications() async {
    final token = await _authService.getToken();
    if (token != null) {
      final customerId = await _authService.getCustomerId();
      if (customerId != null) {
        setState(() {
          _isLoggedIn = true;
          _notifications = _notificationService.getNotifications(customerId);
        });
      } else {
        setState(() {
          _isLoggedIn = false;
        });
      }
    } else {
      setState(() {
        _isLoggedIn = false;
      });
    }
  }

  void _refreshNotifications() async {
    if (_isLoggedIn) {
      final customerId = await _authService.getCustomerId();
      if (customerId != null) {
        setState(() {
          _notifications = _notificationService.getNotifications(customerId);
        });
      }
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
                  final error = snapshot.error;
                  final errorMessage = error?.toString() ?? 'Unknown error';
                  
                  // Handle specific error cases
                  String displayMessage = 'An error occurred while loading your notifications. Please try again.';
                  IconData errorIcon = Icons.error;
                  Color iconColor = Colors.red;
                  
                  if (errorMessage.contains('404')) {
                    displayMessage = 'No notifications found.';
                    errorIcon = Icons.notifications_off;
                    iconColor = Colors.grey;
                  } else if (errorMessage.contains('Network')) {
                    displayMessage = 'Network error. Please check your connection and try again.';
                  } else if (errorMessage.contains('Unauthorized') || errorMessage.contains('401')) {
                    displayMessage = 'Authentication error. Please log in again.';
                  }
                  
                  return Center(
                    child: Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(errorIcon, size: 80, color: iconColor),
                          const SizedBox(height: 16),
                          Text(
                            'Error: $errorMessage',
                            style: const TextStyle(fontSize: 18, color: Colors.red),
                            textAlign: TextAlign.center,
                          ),
                          const SizedBox(height: 16),
                          Text(
                            displayMessage,
                            style: const TextStyle(fontSize: 14, color: Colors.grey),
                            textAlign: TextAlign.center,
                          ),
                          const SizedBox(height: 24),
                          ElevatedButton(
                            onPressed: _checkLoginStatusAndLoadNotifications,
                            child: const Text('Retry'),
                          ),
                        ],
                      ),
                    ),
                  );
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
                            if (context.mounted) {
                              context.push('/order/loi/${notification.loid}');
                            }
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
                    onPressed: () {
                      if (context.mounted) {
                        context.go('/login');
                      }
                    },
                    child: const Text('Login'),
                  ),
                ],
              ),
            ),
    );
  }
}
