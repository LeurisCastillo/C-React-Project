import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../state/AuthContext";

type CreateProductRequest = {
  name: string;
  category: string;
  price: number;
  stock: number;
  description?: string | null;
};

export default function NewProductPage() {
  const auth = useAuth();
  const nav = useNavigate();
  const [form, setForm] = useState<CreateProductRequest>({
    name: "",
    category: "Electronics",
    price: 0,
    stock: 0,
    description: ""
  });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const set = (k: keyof CreateProductRequest, v: any) => setForm((p) => ({ ...p, [k]: v }));

  const validate = () => {
    const errs: string[] = [];
    if (!form.name.trim()) errs.push("Name is required.");
    if (!form.category.trim()) errs.push("Category is required.");
    if (form.price < 0) errs.push("Price must be >= 0.");
    if (form.stock < 0) errs.push("Stock must be >= 0.");
    if ((form.description ?? "").length > 2000) errs.push("Description too long.");
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
      const res = await api.post("/api/products", form, {
        headers: { Authorization: `Bearer ${auth.token}` }
      });

      nav(`/products/${res.data.id}`);
    } catch (err: any) {
      const msg =
        err?.response?.data?.detail ??
        JSON.stringify(err?.response?.data ?? {}) ??
        "Create failed.";
      setError(String(msg));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card" style={{ maxWidth: 680 }}>
      <h2>Add product</h2>

      <form onSubmit={onSubmit}>
        <div className="row">
          <div style={{ flex: 2, minWidth: 240 }}>
            <label>Name</label>
            <input value={form.name} onChange={(e) => set("name", e.target.value)} />
          </div>

          <div style={{ flex: 1, minWidth: 180 }}>
            <label>Category</label>
            <select value={form.category} onChange={(e) => set("category", e.target.value)}>
              <option value="Electronics">Electronics</option>
              <option value="Home">Home</option>
              <option value="Sports">Sports</option>
            </select>
          </div>
        </div>

        <div className="row" style={{ marginTop: 10 }}>
          <div style={{ flex: 1, minWidth: 160 }}>
            <label>Price</label>
            <input
              type="number"
              step="0.01"
              value={form.price}
              onChange={(e) => set("price", Number(e.target.value))}
            />
          </div>
          <div style={{ flex: 1, minWidth: 160 }}>
            <label>Stock</label>
            <input
              type="number"
              value={form.stock}
              onChange={(e) => set("stock", Number(e.target.value))}
            />
          </div>
        </div>

        <div style={{ marginTop: 10 }}>
          <label>Description</label>
          <textarea
            rows={4}
            value={form.description ?? ""}
            onChange={(e) => set("description", e.target.value)}
          />
        </div>

        {error && (
          <div className="card" style={{ marginTop: 12, borderColor: "#7f1d1d" }}>
            {error}
          </div>
        )}

        <div className="row" style={{ marginTop: 12 }}>
          <button className="primary" disabled={loading}>
            {loading ? "Saving..." : "Save"}
          </button>
          <button type="button" onClick={() => nav("/products")}>Cancel</button>
        </div>
      </form>
    </div>
  );
}
