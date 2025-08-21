import 'package:karolin/config.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:karolin/services/auth_service.dart'; // Assuming auth_service will provide the token

class ApiService {
  final String _baseUrl = AppConfig.baseUrl;
  final AuthService _authService = AuthService(); // Simple instantiation for now

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

  Future<dynamic> get(String endpoint) async {
    try {
      final token = await _authService.getToken();
      final response = await http.get(
        Uri.parse('$_baseUrl/$endpoint'),
        headers: {
          'Content-Type': 'application/json',
          if (token != null) 'Authorization': 'Bearer $token',
        },
      );

      if (response.statusCode == 200) {
        return json.decode(response.body);
      } else {
        throw Exception(_parseErrorMessage(response));
      }
    } catch (e) {
      // If it's already an Exception we threw, rethrow it
      if (e is Exception) {
        rethrow;
      }
      // Otherwise, wrap it
      throw Exception('Network error: $e');
    }
  }

  Future<dynamic> post(String endpoint, Map<String, dynamic> data) async {
    try {
      final token = await _authService.getToken();
      final response = await http.post(
        Uri.parse('$_baseUrl/$endpoint'),
        headers: {
          'Content-Type': 'application/json',
          if (token != null) 'Authorization': 'Bearer $token',
        },
        body: json.encode(data),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        return json.decode(response.body);
      } else {
        // For payment endpoint, if we get 400 but data might still be recorded
        if (endpoint == 'Payment' && response.statusCode == 400) {
          // Try to parse the response
          try {
            final responseBody = json.decode(response.body);
            // If it contains any success indicators, we might still want to proceed
            if (responseBody is Map) {
              // Check for common success patterns
              if ((responseBody.containsKey('Message') && 
                   (responseBody['Message'].toString().toLowerCase().contains('success') || 
                    responseBody['Message'].toString().toLowerCase().contains('processed'))) ||
                  responseBody.containsKey('PaymentHistoryID') ||
                  responseBody.containsKey('paymentHistoryId')) {
                return responseBody; // Return the response instead of throwing error
              }
            }
          } catch (parseError) {
            // If we can't parse, continue with normal error handling
          }
          // Still throw the error but caller can decide how to handle it
          throw Exception('Payment API returned 400: ${_parseErrorMessage(response)}');
        }
        throw Exception(_parseErrorMessage(response));
      }
    } catch (e) {
      // If it's already an Exception we threw, rethrow it
      if (e is Exception) {
        rethrow;
      }
      // Otherwise, wrap it
      throw Exception('Network error: $e');
    }
  }
  
  Future<dynamic> put(String endpoint, Map<String, dynamic> data) async {
    try {
      final token = await _authService.getToken();
      final response = await http.put(
        Uri.parse('$_baseUrl/$endpoint'),
        headers: {
          'Content-Type': 'application/json',
          if (token != null) 'Authorization': 'Bearer $token',
        },
        body: json.encode(data),
      );

      if (response.statusCode == 200) {
        return json.decode(response.body);
      } else {
        throw Exception(_parseErrorMessage(response));
      }
    } catch (e) {
      // If it's already an Exception we threw, rethrow it
      if (e is Exception) {
        rethrow;
      }
      // Otherwise, wrap it
      throw Exception('Network error: $e');
    }
  }
}