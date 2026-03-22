import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../api/axiosConfig';
import type { LoginRequest, AuthResponse } from '../types/User';

export const login = createAsyncThunk(
  'Account/login',
  async (credentials: LoginRequest, { rejectWithValue }) => {
    try {
      const response = await api.post<AuthResponse>('Account/login', credentials);
      localStorage.setItem('token', response.data.token); // Token'ı kalıcı yap
      return response.data;
    } catch (err: any) {
      return rejectWithValue(err.response.data.message || 'Giriş başarısız');
    }
  }
);

const authSlice = createSlice({
  name: 'Account',
  initialState: {
  user: null as any, // İleride User tipini buraya bağlarız
  token: localStorage.getItem('token'),
  isLoading: false,
  error: null as string | null, // Tipini açıkça belirttik
},
  reducers: {
    logout: (state) => {
      state.user = null;
      state.token = null;
      localStorage.removeItem('token');
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => { state.isLoading = true; state.error = null; })
      .addCase(login.fulfilled, (state, action) => {
        state.isLoading = false;
        state.user = action.payload.user;
        state.token = action.payload.token;
      })
      .addCase(login.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
      });
  },
});

export const { logout } = authSlice.actions;
export default authSlice.reducer;