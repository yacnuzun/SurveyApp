import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import SurveyListPage from './pages/SurveyListPage';
// import SurveyListPage from './pages/SurveyListPage'; // Hazır olduğunda açacağız

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Ana dizine gelindiğinde direkt Login'e yönlendir */}
        <Route path="/" element={<Navigate to="/login" />} />
        
        {/* Login Sayfası */}
        <Route path="/login" element={<LoginPage />} />

        {/* Anket Listesi Sayfası (Henüz içeriği boş olsa bile route'u tanımlayalım) */}
        <Route path="/surveys" element={<SurveyListPage />} />

        {/* Tanımsız yollar için Login'e geri gönder */}
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;