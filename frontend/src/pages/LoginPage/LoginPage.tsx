import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { login } from '../../store/authSlice'; 
import { type AppDispatch } from '../../store/appStore';
import './LoginPage.css'; 

const LoginPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  
  // State tanımlamaları
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

 const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();
  const resultAction = await dispatch(login({ email, password }));

  if (login.fulfilled.match(resultAction)) {
    const roles = resultAction.payload.role as string[];
    const isAdmin = roles.some((r: string) => r.toLowerCase() === 'admin');
    navigate(isAdmin ? '/admin/surveys' : '/surveys');
  } else if (login.rejected.match(resultAction)) {
    alert(resultAction.payload as string); // ← hata mesajını göster
  }
};

  return (
    <div className="login-page">
      <div className="login-container">
        <div className="login-box">
          <h1 className="login-title">📋 Anket Yönetim Sistemi</h1>
          <p className="login-subtitle">Giriş Yap</p>

          <form onSubmit={handleSubmit} className="login-form">
            <div className="form-group">
              <label htmlFor="email">E-posta</label>
              <input
                type="email"
                id="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="ornek@email.com"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="password">Şifre</label>
              <input
                type="password"
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Şifrenizi girin"
                required
              />
            </div>

            <button type="submit" className="login-button">
              Giriş Yap
            </button>
          </form>

          <div className="login-footer">
            <p>Admin: admin@surveyapp.com / Admin123!</p>
            <p>User: user1@surveyapp.com / User123!</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;