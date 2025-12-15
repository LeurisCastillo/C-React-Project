import { useEffect, useState } from "react";
import { api } from "../services/api";

type WeatherResponse = {
  city: string;
  latitude: number;
  longitude: number;
  temperatureC: number;
  windSpeedKmh: number;
  timeIso?: string | null;
};

export default function WeatherPage() {
  const [city, setCity] = useState("Santo Domingo");
  const [data, setData] = useState<WeatherResponse | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await api.get<WeatherResponse>("/api/external/weather", { params: { city } });
      setData(res.data);
    } catch {
      setError("Failed to load weather.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className="card" style={{ maxWidth: 740 }}>
      <h2>Weather</h2>
      <small className="muted">Data comes from the backend endpoint /api/external/weather</small>

      <div className="row" style={{ marginTop: 12 }}>
        <div style={{ flex: 1, minWidth: 220 }}>
          <label>City</label>
          <select value={city} onChange={(e) => setCity(e.target.value)}>
            <option value="Santo Domingo">Santo Domingo</option>
            <option value="Santiago">Santiago</option>
            <option value="Punta Cana">Punta Cana</option>
            <option value="Madrid">Madrid</option>
            <option value="New York">New York</option>
          </select>
        </div>

        <div style={{ alignSelf: "end" }}>
          <button onClick={load} disabled={loading}>
            {loading ? "Loading..." : "Refresh"}
          </button>
        </div>
      </div>

      {error && <div className="card" style={{ marginTop: 12, borderColor: "#7f1d1d" }}>{error}</div>}

      {data && (
        <div className="row" style={{ marginTop: 12 }}>
          <div className="card" style={{ flex: 1 }}>
            <b>Temperature</b>
            <div>{data.temperatureC.toFixed(1)} °C</div>
          </div>
          <div className="card" style={{ flex: 1 }}>
            <b>Wind</b>
            <div>{data.windSpeedKmh.toFixed(1)} km/h</div>
          </div>
          <div className="card" style={{ flex: 1 }}>
            <b>Time</b>
            <div>{data.timeIso ?? "-"}</div>
          </div>
        </div>
      )}
    </div>
  );
}
