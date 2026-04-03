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

export interface LoginRequest {
  email: string;
  password: string;
}

export interface UserDto {
  userId: number;
  userName: string;
  email: string;
  status: boolean;
}
