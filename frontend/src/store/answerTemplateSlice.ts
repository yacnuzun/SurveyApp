// src/store/answerTemplateSlice.ts

import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../api/axiosConfig';
import type { AnswerTemplate, AnswerTemplateCreateDto, AnswerTemplateUpdateDto } from '../types/Answertemplate';

export const fetchTemplates = createAsyncThunk('answerTemplate/fetchAll', async (_, { rejectWithValue }) => {
  try {
    const res = await api.get<AnswerTemplate[]>('AnswerTemplates');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Şablonlar yüklenemedi');
  }
});

export const createTemplate = createAsyncThunk('answerTemplate/create', async (dto: AnswerTemplateCreateDto, { rejectWithValue }) => {
  try {
    const res = await api.post('AnswerTemplates', dto);
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Şablon oluşturulamadı');
  }
});

export const updateTemplate = createAsyncThunk('answerTemplate/update', async (dto: AnswerTemplateUpdateDto, { rejectWithValue }) => {
  try {
    const res = await api.put(`AnswerTemplates/${dto.id}`, dto);
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Şablon güncellenemedi');
  }
});

export const deleteTemplate = createAsyncThunk('answerTemplate/delete', async (id: number, { rejectWithValue }) => {
  try {
    await api.delete(`AnswerTemplates/${id}`);
    return id;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Şablon silinemedi');
  }
});

const answerTemplateSlice = createSlice({
  name: 'answerTemplate',
  initialState: {
    templates: [] as AnswerTemplate[],
    isLoading: false,
    error: null as string | null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchTemplates.pending, (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchTemplates.fulfilled, (state, action) => { state.isLoading = false; state.templates = action.payload; })
      .addCase(fetchTemplates.rejected, (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(createTemplate.fulfilled, (state, action) => { state.templates.push(action.payload); })
      .addCase(updateTemplate.fulfilled, (state, action) => {
        const idx = state.templates.findIndex(t => t.id === action.payload.id);
        if (idx !== -1) state.templates[idx] = action.payload;
      })
      .addCase(deleteTemplate.fulfilled, (state, action) => {
        state.templates = state.templates.filter(t => t.id !== action.payload);
      });
  },
});

export default answerTemplateSlice.reducer;