// src/App.tsx — GÜNCELLENDİ

import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage/LoginPage';
import SurveyListPage from './pages/SurveyListPage/SurveyListPage';
import SurveyDetailPage from './pages/SurveyDetailPage/SurveyDetailPage';
import ReportPage from './pages/ReportPage/ReportPage';
import AnswerTemplatePage from './pages/AnswerTemplatePage/AnswerTemplatePage';
import QuestionPage from './pages/QuestionPage/QuestionPage';
import SurveyManagementPage from './pages/SurveyManagementPage/SurveyManagementPage';
import Navbar from './components/Navbar/Navbar';
import ProtectedRoute from './components/ProtectedRoute';
import AdminRoute from './components/AdminRoute';

/**
 * Route yapısı:
 *
 * /login                          → Herkese açık
 * <ProtectedRoute>                → Token yoksa → /login
 *   /surveys                      → Kullanıcı: atanan anketler
 *   /survey-detail/:id            → Anket doldurma
 *   <AdminRoute>                  → Admin değilse → /surveys
 *     /admin/templates            → Cevap şablonları CRUD
 *     /admin/questions            → Soru CRUD
 *     /admin/surveys              → Anket yönetimi CRUD
 *     /admin/surveys/:id/report   → Raporlama
 */
function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route element={<ProtectedRoute />}>
          {/* Kullanıcı rotaları */}
          <Route path="/surveys" element={<SurveyListPage />} />
          <Route path="/survey-detail/:id" element={<SurveyDetailPage />} />

          {/* Admin rotaları */}
          <Route element={<AdminRoute />}>
            <Route path="/admin/templates" element={<AnswerTemplatePage />} />
            <Route path="/admin/questions" element={<QuestionPage />} />
            <Route path="/admin/surveys" element={<SurveyManagementPage />} />
            <Route path="/admin/surveys/:id/report" element={<ReportPage />} />
          </Route>
        </Route>

        <Route path="/" element={<Navigate to="/surveys" replace />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;