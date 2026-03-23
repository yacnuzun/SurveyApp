import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import type { AppDispatch } from '../../store/appStore';
import { createComplexSurvey } from '../../store/surveySlice';
import type { SurveyCreateDto, QuestionCreateDto } from '../../types/Survey';
import { useNavigate } from 'react-router-dom';
import './AdminSurveyCreate.css';

const AdminSurveyCreate = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const [survey, setSurvey] = useState<SurveyCreateDto>({
    title: '',
    description: '',
    startDate: new Date().toISOString().split('T')[0],
    endDate: '',
    questions: []
  });

  const addQuestion = () => {
    const newQuestion: QuestionCreateDto = {
      text: '',
      type: 0,
      order: survey.questions.length + 1,
      options: []
    };
    setSurvey({ ...survey, questions: [...survey.questions, newQuestion] });
  };

  const addOption = (qIndex: number) => {
    const updatedQuestions = [...survey.questions];
    updatedQuestions[qIndex].options.push({
      text: '',
      order: updatedQuestions[qIndex].options.length + 1
    });
    setSurvey({ ...survey, questions: updatedQuestions });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const result = await dispatch(createComplexSurvey(survey));
    if (createComplexSurvey.fulfilled.match(result)) {
      alert("Anket Başarıyla Oluşturuldu!");
      navigate('/');
    }
  };

  return (
    <div className="admin-create-container" style={{ padding: '20px' }}>
      <h2>Yeni Kompleks Anket Oluştur</h2>
      <form onSubmit={handleSubmit}>
        <input 
          type="text" placeholder="Anket Başlığı" 
          onChange={e => setSurvey({...survey, title: e.target.value})}
          className="form-control" required 
        />
        <textarea 
          placeholder="Açıklama" 
          onChange={e => setSurvey({...survey, description: e.target.value})}
          className="form-control"
        />
        
        <div style={{ margin: '20px 0' }}>
          <label>Bitiş Tarihi: </label>
          <input type="date" onChange={e => setSurvey({...survey, endDate: e.target.value})} required />
        </div>

        <hr />
        <h3>Sorular</h3>
        {survey.questions.map((q, qIndex) => (
          <div key={qIndex} className="question-setup" style={{ border: '1px solid #ccc', padding: '10px', marginBottom: '10px' }}>
            <input 
              type="text" placeholder={`${qIndex + 1}. Soru Metni`}
              value={q.text}
              onChange={e => {
                const qs = [...survey.questions];
                qs[qIndex].text = e.target.value;
                setSurvey({...survey, questions: qs});
              }}
            />
            <select 
              value={q.type}
              onChange={e => {
                const qs = [...survey.questions];
                qs[qIndex].type = parseInt(e.target.value);
                setSurvey({...survey, questions: qs});
              }}
            >
              <option value={0}>Tekli Seçim (Radio)</option>
              <option value={1}>Çoklu Seçim (Checkbox)</option>
              <option value={2}>Metin (Yazı)</option>
            </select>

            {q.type !== 2 && (
              <div className="options-setup" style={{ marginLeft: '20px' }}>
                {q.options.map((opt, oIndex) => (
                  <input 
                    key={oIndex} type="text" placeholder="Seçenek metni"
                    onChange={e => {
                      const qs = [...survey.questions];
                      qs[qIndex].options[oIndex].text = e.target.value;
                      setSurvey({...survey, questions: qs});
                    }}
                  />
                ))}
                <button type="button" onClick={() => addOption(qIndex)}>+ Seçenek Ekle</button>
              </div>
            )}
          </div>
        ))}

        <button type="button" onClick={addQuestion} style={{ backgroundColor: '#007bff', color: '#fff' }}>
          + Yeni Soru Ekle
        </button>
        <br /><br />
        <button type="submit" className="btn-success" style={{ width: '100%', padding: '15px' }}>
          ANKETİ YAYINLA
        </button>
      </form>
    </div>
  );
};

export default AdminSurveyCreate;