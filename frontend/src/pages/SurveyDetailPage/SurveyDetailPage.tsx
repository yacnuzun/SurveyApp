import { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; // navigate eklendi
import { useDispatch, useSelector } from 'react-redux';
import { fetchSurveyById, setAnswer, submitSurveyAnswers } from '../../store/participationSlice'; // submit eklendi
import type { RootState, AppDispatch } from '../../store/appStore';
import type { Question } from '../../types/Survey'; 
import { QuestionType } from '../../types/Survey';
import './SurveyDetailPage.css';

const SurveyDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate(); // navigate burada tanımlanmalı

  const { currentSurvey, answers, isLoading } = useSelector((state: RootState) => state.participation);

  useEffect(() => {
    if (id) dispatch(fetchSurveyById(id));
  }, [id, dispatch]);

  const handleInputChange = (questionId: number, value: any) => {
    dispatch(setAnswer({ questionId, value }));
  };

  const handleFinish = () => {
    // currentSurvey null kontrolü (TypeScript hatasını çözer)
    if (!currentSurvey) return;

    const formattedAnswers = Object.entries(answers).map(([qId, val]) => ({
      questionId: parseInt(qId),
      optionId: typeof val === 'number' ? val : null,
      textAnswer: typeof val === 'string' ? val : null
    }));

    const payload = {
      surveyId: currentSurvey.id,
      answers: formattedAnswers
    };

    // 'res: any' TypeScript uyarısını çözer
    dispatch(submitSurveyAnswers(payload)).then((res: any) => {
      if (res.meta.requestStatus === 'fulfilled') {
        alert("Anket başarıyla gönderildi!");
        navigate('/'); // Listeye yönlendir
      }
    });
  };

  if (isLoading || !currentSurvey) return <div>Yükleniyor...</div>;

  return (
    <div className="survey-detail-container">
      <h1>{currentSurvey.title}</h1>
      <p>{currentSurvey.description}</p>
      <hr />

      {currentSurvey.questions?.map((q: Question) => (
        <div key={q.id} className="question-block" style={{ marginBottom: '20px' }}>
          <h4>{q.text}</h4> {/* DTO'da 'text' demiştik */}
          
          {q.type === QuestionType.Text && (
            <input 
              type="text" 
              className="form-control"
              onChange={(e) => handleInputChange(q.id, e.target.value)} 
            />
          )}

          {q.type === QuestionType.Multi && q.options?.map((opt: any) => (
            <label key={opt.id} style={{ display: 'block' }}>
              <input 
                type="radio" 
                name={`q-${q.id}`} 
                onChange={() => handleInputChange(q.id, opt.id)} 
              />
              {opt.text} {/* DTO'da 'text' demiştik */}
            </label>
          ))}
        </div>
      ))}

      <button 
        className="btn-success" 
        onClick={handleFinish} // onClick buraya bağlandı
        style={{ marginTop: '20px', padding: '10px 20px', cursor: 'pointer' }}
      >
        Anketi Bitir
      </button>
    </div>
  );
};

export default SurveyDetailPage;