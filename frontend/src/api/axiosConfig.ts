// src/api/axiosConfig.ts

import axios from 'axios';
import { API_URLS } from './apiConfig';

// Varsayılan instance — IdentityService base URL
const api = axios.create({
  baseURL: API_URLS.identity,
});

// Her istekte token'ı header'a ekle
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;