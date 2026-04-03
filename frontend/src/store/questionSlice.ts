import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { Question, QuestionCreateDto, QuestionUpdateDto } from '../types/Survey';
import { mgmtApi } from '../api/axiosConfig';

export const fetchQuestions = createAsyncThunk('question/fetchAll', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmtApi.get<Question[]>('Questions');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Sorular yüklenemedi');
  }
});

export const createQuestion = createAsyncThunk('question/create', async (dto: QuestionCreateDto, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.post('Questions', dto);
    dispatch(fetchQuestions()); // ← liste yenile
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Soru oluşturulamadı');
  }
});

export const updateQuestion = createAsyncThunk('question/update', async (dto: QuestionUpdateDto, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.put(`Questions/${dto.id}`, dto);
    dispatch(fetchQuestions()); // ← liste yenile
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Soru güncellenemedi');
  }
});

export const deleteQuestion = createAsyncThunk('question/delete', async (id: number, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.delete(`Questions/${id}`);
    dispatch(fetchQuestions()); // ← liste yenile
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
      .addCase(fetchQuestions.pending,   (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchQuestions.fulfilled, (state, action) => { state.isLoading = false; state.questions = action.payload; })
      .addCase(fetchQuestions.rejected,  (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(createQuestion.rejected,  (state, action) => { state.error = action.payload as string; })
      .addCase(updateQuestion.rejected,  (state, action) => { state.error = action.payload as string; })
      .addCase(deleteQuestion.rejected,  (state, action) => { state.error = action.payload as string; });
  },
});

export default questionSlice.reducer;