// src/components/ProtectedRoute.tsx
// Angular'daki AuthGuard + canActivateChild mantığının React karşılığı.
// Token varlığı VE expiration kontrolü yaparak koruma sağlar.

import { Navigate, Outlet } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store/appStore';

/**
 * Token'ın süresi dolmuş mu kontrol eder.
 * authSlice token'ı localStorage'a kaydederken expiration'ı kaydetmiyor,
 * bu yüzden Redux state'indeki token varlığını kontrol ediyoruz.
 * Expiration kontrolü eklemek istersen authSlice'a expiration'ı da kaydet.
 */
const isTokenValid = (): boolean => {
  const token = localStorage.getItem('token');
  if (!token) return false;

  // İleride expiration kontrolü eklemek için:
  const expiration = localStorage.getItem('token_expiration');
  if (!expiration) return false;
  return Date.parse(expiration) > Date.now();

  return true;
};

/**
 * Giriş yapılmamış kullanıcıları login'e yönlendiren genel koruma.
 * App.tsx'te tüm korumalı route'ların üstüne sarılır.
 * Angular'daki canActivate / canActivateChild karşılığı.
 */
const ProtectedRoute = () => {
  const { token } = useSelector((state: RootState) => state.auth);

  // Hem Redux state hem localStorage kontrol edilir.
  // Sayfa yenilendiğinde Redux sıfırlanır ama localStorage kalır.
  const isLoggedIn = !!token || isTokenValid();

  if (!isLoggedIn) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;