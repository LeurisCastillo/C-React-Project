import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { api } from "../services/api";

type ProductDto = {
  id: string;
  name: string;
  category: string;
  price: number;
  stock: number;
  description?: string | null;
  createdAtUtc: string;
};

export default function ProductDetailsPage() {
  const { id } = useParams();
  const [product, setProduct] = useState<ProductDto | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    (async () => {
      try {
        const res = await api.get<ProductDto>(`/api/products/${id}`);
        setProduct(res.data);
      } catch {
        setError("Product not found.");
      }
    })();
  }, [id]);

  if (error) return <div className="card" style={{ borderColor: "#7f1d1d" }}>{error}</div>;
  if (!product) return <div className="card">Loading...</div>;

  return (
    <div className="card">
      <h2>{product.name}</h2>
      <small className="muted">{product.category} · Created {new Date(product.createdAtUtc).toLocaleString()}</small>

      <div className="row" style={{ marginTop: 12 }}>
        <div className="card" style={{ flex: 1 }}>
          <b>Price</b>
          <div>{product.price.toFixed(2)}</div>
        </div>
        <div className="card" style={{ flex: 1 }}>
          <b>Stock</b>
          <div>{product.stock}</div>
        </div>
      </div>

      {product.description && (
        <div style={{ marginTop: 12 }}>
          <b>Description</b>
          <p>{product.description}</p>
        </div>
      )}

      <div className="row">
        <Link to="/products"><button>Back</button></Link>
      </div>
    </div>
  );
}
