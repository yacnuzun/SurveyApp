// src/api/apiConfig.ts
// Tüm servis URL'leri tek yerden yönetilir

export const API_URLS = {
  identity:    'http://localhost:7220/api',
  management:  'http://localhost:7158/api',
  follow:      'http://localhost:7138/api',
  reporting:   'http://localhost:7025/api',
} as const;