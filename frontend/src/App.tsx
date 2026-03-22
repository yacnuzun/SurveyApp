import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import SurveyListPage from './pages/SurveyListPage';
import SurveyDetailPage from './pages/SurveyDetailPage';
import AdminSurveyCreate from './pages/AdminSurveyCreate';
import AdminRoute from './components/AdminRoute';

function App() {
  return (
    <BrowserRouter>
      <Routes>
  {/* Login */}
  <Route path="/login" element={<LoginPage />} />

  {/* Public Routes */}
  <Route path="/surveys" element={<SurveyListPage />} />
  <Route path="/survey-detail/:id" element={<SurveyDetailPage />} />

  {/* Admin Protected Routes */}
  <Route path="/admin" element={<AdminRoute />}>
    <Route path="create-survey" element={<AdminSurveyCreate />} />
  </Route>

  {/* Yönlendirmeler */}
  <Route path="/" element={<Navigate to="/surveys" replace />} />
  <Route path="*" element={<Navigate to="/login" replace />} />
</Routes>
    </BrowserRouter>
  );
}

export default App;