// src/store/participationSlice.ts

import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_URLS } from '../api/apiConfig';
import type { SurveyDetail } from '../types/Survey';

const mgmt = axios.create({ baseURL: API_URLS.management });
mgmt.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

const follow = axios.create({ baseURL: API_URLS.follow });
follow.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export const fetchSurveyById = createAsyncThunk(
  'participation/fetchById',
  async (id: string, { rejectWithValue }) => {
    try {
      const res = await mgmt.get<SurveyDetail>(`Surveys/${id}`);
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || 'Anket yüklenemedi');
    }
  }
);

export const submitSurveyAnswers = createAsyncThunk(
  'participation/submit',
  async (payload: {
    surveyId: number;
    answers: {
      questionId: number;
      questionText: string;
      optionId: number | null;
      optionText: string | null;
      textAnswer: string | null;
    }[];
  }, { rejectWithValue }) => {
    try {
      const res = await follow.post('Participations/submit', payload);
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || 'Gönderim başarısız');
    }
  }
);

const participationSlice = createSlice({
  name: 'participation',
  initialState: {
    currentSurvey: null as SurveyDetail | null,
    answers: {} as Record<number, any>,
    isLoading: false,
    isSubmitting: false,
    error: null as string | null,
  },
  reducers: {
    setAnswer: (state, action) => {
      const { questionId, value } = action.payload;
      state.answers[questionId] = value;
    },
    resetAnswers: (state) => {
      state.answers = {};
      state.currentSurvey = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchSurveyById.pending, (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchSurveyById.fulfilled, (state, action) => { state.isLoading = false; state.currentSurvey = action.payload; })
      .addCase(fetchSurveyById.rejected, (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(submitSurveyAnswers.pending, (state) => { state.isSubmitting = true; })
      .addCase(submitSurveyAnswers.fulfilled, (state) => { state.isSubmitting = false; state.answers = {}; })
      .addCase(submitSurveyAnswers.rejected, (state, action) => { state.isSubmitting = false; state.error = action.payload as string; });
  },
});

export const { setAnswer, resetAnswers } = participationSlice.actions;
export default participationSlice.reducer;