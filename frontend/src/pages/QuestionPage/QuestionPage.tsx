// src/pages/QuestionPage/QuestionPage.tsx

import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchQuestions, createQuestion, updateQuestion, deleteQuestion } from '../../store/questionSlice';
import { fetchTemplates } from '../../store/answerTemplateSlice';
import type { AppDispatch, RootState } from '../../store/appStore';
import type { Question, QuestionCreateDto } from '../../types/Survey';
import { QuestionType } from '../../types/Survey';
import '../Shared/crud.css';

const typeLabels: Record<QuestionType, string> = {
  [QuestionType.Single]: 'Tekli Seçim',
  [QuestionType.Multi]: 'Çoklu Seçim',
  [QuestionType.Text]: 'Metin',
};

const emptyForm = (): QuestionCreateDto => ({
  text: '',
  type: QuestionType.Single,
  answerTemplateId: undefined,
});

const QuestionPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { questions, isLoading, error } = useSelector((state: RootState) => state.question);
  const { templates } = useSelector((state: RootState) => state.answerTemplate);

  const [form, setForm] = useState<QuestionCreateDto>(emptyForm());
  const [editingId, setEditingId] = useState<number | null>(null);
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    dispatch(fetchQuestions());
    dispatch(fetchTemplates());
  }, [dispatch]);

  const needsTemplate = form.type !== QuestionType.Text;

  const handleEdit = (q: Question) => {
    setEditingId(q.id);
    setForm({ text: q.text, type: q.type, answerTemplateId: q.answerTemplateId });
    setShowForm(true);
  };

  const handleCancel = () => {
    setEditingId(null);
    setForm(emptyForm());
    setShowForm(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const payload = { ...form, answerTemplateId: needsTemplate ? form.answerTemplateId : undefined };
    if (editingId !== null) {
      await dispatch(updateQuestion({ ...payload, id: editingId }));
    } else {
      await dispatch(createQuestion(payload));
    }
    handleCancel();
  };

  const handleDelete = (id: number) => {
    if (window.confirm('Bu soruyu silmek istediğinize emin misiniz?')) {
      dispatch(deleteQuestion(id));
    }
  };

  return (
    <div className="crud-page">
      <div className="crud-header">
        <div>
          <h1>Sorular</h1>
          <p>Anketlerde kullanılacak soruları yönetin</p>
        </div>
        <button className="btn-primary" onClick={() => setShowForm(true)}>+ Yeni Soru</button>
      </div>

      {showForm && (
        <div className="form-card">
          <h3>{editingId ? 'Soruyu Düzenle' : 'Yeni Soru'}</h3>
          <form onSubmit={handleSubmit}>
            <div className="field">
              <label>Soru Metni</label>
              <input
                type="text"
                placeholder="Soruyu yazın..."
                value={form.text}
                onChange={e => setForm({ ...form, text: e.target.value })}
                required
              />
            </div>
            <div className="field">
              <label>Soru Tipi</label>
              <select
                value={form.type}
                onChange={e => setForm({ ...form, type: parseInt(e.target.value) as QuestionType, answerTemplateId: undefined })}
              >
                <option value={QuestionType.Single}>Tekli Seçim (Radio)</option>
                <option value={QuestionType.Multi}>Çoklu Seçim (Checkbox)</option>
                <option value={QuestionType.Text}>Metin Girişi</option>
              </select>
            </div>

            {needsTemplate && (
              <div className="field">
                <label>Cevap Şablonu</label>
                {templates.length === 0 ? (
                  <p className="hint-warn">Önce Cevap Şablonları sayfasından şablon oluşturun.</p>
                ) : (
                  <select
                    value={form.answerTemplateId ?? ''}
                    onChange={e => setForm({ ...form, answerTemplateId: parseInt(e.target.value) })}
                    required
                  >
                    <option value="">Şablon seçin...</option>
                    {templates.map(t => (
                      <option key={t.id} value={t.id}>{t.name} ({t.optionCount} seçenek)</option>
                    ))}
                  </select>
                )}
                {form.answerTemplateId && (
                  <div className="template-preview">
                    {templates.find(t => t.id === form.answerTemplateId)?.options.map(o => (
                      <span key={o.id} className="chip">{o.text}</span>
                    ))}
                  </div>
                )}
              </div>
            )}

            <div className="form-actions">
              <button type="button" className="btn-ghost" onClick={handleCancel}>İptal</button>
              <button type="submit" className="btn-primary">{editingId ? 'Güncelle' : 'Kaydet'}</button>
            </div>
          </form>
        </div>
      )}

      {isLoading && <div className="loader">Yükleniyor...</div>}
      {error && <div className="error-msg">{error}</div>}

      <div className="questions-table">
        <table>
          <thead>
            <tr>
              <th>#</th>
              <th>Soru</th>
              <th>Tip</th>
              <th>Şablon</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {questions.map((q, i) => (
              <tr key={q.id}>
                <td>{i + 1}</td>
                <td>{q.text}</td>
                <td><span className={`type-badge type-${q.type}`}>{typeLabels[q.type]}</span></td>
                <td>{q.answerTemplateName ?? <span className="muted">—</span>}</td>
                <td>
                  <div className="row-actions">
                    <button className="btn-edit-sm" onClick={() => handleEdit(q)}>Düzenle</button>
                    <button className="btn-delete-sm" onClick={() => handleDelete(q.id)}>Sil</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {!isLoading && questions.length === 0 && <p className="no-data">Henüz soru eklenmemiş.</p>}
      </div>
    </div>
  );
};

export default QuestionPage;