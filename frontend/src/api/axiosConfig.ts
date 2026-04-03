// src/api/axiosConfig.ts

import axios from 'axios';
import type{AxiosInstance} from 'axios';
import { API_URLS } from './apiConfig';

// Ortak interceptor ekleyen helper
const addInterceptors = (instance: AxiosInstance): AxiosInstance => {
  // Request — token ekle
  instance.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  });

  // Response — 401 kontrolü
  instance.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        window.location.href = '/login';
      }
      return Promise.reject(error);
    }
  );

  return instance;
};

// Tüm servis instance'ları
export const identityApi  = addInterceptors(axios.create({ baseURL: API_URLS.identity }));
export const mgmtApi      = addInterceptors(axios.create({ baseURL: API_URLS.management }));
export const followApi    = addInterceptors(axios.create({ baseURL: API_URLS.follow }));
export const reportingApi = addInterceptors(axios.create({ baseURL: API_URLS.reporting }));

export default identityApi;