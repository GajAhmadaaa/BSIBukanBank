import 'package:go_router/go_router.dart';
import 'package:flutter/material.dart';
import 'pages/splash_page.dart';
import 'pages/home_page.dart';
import 'pages/auth/login_page.dart';
import 'pages/auth/register_page.dart';
import 'pages/profile_page.dart';
import 'pages/notification_page.dart';
import 'pages/cart/cart_page.dart';
import 'pages/cart/pending_page.dart';
import 'pages/cart/unpaid_page.dart';
import 'pages/cart/paid_page.dart';
import 'pages/order_status_page.dart';

final GoRouter appRouter = GoRouter(
  routes: <RouteBase>[
    GoRoute(
      path: '/',
      builder: (BuildContext context, GoRouterState state) {
        return const SplashPage();
      },
    ),
    GoRoute(
      path: '/home',
      builder: (BuildContext context, GoRouterState state) {
        return HomePage();
      },
    ),
    GoRoute(
      path: '/login',
      builder: (BuildContext context, GoRouterState state) {
        return const LoginPage();
      },
    ),
    GoRoute(
      path: '/register',
      builder: (BuildContext context, GoRouterState state) {
        return const RegisterPage();
      },
    ),
    GoRoute(
      path: '/profile',
      builder: (BuildContext context, GoRouterState state) {
        return const ProfilePage();
      },
    ),
    GoRoute(
      path: '/notification',
      builder: (BuildContext context, GoRouterState state) {
        return const NotificationPage();
      },
    ),
    GoRoute(
      path: '/order/:id',
      builder: (BuildContext context, GoRouterState state) {
        final id = int.parse(state.pathParameters['id']!);
        return OrderStatusPage(orderId: id);
      },
    ),
    ShellRoute(
      builder: (BuildContext context, GoRouterState state, Widget child) {
        return CartPage(state: state, child: child);
      },
      routes: <RouteBase>[
        GoRoute(
          path: '/cart/pending',
          builder: (BuildContext context, GoRouterState state) {
            return const PendingPage();
          },
        ),
        GoRoute(
          path: '/cart/unpaid',
          builder: (BuildContext context, GoRouterState state) {
            return const UnpaidPage();
          },
        ),
        GoRoute(
          path: '/cart/paid',
          builder: (BuildContext context, GoRouterState state) {
            return const PaidPage();
          },
        ),
      ],
    ),
  ],
);