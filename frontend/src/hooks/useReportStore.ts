// src/hooks/useReportStore.ts  (store/ yerine hooks/ altına taşı)

import { create } from 'zustand';
import axios from 'axios';
import { API_URLS } from '../api/apiConfig';
import type { SurveyReport } from '../types/Report';

const reporting = axios.create({ baseURL: API_URLS.reporting });
reporting.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

interface ReportState {
  report: SurveyReport | null;
  isLoading: boolean;
  getReport: (id: number) => Promise<void>;
}

export const useReportStore = create<ReportState>((set) => ({
  report: null,
  isLoading: false,
  getReport: async (id) => {
    set({ isLoading: true });
    try {
      const res = await reporting.get<SurveyReport>(`Reports/${id}`);
      set({ report: res.data, isLoading: false });
    } catch (error) {
      set({ isLoading: false });
      console.error('Rapor çekilemedi', error);
    }
  },
}));