# BCP_ER

Solución con:
- ApiER (ASP.NET Core 8 Minimal API + Swagger)
- FrontER (WinForms .NET 8)

## Requisitos
- Visual Studio 2022 Community
- .NET 8 SDK
- SQL Server local (SQLEXPRESS o LocalDB)

## Configuración
- Edita `ApiER/appsettings.json` con tu ConnectionString (local).
- Opcional: usa User Secrets para no commitear credenciales.

## Cómo correr
1. Crear BD y SPs (ver carpeta `scripts/` si aplica).
2. En VS: **Mostrar siempre nodo de solución** y configurar **Proyectos de inicio múltiples**: ApiER=Start, FrontER=Start.
3. F5: Swagger (API) y FrontER (WinForms) se abren.

## Probar
- En Swagger: Authorize (Basic) si aplica.
- FrontER: Login con la misma base URL y credenciales; pestañas para Crear/Buscar/Baja/Reporte.
