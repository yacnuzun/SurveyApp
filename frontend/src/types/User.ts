export interface User {
  id: string;
  email: string;
  fullName: string;
  role: string[];
}

export interface AccessToken {
  token: string;
  expiration: string;
}

export interface AuthResponse {
  userId: number;
  userName: string;
  email: string;
  role: string[]; // Backend'de Role (List<string>) demiştik
  token: AccessToken; // Bu bir nesne, direkt string değil!
}

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

export interface LoginRequest {
  email: string;
  password: string;
}