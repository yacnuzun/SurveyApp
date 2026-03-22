export interface AccessToken {
  token: string;
  expiration: string;
}

export interface AuthResponse {
  userId: number;
  userName: string;
  email: string;
  role: string[]; // Backend'deki Role (List<string>) buraya dizi olarak gelir
  token: AccessToken;
}