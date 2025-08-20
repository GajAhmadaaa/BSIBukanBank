import 'dart:convert';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;
import 'package:karolin/config.dart';
import 'package:karolin/models/user.dart';
import 'package:karolin/models/customer.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

class AuthService {
  final _storage = const FlutterSecureStorage();
  final String _baseUrl = AppConfig.baseUrl;
  static const String _tokenKey = 'jwt_token';

  // Helper to parse error messages from API response
  String _parseErrorMessage(http.Response response) {
    try {
      final Map<String, dynamic> errorData = json.decode(response.body);
      if (errorData.containsKey('Message')) {
        return errorData['Message'];
      } else if (errorData.containsKey('title')) { // Common for ASP.NET Core ProblemDetails
        return errorData['title'];
      } else if (errorData.containsKey('errors')) { // For validation errors
        final errors = errorData['errors'] as Map<String, dynamic>;
        return errors.values.expand((e) => e as List).join('\n');
      }
      return 'An unknown error occurred (Status: ${response.statusCode})';
    } catch (e) {
      return 'Failed to parse error response (Status: ${response.statusCode})';
    }
  }

  Future<String?> getToken() async {
    return await _storage.read(key: _tokenKey);
  }

  Future<void> saveToken(String token) async {
    await _storage.write(key: _tokenKey, value: token);
  }

  Future<void> clearToken() async {
    await _storage.delete(key: _tokenKey);
  }

  Future<User> login(String email, String password) async {
    final response = await http.post(
      Uri.parse('$_baseUrl/Usman/login'),
      headers: {'Content-Type': 'application/json'},
      body: json.encode({'email': email, 'password': password}),
    );

    if (response.statusCode == 200) {
      final data = json.decode(response.body);
      final user = User.fromJson(data);
      await saveToken(user.token);
      return user;
    } else {
      throw Exception(_parseErrorMessage(response));
    }
  }

  Future<void> logout() async {
    await clearToken();
  }

  Future<void> register(String email, String password, String name) async {
    final response = await http.post(
      Uri.parse('$_baseUrl/Registration/customer'), // Correct endpoint
      headers: {'Content-Type': 'application/json'},
      body: json.encode({
        'Email': email,
        'Password': password,
        'CustomerData': {
          'Name': name,
          'Address': '', // Opsional, bisa diisi jika ada di UI
          'PhoneNumber': '' // Opsional, bisa diisi jika ada di UI
        }
      }),
    );

    if (response.statusCode != 201) {
      throw Exception(_parseErrorMessage(response));
    }
  }

  Future<String?> getUserEmail() async {
    final token = await getToken();
    if (token != null) {
      Map<String, dynamic> decodedToken = JwtDecoder.decode(token);
      // The key for email in the JWT payload is "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
      // as it is a standard claim type.
      return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
    }
    return null;
  }
  
  Future<int?> getCustomerId() async {
    final token = await getToken();
    if (token != null) {
      // First try to get customer ID from JWT token
      Map<String, dynamic> decodedToken = JwtDecoder.decode(token);
      
      // The key for customer ID in the JWT payload might be custom
      // Let's check for common patterns
      final customerIdFromToken = decodedToken['CustomerId'] ?? 
                        decodedToken['customerid'] ??
                        decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      
      if (customerIdFromToken != null) {
        // If it's a string that represents a number, parse it
        if (customerIdFromToken is String) {
          return int.tryParse(customerIdFromToken);
        }
        // If it's already a number, convert it
        if (customerIdFromToken is int) {
          return customerIdFromToken;
        }
        if (customerIdFromToken is num) {
          return customerIdFromToken.toInt();
        }
      }
      
      // If not found in token, fetch customer by email and get ID
      try {
        // Get email from token - try multiple possible keys
        final email = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? 
                     decodedToken['email'];
        if (email != null) {
          final response = await http.get(
            Uri.parse('$_baseUrl/Customer/email/$email'),
            headers: {
              'Content-Type': 'application/json',
              'Authorization': 'Bearer $token',
            },
          );
          
          if (response.statusCode == 200) {
            final data = json.decode(response.body);
            // Assuming the API returns a customer object with an Id field
            final customerId = data['CustomerId'] ?? data['Id'];
            if (customerId is int) {
              return customerId;
            }
            if (customerId is String) {
              return int.tryParse(customerId);
            }
            return customerId as int?;
          }
        }
      } catch (e) {
        print('Error fetching customer by email: $e');
      }
    }
    return null;
  }
  
  Future<Customer?> getCustomerDetails() async {
    final token = await getToken();
    if (token != null) {
      try {
        // First try to get customer details from JWT token
        Map<String, dynamic> decodedToken = JwtDecoder.decode(token);
        
        // Try to get customer info directly from token
        final customerIdFromToken = decodedToken['CustomerId'] ?? 
                          decodedToken['customerid'] ??
                          decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        
        final customerNameFromToken = decodedToken['CustomerName'] ?? 
                            decodedToken['customername'] ??
                            decodedToken['name'];
        
        final customerEmailFromToken = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? 
                             decodedToken['email'];
        
        // If we have enough info from token, create customer object
        if (customerIdFromToken != null && customerEmailFromToken != null) {
          int? customerId;
          if (customerIdFromToken is String) {
            customerId = int.tryParse(customerIdFromToken);
          } else if (customerIdFromToken is int) {
            customerId = customerIdFromToken;
          } else if (customerIdFromToken is num) {
            customerId = customerIdFromToken.toInt();
          }
          
          if (customerId != null) {
            return Customer(
              id: customerId,
              name: customerNameFromToken ?? customerEmailFromToken,
              email: customerEmailFromToken,
              address: null,
              phoneNumber: null,
            );
          }
        }
        
        // If not found in token, fetch customer by email
        final email = customerEmailFromToken;
        if (email != null) {
          final response = await http.get(
            Uri.parse('$_baseUrl/Customer/email/$email'),
            headers: {
              'Content-Type': 'application/json',
              'Authorization': 'Bearer $token',
            },
          );
          
          if (response.statusCode == 200) {
            final data = json.decode(response.body);
            return Customer.fromJson(data);
          }
        }
      } catch (e) {
        print('Error fetching customer details: $e');
      }
    }
    return null;
  }
}
