import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import api from '../api/axiosConfig';
import type { LoginRequest, AuthResponse } from '../types/User';

export const login = createAsyncThunk(
  'Account/login',
  async (credentials: LoginRequest, { rejectWithValue }) => {
    try {
      const response = await api.post<AuthResponse>('Account/login', credentials);
      
      // ÇÖZÜM BURADA: response.data.token bir nesne olduğu için, 
      // içindeki asıl string olan .token'ı kaydediyoruz.
      localStorage.setItem('token', response.data.token.token); 
      
      return response.data;
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || 'Giriş başarısız');
    }
  }
);

const authSlice = createSlice({
  name: 'Account',
  initialState: {
    user: null as any,
    token: localStorage.getItem('token'), // Bu zaten string döner
    isLoading: false,
    error: null as string | null,
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
      .addCase(login.pending, (state) => { 
        state.isLoading = true; 
        state.error = null; 
      })
      .addCase(login.fulfilled, (state, action: PayloadAction<AuthResponse>) => {
        const { role, email, userName, userId, token } = action.payload;

        state.isLoading = false;
        
        // State'e de nesneyi değil, içindeki string'i yazıyoruz
        state.token = token.token; 
        
        state.user = {
          id: userId,
          name: userName,
          email: email,
          roles: role
        };

        state.error = null;
      })
      .addCase(login.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
      });
  },
});

export const { logout } = authSlice.actions;
export default authSlice.reducer;