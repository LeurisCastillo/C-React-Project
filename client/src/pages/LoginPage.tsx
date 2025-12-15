import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../state/AuthContext";

type LoginResponse = { accessToken: string; email: string; role: string };

export default function LoginPage() {
  const [email, setEmail] = useState("admin@demo.com");
  const [password, setPassword] = useState("Admin123*");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const auth = useAuth();
  const nav = useNavigate();

  const validate = () => {
    const errs: string[] = [];
    if (!email.trim()) errs.push("Email is required.");
    if (!password.trim()) errs.push("Password is required.");
    if (password.trim().length < 6) errs.push("Password must be at least 6 characters.");
    return errs;
  };

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    const errs = validate();
    if (errs.length) {
      setError(errs.join(" "));
      return;
    }

    setLoading(true);
    try {
      const res = await api.post<LoginResponse>("/api/auth/login", { email, password });
      auth.login(res.data.accessToken, res.data.email, res.data.role);
      nav("/products");
    } catch (err: any) {
      const msg = err?.response?.data?.message ?? "Login failed.";
      setError(String(msg));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card" style={{ maxWidth: 520 }}>
      <h2>Login</h2>
      <small className="muted">
        Default seeded user: <b>admin@demo.com</b> / <b>Admin123*</b>
      </small>

      <form onSubmit={onSubmit} style={{ marginTop: 12 }}>
        <div style={{ marginBottom: 10 }}>
          <label>Email</label>
          <input value={email} onChange={(e) => setEmail(e.target.value)} />
        </div>

        <div style={{ marginBottom: 10 }}>
          <label>Password</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
        </div>

        {error && (
          <div className="card" style={{ marginBottom: 10, borderColor: "#7f1d1d" }}>
            {error}
          </div>
        )}

        <button className="primary" disabled={loading}>
          {loading ? "Signing in..." : "Sign in"}
        </button>
      </form>
    </div>
  );
}
