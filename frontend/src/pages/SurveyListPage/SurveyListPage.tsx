// src/pages/SurveyListPage/SurveyListPage.tsx

import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchMySurveys, fetchActiveSurveys } from '../../store/surveySlice';
import type { AppDispatch, RootState } from '../../store/appStore';
import type { Survey } from '../../types/Survey';
import { useNavigate } from 'react-router-dom';
import './SurveyListPage.css';

const SurveyListPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { surveys, isLoading, error } = useSelector((state: RootState) => state.survey);
  const { user } = useSelector((state: RootState) => state.auth);

  const isAdmin = user?.roles?.some((r: string) => r.toLowerCase() === 'admin');

  useEffect(() => {
    if (isAdmin) {
      dispatch(fetchActiveSurveys());
    } else {
      dispatch(fetchMySurveys());
    }
  }, [dispatch, isAdmin]);

  if (isLoading) return <div className="loader">Anketler yükleniyor...</div>;
  if (error) return <div className="error-msg">{error}</div>;

  return (
    <div className="survey-list-container">
      <header className="survey-header">
        <h1>{isAdmin ? 'Aktif Anketler' : 'Anketlerim'}</h1>
        <p>
          {isAdmin
            ? 'Sistemdeki tüm aktif anketler.'
            : 'Size atanmış aktif anketler. Görüşleriniz bizim için değerli!'}
        </p>
      </header>

      <div className="survey-grid">
        {surveys.length > 0 ? (
          surveys.map((survey: Survey) => (
            <div key={survey.id} className="survey-card">
              <div className="card-content">
                <h3>{survey.title}</h3>
                <p>{survey.description}</p>
              </div>
              <button
                className="join-button"
                onClick={() => navigate(`/survey-detail/${survey.id}`)}
              >
                Katıl
              </button>
              {isAdmin && (
                <button
                  className="report-btn"
                  onClick={() => navigate(`/admin/surveys/${survey.id}/report`)}
                >
                  📊 İstatistikleri Gör
                </button>
              )}
            </div>
          ))
        ) : (
          <p className="no-data">
            {isAdmin ? 'Aktif anket bulunmuyor.' : 'Size atanmış aktif anket bulunmuyor.'}
          </p>
        )}
      </div>
    </div>
  );
};

export default SurveyListPage;