import { Navigate, Outlet } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store/appStore';

const ProtectedRoute = () => {
  const { token } = useSelector((state: RootState) => state.auth);
  const localToken = localStorage.getItem('token');

  // Redux state VEYA localStorage'da token varsa geçir
  if (!token && !localToken) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;