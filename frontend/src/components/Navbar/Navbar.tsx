// src/components/Navbar/Navbar.tsx — GÜNCELLENDİ

import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, useLocation } from 'react-router-dom';
import { logout } from '../../store/authSlice';
import type { AppDispatch, RootState } from '../../store/appStore';
import './Navbar.css';

const adminLinks = [
  { path: '/admin/templates', label: 'Şablonlar' },
  { path: '/admin/questions', label: 'Sorular' },
  { path: '/admin/surveys',   label: 'Anketler' },
];

const Navbar = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const location = useLocation();
  const { user } = useSelector((state: RootState) => state.auth);

  const isAdmin = user?.roles?.some((r: string) => r.toLowerCase() === 'admin');

  if (location.pathname === '/login') return null;

  return (
    <nav className="navbar">
      <div className="navbar-brand" onClick={() => navigate('/surveys')}>
        <span className="brand-icon">📋</span>
        <span className="brand-text">Anket Sistemi</span>
      </div>

      <div className="navbar-links">
        <button
          className={`nav-link ${location.pathname === '/surveys' ? 'active' : ''}`}
          onClick={() => navigate('/surveys')}
        >
          Anketlerim
        </button>

        {isAdmin && adminLinks.map(link => (
          <button
            key={link.path}
            className={`nav-link admin-link ${location.pathname === link.path ? 'active' : ''}`}
            onClick={() => navigate(link.path)}
          >
            {link.label}
          </button>
        ))}
      </div>

      <div className="navbar-user">
        {user && (
          <>
            <div className="user-info">
              <span className="user-avatar">{user.name?.charAt(0).toUpperCase()}</span>
              <div className="user-details">
                <span className="user-name">{user.name}</span>
                <span className={`user-role ${isAdmin ? 'role-admin' : 'role-user'}`}>
                  {isAdmin ? 'Admin' : 'Kullanıcı'}
                </span>
              </div>
            </div>
            <button className="logout-btn" onClick={() => { dispatch(logout()); navigate('/login'); }}>
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" />
                <polyline points="16 17 21 12 16 7" />
                <line x1="21" y1="12" x2="9" y2="12" />
              </svg>
              <span>Çıkış</span>
            </button>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;