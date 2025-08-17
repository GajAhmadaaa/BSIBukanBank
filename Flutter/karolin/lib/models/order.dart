class LetterOfIntent {
  final int id;
  final int dealerId;
  final int customerId;
  final int salesPersonId;
  final DateTime loidate;
  final String? paymentMethod;
  final String? note;
  final String status;
  final List<LetterOfIntentDetail> details;

  LetterOfIntent({
    required this.id,
    required this.dealerId,
    required this.customerId,
    required this.salesPersonId,
    required this.loidate,
    this.paymentMethod,
    this.note,
    required this.status,
    required this.details,
  });

  factory LetterOfIntent.fromJson(Map<String, dynamic> json) {
    var detailsList = json['Details'] as List? ?? [];
    List<LetterOfIntentDetail> details =
        detailsList.map((i) => LetterOfIntentDetail.fromJson(i)).toList();

    return LetterOfIntent(
      id: json['Id'],
      dealerId: json['DealerId'],
      customerId: json['CustomerId'],
      salesPersonId: json['SalesPersonId'],
      loidate: DateTime.parse(json['Loidate']),
      paymentMethod: json['PaymentMethod'],
      note: json['Note'],
      status: json['Status'],
      details: details,
    );
  }
}

class LetterOfIntentDetail {
  final int loidetailId;
  final int carId;
  final String carName; // Assuming this will be joined in the backend DTO
  final double agreedPrice;
  final double? discount;
  final String? note;

  LetterOfIntentDetail({
    required this.loidetailId,
    required this.carId,
    required this.carName,
    required this.agreedPrice,
    this.discount,
    this.note,
  });

  factory LetterOfIntentDetail.fromJson(Map<String, dynamic> json) {
    return LetterOfIntentDetail(
      loidetailId: json['LoidetailId'],
      carId: json['CarId'],
      carName: json['CarName'] ?? 'N/A', // Placeholder
      agreedPrice: (json['AgreedPrice'] as num).toDouble(),
      discount: (json['Discount'] as num?)?.toDouble(),
      note: json['Note'],
    );
  }
}
