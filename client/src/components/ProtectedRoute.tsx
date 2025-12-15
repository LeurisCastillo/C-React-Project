import { Navigate } from "react-router-dom";
import { useAuth } from "../state/AuthContext";

export default function ProtectedRoute({ children }: { children: JSX.Element }) {
  const auth = useAuth();

  if (!auth.isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  if (!auth.isAdmin) {
    return <Navigate to="/products" replace />;
  }

  return children;
}
