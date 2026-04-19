export interface AuthResponse {
  isSuccess: boolean;
  message: string;
  role?: string;
  firstName?: string; // ← Add
  lastName?: string; // ← Add
}

export interface CurrentUser {
  firstName: string;
  lastName: string;
  role?: string;
}
