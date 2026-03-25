import { Navigate, Outlet } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store/appStore';

const AdminRoute = () => {
  const { user, token } = useSelector((state: RootState) => state.auth);

  // Giriş yapılmamışsa login'e yönlendir
  if (!token || !user) {
    return <Navigate to="/login" replace />;
  }

  // Admin rolü yoksa anket listesine yönlendir
  const isAdmin = user.roles?.some((r: string) => r.toLowerCase() === 'admin');
  if (!isAdmin) {
    return <Navigate to="/surveys" replace />;
  }

  return <Outlet />;
};

export default AdminRoute;