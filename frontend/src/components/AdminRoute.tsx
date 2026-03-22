import { Navigate, Outlet } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { type RootState } from '../store/appStore';

const AdminRoute = () => {
    const state = useSelector((state: RootState) => state);
console.log("TÜM REDUX STATE YAPISI:", state);

// Eğer burada 'auth' yerine 'authReducer' veya 'user' yerine 'currentUser' 
// gibi bir isim görüyorsan hata bundandır.
const { user } = state.auth;
  //const { user } = useSelector((state: RootState) => state.auth);
  
  console.log("AdminRoute denetleniyor...");
  console.log("Mevcut User Roles:", user?.roles);

  if (!user || !user.roles) {
    console.warn("Kullanıcı veya roller bulunamadı.");
    return <Navigate to="/login" replace />;
  }

  // C#'taki .Any(x => x == "Admin") mantığı:
  const isAdmin = user?.roles?.some((r: string) => r.toLowerCase() === 'admin');

  if (!isAdmin) {
    console.warn("Kullanıcı admin rollerine sahip değil.");
    return <Navigate to="/surveys" replace />;
  }

  return <Outlet />;
};

export default AdminRoute;