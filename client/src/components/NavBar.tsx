import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../state/AuthContext";

export default function NavBar() {
  const auth = useAuth();
  const nav = useNavigate();

  return (
    <nav>
      <div className="nav-inner container">
        <div className="links">
          <Link to="/products"><b>Products</b></Link>
          <Link to="/weather">Weather</Link>
          {auth.isAdmin && <Link to="/products/new">Add product</Link>}
        </div>

        <div className="links">
          {auth.isAuthenticated ? (
            <>
              <span className="badge">{auth.email} · {auth.role}</span>
              <button
                onClick={() => {
                  auth.logout();
                  nav("/login");
                }}
              >
                Logout
              </button>
            </>
          ) : (
            <Link to="/login">Login</Link>
          )}
        </div>
      </div>
    </nav>
  );
}
