import 'package:flutter/material.dart';

Widget buildStatusChip(String status) {
  Color backgroundColor;
  Color textColor = Colors.white; // Default text color

  switch (status.toLowerCase()) {
    case 'pending':
      backgroundColor = Colors.orange;
      break;
    case 'readyforagreement':
      backgroundColor = Colors.blue;
      break;
    case 'unpaid':
      backgroundColor = Colors.red;
      break;
    case 'paid':
      backgroundColor = Colors.green;
      break;
    case 'converted': // For LOI status after conversion
      backgroundColor = Colors.purple;
      break;
    default:
      backgroundColor = Colors.grey;
      break;
  }

  return Chip(
    label: Text(status, style: TextStyle(color: textColor, fontWeight: FontWeight.bold, fontSize: 12.0)), // Reduced font size
    backgroundColor: backgroundColor,
    padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 2), // Reduced padding
    labelPadding: EdgeInsets.zero, // Remove default label padding
  );
}