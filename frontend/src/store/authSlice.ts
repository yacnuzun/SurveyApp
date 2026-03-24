import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import api from '../api/axiosConfig';
import type { LoginRequest, AuthResponse } from '../types/User';

export const login = createAsyncThunk(
  'auth/login',
  async (credentials: LoginRequest, { rejectWithValue }) => {
    try {
      const response = await api.post<AuthResponse>('Account/login', credentials);
      // token bir nesne: { token: string, expiration: string }
      localStorage.setItem('token', response.data.token.token);
      return response.data;
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || 'Giriş başarısız');
    }
  }
);

interface AuthState {
  user: {
    id: number;
    name: string;
    email: string;
    roles: string[];
  } | null;
  token: string | null;
  isLoading: boolean;
  error: string | null;
}

const initialState: AuthState = {
  user: null,
  token: localStorage.getItem('token'),
  isLoading: false,
  error: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
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
        state.token = token.token;
        state.user = {
          id: userId,
          name: userName,
          email: email,
          roles: role,
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