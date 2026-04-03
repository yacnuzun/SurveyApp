// store/userSlice.ts
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { identityApi } from '../api/axiosConfig';
import type { UserDto } from '../types/User';

export const fetchUsers = createAsyncThunk(
  'user/fetchUsers',
  async (search: string = '', { rejectWithValue }) => {
    try {
      const res = await identityApi.get<UserDto[]>('Account/users', {
        params: { Search: search }
      });
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err.response?.data?.message || 'Kullanıcılar yüklenemedi');
    }
  }
);

const userSlice = createSlice({
  name: 'user',
  initialState: {
    users: [] as UserDto[],
    isLoading: false,
    error: null as string | null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchUsers.pending,    (state) => { state.isLoading = true; state.error = null; })
      .addCase(fetchUsers.fulfilled,  (state, action) => { state.isLoading = false; state.users = action.payload; })
      .addCase(fetchUsers.rejected,   (state, action) => { state.isLoading = false; state.error = action.payload as string; });
  }
});

export default userSlice.reducer;