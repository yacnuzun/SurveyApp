import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchActiveSurveys } from '../store/surveySlice';
import type { AppDispatch, RootState } from '../store/appStore';
import type { Survey } from '../types/Survey';
//import './SurveyListPage.css';

const SurveyListPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { surveys, isLoading, error } = useSelector((state: RootState) => state.survey);

  useEffect(() => {
    dispatch(fetchActiveSurveys());
  }, [dispatch]);

  if (isLoading) return <div className="loader">Anketler yükleniyor...</div>;
  if (error) return <div className="error-msg">{error}</div>;

  return (
    <div className="survey-list-container">
      <header className="survey-header">
        <h1>Aktif Anketler</h1>
        <p>Görüşleriniz bizim için değerli! Bir anket seçin ve hemen doldurun.</p>
      </header>

      <div className="survey-grid">
        {surveys.length > 0 ? (
          surveys.map((survey: Survey) => (
            <div key={survey.id} className="survey-card">
              <div className="card-content">
                <h3>{survey.title}</h3>
                <p>{survey.description}</p>
              </div>
              <button className="join-button" onClick={() => console.log("Ankete git:", survey.id)}>
                Katıl
              </button>
            </div>
          ))
        ) : (
          <p className="no-data">Şu an aktif bir anket bulunmuyor.</p>
        )}
      </div>
    </div>
  );
};

export default SurveyListPage;