import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { mgmtApi } from '../api/axiosConfig';
import type { AnswerTemplate, AnswerTemplateCreateDto, AnswerTemplateUpdateDto } from '../types/AnswerTemplate';

export const fetchTemplates = createAsyncThunk('answerTemplate/fetchAll', async (_, { rejectWithValue }) => {
  try {
    const res = await mgmtApi.get<AnswerTemplate[]>('AnswerTemplates');
    return res.data;
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Şablonlar yüklenemedi');
  }
});

export const createTemplate = createAsyncThunk('answerTemplate/create', async (dto: AnswerTemplateCreateDto, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.post('AnswerTemplates', dto);
    dispatch(fetchTemplates()); // ← liste yenile
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Şablon oluşturulamadı');
  }
});

export const updateTemplate = createAsyncThunk('answerTemplate/update', async (dto: AnswerTemplateUpdateDto, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.put(`AnswerTemplates/${dto.id}`, dto);
    dispatch(fetchTemplates()); // ← liste yenile
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.message || 'Şablon güncellenemedi');
  }
});

export const deleteTemplate = createAsyncThunk('answerTemplate/delete', async (id: number, { rejectWithValue, dispatch }) => {
  try {
    await mgmtApi.delete(`AnswerTemplates/${id}`);
    dispatch(fetchTemplates()); // ← liste yenile
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
      .addCase(fetchTemplates.pending,   (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchTemplates.fulfilled, (state, action) => { state.isLoading = false; state.templates = action.payload; })
      .addCase(fetchTemplates.rejected,  (state, action) => { state.isLoading = false; state.error = action.payload as string; })
      .addCase(createTemplate.rejected,  (state, action) => { state.error = action.payload as string; })
      .addCase(updateTemplate.rejected,  (state, action) => { state.error = action.payload as string; })
      .addCase(deleteTemplate.rejected,  (state, action) => { state.error = action.payload as string; });
  },
});

export default answerTemplateSlice.reducer;