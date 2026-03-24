// src/pages/AdminSurveyCreate/AdminSurveyCreate.tsx
// NOT: Bu sayfa artık legacy — yeni akış SurveyManagementPage üzerinden.
// Ama hata vermemesi için yeni DTO yapısına uyarlandı.

import { useState } from 'react';
import { useDispatch } from 'react-redux';
import type { AppDispatch } from '../../store/appStore';
import { createSurvey } from '../../store/surveySlice';
import type { SurveyCreateDto } from '../../types/Survey';
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
    questionIds: [],
    assignedUserIds: [],
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const result = await dispatch(createSurvey(survey));
    if (createSurvey.fulfilled.match(result)) {
      alert('Anket başarıyla oluşturuldu!');
      navigate('/admin/surveys');
    }
  };

  return (
    <div className="admin-create-container" style={{ padding: '20px' }}>
      <h2>Yeni Anket Oluştur</h2>
      <p style={{ color: '#636e72', marginBottom: '20px' }}>
        Soru eklemek için önce{' '}
        <a href="/admin/templates">Cevap Şablonları</a> ve{' '}
        <a href="/admin/questions">Sorular</a> sayfalarını kullanın,
        ardından <a href="/admin/surveys">Anket Yönetimi</a>'nden anketi oluşturun.
      </p>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Anket Başlığı"
          onChange={e => setSurvey({ ...survey, title: e.target.value })}
          className="form-control"
          required
        />
        <textarea
          placeholder="Açıklama"
          onChange={e => setSurvey({ ...survey, description: e.target.value })}
          className="form-control"
        />
        <div style={{ margin: '20px 0' }}>
          <label>Bitiş Tarihi: </label>
          <input
            type="date"
            onChange={e => setSurvey({ ...survey, endDate: e.target.value })}
            required
          />
        </div>
        <button type="submit" style={{ width: '100%', padding: '15px', background: '#4834d4', color: '#fff', border: 'none', borderRadius: '8px', fontSize: '16px', cursor: 'pointer' }}>
          ANKETİ YAYINLA
        </button>
      </form>
    </div>
  );
};

export default AdminSurveyCreate;