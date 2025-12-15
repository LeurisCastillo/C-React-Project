# Candidates Channels – Fullstack React + ASP.NET Core (Clean Architecture)

Este repositorio cumple con los requisitos de la prueba técnica (React + ASP.NET Core, endpoints REST, SQL Server, JWT y consumo de API pública externa). fileciteturn1file1L22-L47

## Estructura

- `server/` Backend ASP.NET Core siguiendo **Clean Architecture** (Domain / Application / Infrastructure / WebApi). fileciteturn1file2L22-L27
- `client/` Frontend React con routing, estado global (Context API), formularios con validación, consumo de API.

## Requisitos funcionales cubiertos

- Auth:
  - `POST /api/auth/login` devuelve JWT. fileciteturn1file0L31-L34
- Products:
  - `GET /api/products` con filtros básicos (category/search) + paginación. fileciteturn1file0L36-L38
  - `GET /api/products/{id}` detalle. fileciteturn1file1L37-L41
  - `POST /api/products` crea (protegido con JWT rol Admin).
  - `DELETE /api/products/{id}` elimina (protegido con JWT rol Admin). fileciteturn1file0L40-L44
- API pública externa:
  - `GET /api/external/weather?city=...` consume Open-Meteo desde backend y retorna datos filtrados. fileciteturn1file0L46-L49

## Ejecutar localmente

### 1) SQL Server (opción rápida con Docker)

En la raíz:

```bash
docker compose up -d
```

> Usuario: `sa`  
> Password: `Your_password123`

### 2) Backend

```bash
cd server
dotnet restore
dotnet run --project src/CandidatesChannels.WebApi
```

- Swagger: `http://localhost:5044/swagger`

**Usuario sembrado (seed):**
- Email: `admin@demo.com`
- Password: `Admin123*`

> Cambia el `Jwt:Key` en `server/src/CandidatesChannels.WebApi/appsettings.json` por una clave larga (>= 32 chars).

### 3) Frontend

```bash
cd client
cp .env.example .env
npm install
npm run dev
```

- App: `http://localhost:5173`

## Migraciones (si prefieres manejarlas manualmente)

```bash
cd server
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/CandidatesChannels.Infrastructure --startup-project src/CandidatesChannels.WebApi
dotnet ef database update --project src/CandidatesChannels.Infrastructure --startup-project src/CandidatesChannels.WebApi
```

## Testing (básico)

```bash
cd server
dotnet test
```
