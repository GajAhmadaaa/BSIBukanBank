import 'dart:convert';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;
import 'package:karolin/config.dart';
import 'package:karolin/models/user.dart';
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
}
