class SalesAgreement {
  final int id;
  final int dealerId;
  final int customerId;
  final int salesPersonId;
  final int? loiid;
  final DateTime transactionDate;
  final double? totalAmount;
  final String? status;
  final List<SalesAgreementDetail> details;

  SalesAgreement({
    required this.id,
    required this.dealerId,
    required this.customerId,
    required this.salesPersonId,
    this.loiid,
    required this.transactionDate,
    this.totalAmount,
    this.status,
    required this.details,
  });

  factory SalesAgreement.fromJson(Map<String, dynamic> json) {
    var detailsList = json['Details'] as List? ?? [];
    List<SalesAgreementDetail> details =
        detailsList.map((i) => SalesAgreementDetail.fromJson(i)).toList();

    return SalesAgreement(
      id: json['Id'],
      dealerId: json['DealerId'],
      customerId: json['CustomerId'],
      salesPersonId: json['SalesPersonId'],
      loiid: json['Loid'],
      transactionDate: DateTime.parse(json['TransactionDate']),
      totalAmount: (json['TotalAmount'] as num?)?.toDouble(),
      status: json['Status'],
      details: details,
    );
  }
}

class SalesAgreementDetail {
  final int salesAgreementDetailId;
  final int carId;
  final String carName;
  final double agreedPrice;
  final double? discount;
  final String? note;

  SalesAgreementDetail({
    required this.salesAgreementDetailId,
    required this.carId,
    required this.carName,
    required this.agreedPrice,
    this.discount,
    this.note,
  });

  factory SalesAgreementDetail.fromJson(Map<String, dynamic> json) {
    return SalesAgreementDetail(
      salesAgreementDetailId: json['SalesAgreementDetailId'],
      carId: json['CarId'],
      carName: json['CarName'] ?? 'N/A',
      agreedPrice: (json['AgreedPrice'] as num).toDouble(),
      discount: (json['Discount'] as num?)?.toDouble(),
      note: json['Note'],
    );
  }
}