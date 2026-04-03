import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { mgmtApi } from '../api/axiosConfig';
import type { Survey, SurveyCreateDto, SurveyUpdateDto } from '../types/Survey';

export const fetchActiveSurveys = createAsyncThunk('survey/fetchActive', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmtApi.get<Survey[]>('Surveys/get-all-active');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anketler yüklenemedi');
  }
});

export const fetchAllSurveysAdmin = createAsyncThunk('survey/fetchAllAdmin', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmtApi.get<Survey[]>('Surveys/get-all-admin');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anketler yüklenemedi');
  }
});

export const fetchMySurveys = createAsyncThunk('survey/fetchMy', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmtApi.get<Survey[]>('Surveys/my-surveys');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anketleriniz yüklenemedi');
  }
});

export const createSurvey = createAsyncThunk('survey/create', async (dto: SurveyCreateDto, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.post('Surveys', dto);
    dispatch(fetchAllSurveysAdmin()); // ← liste yenile
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anket oluşturulamadı');
  }
});

export const updateSurvey = createAsyncThunk('survey/update', async (dto: SurveyUpdateDto, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.put(`Surveys/${dto.id}`, dto);
    dispatch(fetchAllSurveysAdmin()); // ← liste yenile
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anket güncellenemedi');
  }
});

export const deleteSurvey = createAsyncThunk('survey/delete', async (id: number, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.delete(`Surveys/${id}`);
    dispatch(fetchAllSurveysAdmin()); // ← liste yenile
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Anket silinemedi');
  }
});

export const toggleSurveyActive = createAsyncThunk('survey/toggle', async (id: number, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.patch(`Surveys/${id}/toggle-active`);
    dispatch(fetchAllSurveysAdmin()); // ← liste yenile
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
      .addCase(fetchActiveSurveys.pending,      (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchActiveSurveys.fulfilled,    (state, action) => { state.isLoading = false; state.surveys = action.payload; })
      .addCase(fetchActiveSurveys.rejected,     (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(fetchAllSurveysAdmin.pending,    (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchAllSurveysAdmin.fulfilled,  (state, action) => { state.isLoading = false; state.surveys = action.payload; })
      .addCase(fetchAllSurveysAdmin.rejected,   (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(fetchMySurveys.pending,          (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchMySurveys.fulfilled,        (state, action) => { state.isLoading = false; state.surveys = action.payload; })
      .addCase(fetchMySurveys.rejected,         (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(createSurvey.rejected,           (state, action) => { state.error = action.payload as string; })
      .addCase(updateSurvey.rejected,           (state, action) => { state.error = action.payload as string; })
      .addCase(deleteSurvey.rejected,           (state, action) => { state.error = action.payload as string; })
      .addCase(toggleSurveyActive.rejected,     (state, action) => { state.error = action.payload as string; });
  },
});

export default surveySlice.reducer;