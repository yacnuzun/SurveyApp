import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../api/axiosConfig';
import type { SurveyDetail } from '../types/Survey';

export const fetchSurveyById = createAsyncThunk(
  'participation/fetchById',
  async (id: string) => {
    const response = await api.get<SurveyDetail>(`https://localhost:7158/api/Surveys/${id}`);
    return response.data;
  }
);

export const submitSurveyAnswers = createAsyncThunk(
  'participation/submit',
  async (payload: {
    surveyId: string | number;
    answers: { questionId: number; optionId: number | null; textAnswer: string | null }[];
  }, { rejectWithValue }) => {
    try {
      const response = await api.post('https://localhost:7138/api/Participations/submit', payload);
      return response.data;
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
      .addCase(fetchSurveyById.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchSurveyById.fulfilled, (state, action) => {
        state.isLoading = false;
        state.currentSurvey = action.payload;
      })
      .addCase(fetchSurveyById.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
      })
      .addCase(submitSurveyAnswers.pending, (state) => {
        state.isSubmitting = true;
      })
      .addCase(submitSurveyAnswers.fulfilled, (state) => {
        state.isSubmitting = false;
        state.answers = {};
      })
      .addCase(submitSurveyAnswers.rejected, (state, action) => {
        state.isSubmitting = false;
        state.error = action.payload as string;
      });
  },
});

export const { setAnswer, resetAnswers } = participationSlice.actions;
export default participationSlice.reducer;