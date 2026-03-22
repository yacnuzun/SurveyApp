// src/api/axiosConfig.ts
import axios, { type InternalAxiosRequestConfig } from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7220/api',
});

api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => { // Tipi ekledik
    const token = localStorage.getItem('token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default api;