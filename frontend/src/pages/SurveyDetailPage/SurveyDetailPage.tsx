import { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchSurveyById, setAnswer, submitSurveyAnswers } from '../../store/participationSlice';
import type { RootState, AppDispatch } from '../../store/appStore';
import type { Question } from '../../types/Survey';
import { QuestionType } from '../../types/Survey';
import './SurveyDetailPage.css';

const SurveyDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const { currentSurvey, answers, isLoading, isSubmitting } = useSelector(
    (state: RootState) => state.participation
  );

  useEffect(() => {
    if (id) dispatch(fetchSurveyById(id));
  }, [id, dispatch]);

  const handleInputChange = (questionId: number, value: any) => {
    dispatch(setAnswer({ questionId, value }));
  };

  const handleFinish = async () => {
    if (!currentSurvey) return;

    const formattedAnswers = Object.entries(answers).map(([qId, val]) => ({
      questionId: parseInt(qId),
      optionId: typeof val === 'number' ? val : null,
      textAnswer: typeof val === 'string' ? val : null,
    }));

    const payload = {
      surveyId: currentSurvey.id,
      answers: formattedAnswers,
    };

    const result = await dispatch(submitSurveyAnswers(payload));
    if (submitSurveyAnswers.fulfilled.match(result)) {
      alert('Anket başarıyla gönderildi!');
      navigate('/surveys');
    }
  };

  if (isLoading || !currentSurvey) return <div className="survey-loading">Yükleniyor...</div>;

  return (
    <div className="survey-detail-container">
      <h1>{currentSurvey.title}</h1>
      <p className="survey-description">{currentSurvey.description}</p>
      <hr />

      {currentSurvey.questions?.map((q: Question) => (
        <div key={q.id} className="question-block">
          <h4>{q.text}</h4>

          {/* Metin sorusu */}
          {q.type === QuestionType.Text && (
            <input
              type="text"
              className="survey-input-text"
              placeholder="Cevabınızı yazın..."
              onChange={(e) => handleInputChange(q.id, e.target.value)}
            />
          )}

          {/* Tekli seçim (Radio) */}
          {q.type === QuestionType.Single && q.options?.map((opt) => (
            <label key={opt.id} className="option-label">
              <input
                type="radio"
                name={`q-${q.id}`}
                onChange={() => handleInputChange(q.id, opt.id)}
              />
              <span className="option-text">{opt.text}</span>
            </label>
          ))}

          {/* Çoklu seçim (Checkbox) */}
          {q.type === QuestionType.Multi && q.options?.map((opt) => (
            <label key={opt.id} className="option-label">
              <input
                type="checkbox"
                onChange={(e) => {
                  const current: number[] = answers[q.id] || [];
                  const updated = e.target.checked
                    ? [...current, opt.id]
                    : current.filter((id) => id !== opt.id);
                  handleInputChange(q.id, updated);
                }}
              />
              <span className="option-text">{opt.text}</span>
            </label>
          ))}
        </div>
      ))}

      <button
        className="btn-finish"
        onClick={handleFinish}
        disabled={isSubmitting}
      >
        {isSubmitting ? 'Gönderiliyor...' : 'Anketi Bitir'}
      </button>
    </div>
  );
};

export default SurveyDetailPage;