using System.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// DI y Swagger
builder.Services.AddSingleton<Db>();
builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("Basic", null);
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BCP ER API", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "basic"} }, Array.Empty<string>() }
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

// --- Endpoints ---

// Auth ping
app.MapPost("/api/auth/login", (HttpContext ctx) =>
{
    return Results.Ok(new { ok = true, user = ctx.User.Identity?.Name });
}).RequireAuthorization();

// Alta cliente + cuenta
app.MapPost("/api/cliente-cuenta", async (Db db, RegistrarDto dto) =>
{
    var ps = new[]
    {
        new SqlParameter("@CARNET_VC", dto.CarnetVc),
        new SqlParameter("@PATERNO_VC", dto.PaternoVc),
        new SqlParameter("@MATERNO_VC", dto.MaternoVc),
        new SqlParameter("@NOMBRES_VC", dto.NombresVc),
        new SqlParameter("@FECHA_NACIMIENTO_DT", dto.FechaNacimientoDt),
        new SqlParameter("@CELULAR_IN", dto.CelularIn),
        new SqlParameter("@CORREO_VC", dto.CorreoVc),
        new SqlParameter("@ESTADO_CLIENTE", dto.EstadoCliente),
        new SqlParameter("@NRO_CUENTA_VC", dto.NroCuentaVc),
        new SqlParameter("@TIPO_CUENTA_VC", dto.TipoCuentaVc),
        new SqlParameter("@SALDO_DC", dto.SaldoDc),
        new SqlParameter("@ESTADO_CUENTA", dto.EstadoCuenta)
    };
    await db.ExecNonQueryAsync("sp_RegistrarClienteConCuenta", ps);
    return Results.Ok(new { ok = true });
}).RequireAuthorization();

// Baja cliente
app.MapPut("/api/clientes/{id:int}/baja", async (Db db, int id) =>
{
    await db.ExecNonQueryAsync("sp_BajaCliente", new SqlParameter("@CLIENTE_ID", id));
    return Results.NoContent();
}).RequireAuthorization();

// Baja cuenta
app.MapPut("/api/cuentas/{id}/baja", async (Db db, string id) =>
{
    await db.ExecNonQueryAsync("sp_BajaCuenta", new SqlParameter("@CUENTA_ID", id));
    return Results.NoContent();
}).RequireAuthorization();

// Buscar por CI  -> DataTable a lista JSON-friendly
app.MapGet("/api/buscar", async (Db db, string ci) =>
{
    var dt = await db.ExecTableAsync("sp_BuscarClientePorCI", new SqlParameter("@CARNET_VC", ci));
    return Results.Ok(dt.ToList()); // requiere tu DataTableExtensions.ToList()
}).RequireAuthorization();

// Consulta cliente + cuentas
app.MapGet("/api/cliente-cuentas", async (Db db, int? id, string? ci) =>
{
    var ps = new[]
    {
        new SqlParameter("@CLIENTE_ID", (object?)id ?? DBNull.Value),
        new SqlParameter("@CARNET_VC", (object?)ci ?? DBNull.Value)
    };
    var dt = await db.ExecTableAsync("sp_ConsultaClienteConCuentas", ps);
    return Results.Ok(dt.ToList());
}).RequireAuthorization();

// Reporte (primer resultset)
app.MapGet("/api/reporte", async (Db db, DateTime desde, DateTime hasta) =>
{
    var ps = new[]
    {
        new SqlParameter("@Desde", desde),
        new SqlParameter("@Hasta", hasta)
    };
    var dt = await db.ExecTableAsync("sp_ReporteRegistrosYTotalCuentas", ps);
    return Results.Ok(dt.ToList());
}).RequireAuthorization();

app.Run();

// DTO
public record RegistrarDto(
    string CarnetVc,
    string PaternoVc,
    string MaternoVc,
    string NombresVc,
    DateTime FechaNacimientoDt,
    int CelularIn,
    string CorreoVc,
    string EstadoCliente,
    string NroCuentaVc,
    string TipoCuentaVc,
    decimal SaldoDc,
    string EstadoCuenta
);
