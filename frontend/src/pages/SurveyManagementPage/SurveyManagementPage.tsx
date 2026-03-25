// src/pages/SurveyManagementPage/SurveyManagementPage.tsx

import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchActiveSurveys, createSurvey, updateSurvey, deleteSurvey, toggleSurveyActive } from '../../store/surveySlice';
import { fetchQuestions } from '../../store/questionSlice';
import type { AppDispatch, RootState } from '../../store/appStore';
import type { Survey, SurveyCreateDto } from '../../types/Survey';
import { useNavigate } from 'react-router-dom';
//import './SurveyManagementPage.css';

const emptyForm = (): SurveyCreateDto => ({
  title: '',
  description: '',
  startDate: new Date().toISOString().split('T')[0],
  endDate: '',
  questionIds: [],
  assignedUserIds: [],
});

const SurveyManagementPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { surveys, isLoading, error } = useSelector((state: RootState) => state.survey);
  const { questions } = useSelector((state: RootState) => state.question);

  const [form, setForm] = useState<SurveyCreateDto>(emptyForm());
  const [editingId, setEditingId] = useState<number | null>(null);
  const [showForm, setShowForm] = useState(false);
  // Kullanıcı ID girişi — gerçek projede user listesi API'den çekilir
  const [userIdInput, setUserIdInput] = useState('');

  useEffect(() => {
    dispatch(fetchActiveSurveys()).then((res: any) => {
    console.log('surveys response:', res.payload);
  });
    dispatch(fetchQuestions());
  }, [dispatch]);

  const toggleQuestion = (id: number) => {
    setForm(f => ({
      ...f,
      questionIds: f.questionIds.includes(id)
        ? f.questionIds.filter(q => q !== id)
        : [...f.questionIds, id],
    }));
  };

  const addUserId = () => {
    const id = parseInt(userIdInput);
    if (!isNaN(id) && !form.assignedUserIds.includes(id)) {
      setForm(f => ({ ...f, assignedUserIds: [...f.assignedUserIds, id] }));
    }
    setUserIdInput('');
  };

  const removeUserId = (id: number) => {
    setForm(f => ({ ...f, assignedUserIds: f.assignedUserIds.filter(u => u !== id) }));
  };

  const handleEdit = (s: Survey) => {
    setEditingId(s.id);
    setForm({
      title: s.title,
      description: s.description,
      startDate: s.startDate.split('T')[0],
      endDate: s.endDate.split('T')[0],
      questionIds: s.questions.map(q => q.id),
      assignedUserIds: s.assignedUserIds,
    });
    setShowForm(true);
  };

  const handleCancel = () => {
    setEditingId(null);
    setForm(emptyForm());
    setUserIdInput('');
    setShowForm(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();
  if (editingId !== null) {
    const survey = surveys.find(s => s.id === editingId);
    await dispatch(updateSurvey({ ...form, id: editingId, isActive: survey?.isActive ?? true }));
  } else {
    await dispatch(createSurvey(form));
  }
  handleCancel();
  dispatch(fetchActiveSurveys()); // ← ekle
};

const handleDelete = async (id: number) => {
  if (window.confirm('Bu anketi silmek istediğinize emin misiniz?')) {
    await dispatch(deleteSurvey(id));
    dispatch(fetchActiveSurveys()); // ← ekle
  }
};

  return (
    <div className="crud-page">
      <div className="crud-header">
        <div>
          <h1>Anket Yönetimi</h1>
          <p>Anketleri oluşturun, düzenleyin ve kullanıcılara atayın</p>
        </div>
        <button className="btn-primary" onClick={() => setShowForm(true)}>+ Yeni Anket</button>
      </div>

      {showForm && (
        <div className="form-card form-card-wide">
          <h3>{editingId ? 'Anketi Düzenle' : 'Yeni Anket'}</h3>
          <form onSubmit={handleSubmit}>
            <div className="field-row">
              <div className="field">
                <label>Başlık</label>
                <input type="text" value={form.title} onChange={e => setForm({ ...form, title: e.target.value })} required />
              </div>
            </div>
            <div className="field">
              <label>Açıklama</label>
              <textarea value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} rows={2} />
            </div>
            <div className="field-row">
              <div className="field">
                <label>Başlangıç Tarihi</label>
                <input type="date" value={form.startDate} onChange={e => setForm({ ...form, startDate: e.target.value })} required />
              </div>
              <div className="field">
                <label>Bitiş Tarihi</label>
                <input type="date" value={form.endDate} onChange={e => setForm({ ...form, endDate: e.target.value })} required />
              </div>
            </div>

            {/* Soru seçimi */}
            <div className="field">
              <label>Sorular <span className="hint">({form.questionIds.length} seçildi)</span></label>
              <div className="question-picker">
                {questions.length === 0
                  ? <p className="hint-warn">Önce Sorular sayfasından soru ekleyin.</p>
                  : questions.map(q => (
                    <label key={q.id} className={`question-pick-item ${form.questionIds.includes(q.id) ? 'selected' : ''}`}>
                      <input
                        type="checkbox"
                        checked={form.questionIds.includes(q.id)}
                        onChange={() => toggleQuestion(q.id)}
                      />
                      <span className="q-text">{q.text}</span>
                      <span className="q-template">{q.answerTemplateName ?? 'Metin'}</span>
                    </label>
                  ))
                }
              </div>
            </div>

            {/* Kullanıcı atama */}
            <div className="field">
              <label>Atanan Kullanıcılar</label>
              <div className="user-assign-row">
                <input
                  type="number"
                  placeholder="Kullanıcı ID girin"
                  value={userIdInput}
                  onChange={e => setUserIdInput(e.target.value)}
                  onKeyDown={e => e.key === 'Enter' && (e.preventDefault(), addUserId())}
                />
                <button type="button" className="btn-ghost" onClick={addUserId}>Ekle</button>
              </div>
              <div className="user-chips">
                {form.assignedUserIds.map(uid => (
                  <span key={uid} className="chip chip-user">
                    #{uid} <button type="button" onClick={() => removeUserId(uid)}>✕</button>
                  </span>
                ))}
              </div>
            </div>

            <div className="form-actions">
              <button type="button" className="btn-ghost" onClick={handleCancel}>İptal</button>
              <button type="submit" className="btn-primary">{editingId ? 'Güncelle' : 'Yayınla'}</button>
            </div>
          </form>
        </div>
      )}

      {isLoading && <div className="loader">Yükleniyor...</div>}
      {error && <div className="error-msg">{error}</div>}

      <div className="survey-mgmt-list">
        {surveys.map(s => (
          <div key={s.id} className={`survey-mgmt-card ${!s.isActive ? 'inactive' : ''}`}>
            <div className="survey-mgmt-top">
              <div>
                <h3>{s.title}</h3>
                <p>{s.description}</p>
                <div className="survey-meta">
                  <span>📅 {s.startDate?.split('T')[0] ?? '-'} — {s.endDate?.split('T')[0] ?? '-'}</span>
                  <span>❓ {s.questions?.length ?? 0} soru</span>
                  <span>👥 {s.assignedUserIds?.length ?? 0} kullanıcı</span>
                </div>
              </div>
              <span className={`status-badge ${s.isActive ? 'active' : 'passive'}`}>
                {s.isActive ? 'Aktif' : 'Pasif'}
              </span>
            </div>
            <div className="survey-mgmt-actions">
              <button className="btn-edit" onClick={() => handleEdit(s)}>Düzenle</button>
              <button
  className={s.isActive ? 'btn-ghost' : 'btn-primary-sm'}
  onClick={async () => {
    if (!s.id) return;
    await dispatch(toggleSurveyActive(s.id));
    dispatch(fetchActiveSurveys()); // ← liste yenile
  }}
>
  {s.isActive ? 'Pasife Al' : 'Aktifleştir'}
</button>
              <button className="btn-ghost" onClick={() => navigate(`/admin/surveys/${s.id}/report`)}>Rapor</button>
              <button className="btn-delete" onClick={() => handleDelete(s.id)}>Sil</button>
            </div>
          </div>
        ))}
        {!isLoading && surveys.length === 0 && <p className="no-data">Henüz anket oluşturulmamış.</p>}
      </div>
    </div>
  );
};

export default SurveyManagementPage;