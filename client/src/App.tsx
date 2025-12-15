import { Navigate, Route, Routes } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import ProductsPage from "./pages/ProductsPage";
import ProductDetailsPage from "./pages/ProductDetailsPage";
import NewProductPage from "./pages/NewProductPage";
import WeatherPage from "./pages/WeatherPage";
import Layout from "./components/Layout";
import ProtectedRoute from "./components/ProtectedRoute";

export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<Navigate to="/products" replace />} />
        <Route path="/login" element={<LoginPage />} />

        <Route path="/products" element={<ProductsPage />} />
        <Route path="/products/:id" element={<ProductDetailsPage />} />

        <Route
          path="/products/new"
          element={
            <ProtectedRoute>
              <NewProductPage />
            </ProtectedRoute>
          }
        />

        <Route path="/weather" element={<WeatherPage />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Route>
    </Routes>
  );
}
