import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { login } from '../store/authSlice'; 
import { type AppDispatch } from '../store/appStore';
//import './LoginPage.css'; 

const LoginPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  
  // State tanımlamaları
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  // Form gönderim fonksiyonu
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // login aksiyonunu tetikle
    const resultAction = await dispatch(login({ email, password }));
    
    // Eğer login başarılıysa yönlendir
    if (login.fulfilled.match(resultAction)) {
      navigate('/surveys');
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
            <p>Admin: admin@example.com / admin123</p>
            <p>User: user@example.com / user123</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;