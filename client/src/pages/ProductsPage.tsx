import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../state/AuthContext";

type ProductDto = {
  id: string;
  name: string;
  category: string;
  price: number;
  stock: number;
  description?: string | null;
  createdAtUtc: string;
};

type PagedResult<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
};

export default function ProductsPage() {
  const auth = useAuth();
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [category, setCategory] = useState<string>("");
  const [search, setSearch] = useState<string>("");
  const [data, setData] = useState<PagedResult<ProductDto> | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const totalPages = useMemo(() => {
    if (!data) return 1;
    return Math.max(1, Math.ceil(data.totalCount / data.pageSize));
  }, [data]);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await api.get<PagedResult<ProductDto>>("/api/products", {
        params: {
          page,
          pageSize,
          category: category || undefined,
          search: search || undefined
        }
      });
      setData(res.data);
    } catch {
      setError("Failed to load products.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, category]);

  const onDelete = async (id: string) => {
    if (!auth.isAdmin) return;

    if (!confirm("Delete this product?")) return;

    try {
      await api.delete(`/api/products/${id}`, {
        headers: { Authorization: `Bearer ${auth.token}` }
      });
      await load();
    } catch (err: any) {
      alert(err?.response?.data?.detail ?? "Delete failed.");
    }
  };

  return (
    <div className="card">
      <h2>Products</h2>

      <div className="row" style={{ marginBottom: 12 }}>
        <div style={{ flex: 1, minWidth: 220 }}>
          <label>Category</label>
          <select
            value={category}
            onChange={(e) => {
              setPage(1);
              setCategory(e.target.value);
            }}
          >
            <option value="">All</option>
            <option value="Electronics">Electronics</option>
            <option value="Home">Home</option>
            <option value="Sports">Sports</option>
          </select>
        </div>

        <div style={{ flex: 2, minWidth: 260 }}>
          <label>Search</label>
          <input value={search} onChange={(e) => setSearch(e.target.value)} placeholder="name or description..." />
        </div>

        <div style={{ alignSelf: "end" }}>
          <button onClick={() => { setPage(1); load(); }} disabled={loading}>
            Apply
          </button>
        </div>
      </div>

      {error && <div className="card" style={{ borderColor: "#7f1d1d" }}>{error}</div>}
      {loading && <small className="muted">Loading...</small>}

      {data && (
        <>
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Category</th>
                <th>Price</th>
                <th>Stock</th>
                <th style={{ width: 180 }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((p) => (
                <tr key={p.id}>
                  <td>{p.name}</td>
                  <td>{p.category}</td>
                  <td>{p.price.toFixed(2)}</td>
                  <td>{p.stock}</td>
                  <td>
                    <div className="row">
                      <Link to={`/products/${p.id}`}>
                        <button>Details</button>
                      </Link>

                      {auth.isAdmin && (
                        <button className="danger" onClick={() => onDelete(p.id)}>
                          Delete
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="row" style={{ justifyContent: "space-between", marginTop: 12 }}>
            <small className="muted">
              Total: {data.totalCount} · Page {data.page} / {totalPages}
            </small>

            <div className="row">
              <button disabled={page <= 1} onClick={() => setPage((p) => p - 1)}>Prev</button>
              <button disabled={page >= totalPages} onClick={() => setPage((p) => p + 1)}>Next</button>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
