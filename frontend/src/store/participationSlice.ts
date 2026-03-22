import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../api/axiosConfig';
import { type SurveyDetail } from '../types/Survey';

export const fetchSurveyById = createAsyncThunk(
  'participation/fetchById',
  async (id: string) => {
    // Kendi endpoint'ine göre güncelle: surveys/get-with-questions/5
    const response = await api.get<SurveyDetail>(`https://localhost:7158/api/Surveys/${id}`);
    return response.data;
  }
);

const participationSlice = createSlice({
  name: 'participation',
  initialState: {
    currentSurvey: null as SurveyDetail | null,
    answers: {} as Record<number, any>, // {questionId: answerValue}
    isLoading: false
  },
  reducers: {
    setAnswer: (state, action) => {
      const { questionId, value } = action.payload;
      state.answers[questionId] = value;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchSurveyById.pending, (state) => { state.isLoading = true; })
      .addCase(fetchSurveyById.fulfilled, (state, action) => {
        state.isLoading = false;
        state.currentSurvey = action.payload;
      });
  }
});

export const { setAnswer } = participationSlice.actions;
export const submitSurveyAnswers = createAsyncThunk(
  'participations/submit',
  async (payload: any) => {
    // Kendi Participation/Follow API endpoint'ini buraya yaz
    const response = await api.post('https://localhost:7138/api/Participations/submit', payload);
    return response.data;
  }
);
export default participationSlice.reducer;