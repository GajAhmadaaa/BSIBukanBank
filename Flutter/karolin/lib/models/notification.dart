class CustomerNotification {
  final int customerNotificationId;
  final int customerId;
  final int? loid;
  final int? salesAgreementId;
  final String notificationType;
  final String message;
  final DateTime createdDate;
  final DateTime? readDate;
  final bool isRead;

  CustomerNotification({
    required this.customerNotificationId,
    required this.customerId,
    this.loid,
    this.salesAgreementId,
    required this.notificationType,
    required this.message,
    required this.createdDate,
    this.readDate,
    required this.isRead,
  });

  factory CustomerNotification.fromJson(Map<String, dynamic> json) {
    return CustomerNotification(
      customerNotificationId: json['CustomerNotificationId'],
      customerId: json['CustomerId'],
      loid: json['Loid'],
      salesAgreementId: json['SalesAgreementId'],
      notificationType: json['NotificationType'],
      message: json['Message'],
      createdDate: DateTime.parse(json['CreatedDate']),
      readDate: json['ReadDate'] != null ? DateTime.parse(json['ReadDate']) : null,
      isRead: json['IsRead'],
    );
  }
}