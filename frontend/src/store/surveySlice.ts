import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../api/axiosConfig';
import type { Survey } from '../types/Survey';

export const fetchActiveSurveys = createAsyncThunk(
  'survey/fetchActive',
  async (_, { rejectWithValue }) => {
    try {
      console.log("Anketler çekiliyor...");
    const response = await api.get('https://localhost:7158/api/Surveys/get-all-active');
    return response.data;
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || 'Anketler yüklenemedi');
    }
  }
);


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
      .addCase(fetchActiveSurveys.pending, (state) => { state.isLoading = true; })
      .addCase(fetchActiveSurveys.fulfilled, (state, action) => {
        state.isLoading = false;
        state.surveys = action.payload;
      })
      .addCase(fetchActiveSurveys.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
      });
  },
});

export default surveySlice.reducer;