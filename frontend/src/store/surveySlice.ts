// src/store/surveySlice.ts

import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_URLS } from '../api/apiConfig';
import type { Survey, SurveyCreateDto, SurveyUpdateDto } from '../types/Survey';

const mgmt = axios.create({ baseURL: API_URLS.management });
mgmt.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export const fetchActiveSurveys = createAsyncThunk('survey/fetchActive', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmt.get<Survey[]>('Surveys/get-all-active');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anketler yüklenemedi');
  }
});

export const fetchMySurveys = createAsyncThunk('survey/fetchMy', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmt.get<Survey[]>('Surveys/my-surveys');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anketleriniz yüklenemedi');
  }
});

export const createSurvey = createAsyncThunk('Surveys/create-complex', async (dto: SurveyCreateDto, { rejectWithValue }) => {
  try {
    const res = await mgmt.post('Surveys', dto);
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anket oluşturulamadı');
  }
});

export const updateSurvey = createAsyncThunk('survey/update', async (dto: SurveyUpdateDto, { rejectWithValue }) => {
  try {
    const res = await mgmt.put<Survey>(`Surveys/${dto.id}`, dto);
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anket güncellenemedi');
  }
});

export const deleteSurvey = createAsyncThunk('survey/delete', async (id: number, { rejectWithValue }) => {
  try {
    await mgmt.delete(`Surveys/${id}`);
    return id;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anket silinemedi');
  }
});

export const toggleSurveyActive = createAsyncThunk('survey/toggle', async (id: number, { rejectWithValue }) => {
  try {
    await mgmt.patch(`Surveys/${id}/toggle-active`);
    return id;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Durum değiştirilemedi');
  }
});

const surveySlice = createSlice({
  name: 'survey',
  initialState: {
    surveys: [] as Survey[],
    isLoading: false,
    error: null as string | null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchActiveSurveys.pending, (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchActiveSurveys.fulfilled, (state, action) => { state.isLoading = false; state.surveys = action.payload; })
      .addCase(fetchActiveSurveys.rejected, (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(fetchMySurveys.pending, (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchMySurveys.fulfilled, (state, action) => { state.isLoading = false; state.surveys = action.payload; })
      .addCase(fetchMySurveys.rejected, (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(createSurvey.fulfilled, (state, action) => { state.surveys.push(action.payload); })
      .addCase(updateSurvey.fulfilled, (state, action) => {
        const idx = state.surveys.findIndex(s => s.id === action.payload.id);
        if (idx !== -1) state.surveys[idx] = action.payload;
      })
      .addCase(deleteSurvey.fulfilled, (state, action) => {
        state.surveys = state.surveys.filter(s => s.id !== action.payload);
      })
      .addCase(toggleSurveyActive.fulfilled, (state, action) => {
        const s = state.surveys.find(s => s.id === action.payload);
        if (s) s.isActive = !s.isActive;
      });
  },
});

export default surveySlice.reducer;