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
    final token = await _authService.getToken();
    final response = await http.get(
      Uri.parse('$_baseUrl/$endpoint'),
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
    );

    if (response.statusCode == 200) {
      return json.decode(response.body);
    } else {
      throw Exception(_parseErrorMessage(response));
    }
  }

  Future<dynamic> post(String endpoint, Map<String, dynamic> data) async {
    final token = await _authService.getToken();
    final response = await http.post(
      Uri.parse('$_baseUrl/$endpoint'),
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: json.encode(data),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return json.decode(response.body);
    } else {
      throw Exception(_parseErrorMessage(response));
    }
  }
  
  Future<dynamic> put(String endpoint, Map<String, dynamic> data) async {
    final token = await _authService.getToken();
    final response = await http.put(
      Uri.parse('$_baseUrl/$endpoint'),
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: json.encode(data),
    );

    if (response.statusCode == 200) {
      return json.decode(response.body);
    } else {
      throw Exception(_parseErrorMessage(response));
    }
  }
}