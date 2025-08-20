class Customer {
  final int id;
  final String name;
  final String email;
  final String? address;
  final String? phoneNumber;

  Customer({
    required this.id,
    required this.name,
    required this.email,
    this.address,
    this.phoneNumber,
  });

  factory Customer.fromJson(Map<String, dynamic> json) {
    return Customer(
      id: json['CustomerId'],
      name: json['Name'],
      email: json['Email'],
      address: json['Address'],
      phoneNumber: json['PhoneNumber'],
    );
  }
}