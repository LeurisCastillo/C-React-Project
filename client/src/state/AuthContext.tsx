import React, { createContext, useContext, useMemo, useState } from "react";

type AuthState = {
  token: string | null;
  email: string | null;
  role: string | null;
};

type AuthContextValue = AuthState & {
  login: (token: string, email: string, role: string) => void;
  logout: () => void;
  isAuthenticated: boolean;
  isAdmin: boolean;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

const STORAGE_KEY = "cc_auth";

function loadAuth(): AuthState {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return { token: null, email: null, role: null };
    return JSON.parse(raw) as AuthState;
  } catch {
    return { token: null, email: null, role: null };
  }
}

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [state, setState] = useState<AuthState>(() => loadAuth());

  const login = (token: string, email: string, role: string) => {
    const next = { token, email, role };
    setState(next);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(next));
  };

  const logout = () => {
    setState({ token: null, email: null, role: null });
    localStorage.removeItem(STORAGE_KEY);
  };

  const value = useMemo<AuthContextValue>(
    () => ({
      ...state,
      login,
      logout,
      isAuthenticated: !!state.token,
      isAdmin: state.role === "Admin"
    }),
    [state]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
