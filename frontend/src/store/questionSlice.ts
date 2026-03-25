import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { Question, QuestionCreateDto, QuestionUpdateDto } from '../types/Survey';
import axios from 'axios';
import { API_URLS } from '../api/apiConfig';

 
const mgmt = axios.create({ baseURL: API_URLS.management });
mgmt.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export const fetchQuestions = createAsyncThunk('question/fetchAll', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmt.get<Question[]>('Questions');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Sorular yüklenemedi');
  }
});
 
export const createQuestion = createAsyncThunk('question/create', async (dto: QuestionCreateDto, { rejectWithValue }) => {
  try {
    const res = await mgmt.post<Question>('Questions', dto);
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Soru oluşturulamadı');
  }
});
 
export const updateQuestion = createAsyncThunk('question/update', async (dto: QuestionUpdateDto, { rejectWithValue }) => {
  try {
    const res = await mgmt.put<Question>(`Questions/${dto.id}`, dto);
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Soru güncellenemedi');
  }
});
 
export const deleteQuestion = createAsyncThunk('question/delete', async (id: number, { rejectWithValue }) => {
  try {
    await mgmt.delete(`Questions/${id}`);
    return id;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Soru silinemedi');
  }
});
 
const questionSlice = createSlice({
  name: 'question',
  initialState: {
    questions: [] as Question[],
    isLoading: false,
    error: null as string | null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchQuestions.pending, (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchQuestions.fulfilled, (state, action) => { state.isLoading = false; state.questions = action.payload; })
      .addCase(fetchQuestions.rejected, (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(createQuestion.fulfilled, (state, action) => { state.questions.push(action.payload); })
      .addCase(updateQuestion.fulfilled, (state, action) => {
        const idx = state.questions.findIndex(q => q.id === action.payload.id);
        if (idx !== -1) state.questions[idx] = action.payload;
      })
      .addCase(deleteQuestion.fulfilled, (state, action) => {
        state.questions = state.questions.filter(q => q.id !== action.payload);
      });
  },
});
 
export default questionSlice.reducer;
