// src/pages/AnswerTemplatePage/AnswerTemplatePage.tsx

import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchTemplates, createTemplate, updateTemplate, deleteTemplate } from '../../store/answerTemplateSlice';
import type { AppDispatch, RootState } from '../../store/appStore';
import type { AnswerTemplate, AnswerTemplateCreateDto } from '../../types/AnswerTemplate';
import '../Shared/crud.css';


const emptyForm = (): AnswerTemplateCreateDto => ({
  name: '',
  options: [{ text: '', order: 1 }, { text: '', order: 2 }],
});

const AnswerTemplatePage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { templates, isLoading, error } = useSelector((state: RootState) => state.answerTemplate);

  const [form, setForm] = useState<AnswerTemplateCreateDto>(emptyForm());
  const [editingId, setEditingId] = useState<number | null>(null);
  const [showForm, setShowForm] = useState(false);

  useEffect(() => { dispatch(fetchTemplates()); }, [dispatch]);

  const handleOptionChange = (index: number, value: string) => {
    const opts = [...form.options];
    opts[index] = { ...opts[index], text: value };
    setForm({ ...form, options: opts });
  };

  const addOption = () => {
    if (form.options.length >= 4) return;
    setForm({ ...form, options: [...form.options, { text: '', order: form.options.length + 1 }] });
  };

  const removeOption = (index: number) => {
    if (form.options.length <= 2) return;
    setForm({ ...form, options: form.options.filter((_, i) => i !== index).map((o, i) => ({ ...o, order: i + 1 })) });
  };

  const handleEdit = (t: AnswerTemplate) => {
    setEditingId(t.id);
    setForm({ name: t.name, options: t.options.map(o => ({ text: o.text, order: o.order })) });
    setShowForm(true);
  };

  const handleCancel = () => {
    setEditingId(null);
    setForm(emptyForm());
    setShowForm(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (editingId !== null) {
      await dispatch(updateTemplate({ ...form, id: editingId }));
    } else {
      await dispatch(createTemplate(form));
    }
    handleCancel();
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Bu şablonu silmek istediğinize emin misiniz?')) {
      dispatch(deleteTemplate(id));
    }
  };

  return (
    <div className="crud-page">
      <div className="crud-header">
        <div>
          <h1>Cevap Şablonları</h1>
          <p>Sorularda kullanılacak seçenek gruplarını yönetin</p>
        </div>
        <button className="btn-primary" onClick={() => setShowForm(true)}>+ Yeni Şablon</button>
      </div>

      {showForm && (
        <div className="form-card">
          <h3>{editingId ? 'Şablonu Düzenle' : 'Yeni Şablon'}</h3>
          <form onSubmit={handleSubmit}>
            <div className="field">
              <label>Şablon Adı</label>
              <input
                type="text"
                placeholder="örn: Evet/Hayır, Memnuniyet Ölçeği"
                value={form.name}
                onChange={e => setForm({ ...form, name: e.target.value })}
                required
              />
            </div>
            <div className="field">
              <label>Seçenekler <span className="hint">(2–4 arası)</span></label>
              {form.options.map((opt, i) => (
                <div key={i} className="option-row">
                  <span className="option-num">{i + 1}</span>
                  <input
                    type="text"
                    placeholder={`Seçenek ${i + 1}`}
                    value={opt.text}
                    onChange={e => handleOptionChange(i, e.target.value)}
                    required
                  />
                  {form.options.length > 2 && (
                    <button type="button" className="btn-icon-danger" onClick={() => removeOption(i)}>✕</button>
                  )}
                </div>
              ))}
              {form.options.length < 4 && (
                <button type="button" className="btn-add-option" onClick={addOption}>+ Seçenek Ekle</button>
              )}
            </div>
            <div className="form-actions">
              <button type="button" className="btn-ghost" onClick={handleCancel}>İptal</button>
              <button type="submit" className="btn-primary">{editingId ? 'Güncelle' : 'Kaydet'}</button>
            </div>
          </form>
        </div>
      )}

      {isLoading && <div className="loader">Yükleniyor...</div>}
      {error && <div className="error-msg">{error}</div>}

      <div className="crud-grid">
        {templates.map(t => (
          <div key={t.id} className="crud-card">
            <div className="crud-card-header">
              <span className="crud-card-title">{t.name}</span>
              <span className="badge">{t.optionCount ?? t.options?.length ?? 0} seçenek</span>
            </div>
            <div className="option-chips">
              {t.options?.map(o => <span key={o.id} className="chip">{o.text}</span>)}
            </div>
            <div className="crud-card-actions">
              <button className="btn-edit" onClick={() => handleEdit(t)}>Düzenle</button>
              <button className="btn-delete" onClick={() => handleDelete(t.id)}>Sil</button>
            </div>
          </div>
        ))}
        {!isLoading && templates.length === 0 && (
          <p className="no-data">Henüz şablon oluşturulmamış.</p>
        )}
      </div>
    </div>
  );
};

export default AnswerTemplatePage;