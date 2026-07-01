# Actividad 5 - Code Review
## Análisis de Código con Problemas

---

## Código analizado

```csharp
[HttpPost]
public IActionResult CrearSolicitud(string usuario, string tipo, string obs)
{
    var conexion = new SqlConnection(
        "Server=localhost;Database=CasaToro;User=sa;Password=1234;");

    conexion.Open();

    var query = "SELECT * FROM Usuarios WHERE Nombre = '" + usuario + "'";
    var cmd = new SqlCommand(query, conexion);
    var resultado = cmd.ExecuteReader();

    var usuarioId = (int)resultado["Id"];

    if ((bool)resultado["Activo"] == false)
        return BadRequest("Usuario inactivo");

    var insert = "INSERT INTO Solicitudes (UsuarioId, TipoId, Observaciones) " +
                 "VALUES ('" + usuarioId + "', '" + tipo + "', '" + obs + "')";

    var cmdInsert = new SqlCommand(insert, conexion);
    cmdInsert.ExecuteNonQuery();

    conexion.Close();

    return Ok("ok");
}
```

---

## Problemas identificados

### ❌ Problema 1 — Cadena de conexión hardcodeada
**Qué hace:** Las credenciales de la base de datos están escritas directamente en el código.  
**Impacto:** Cualquier persona que vea el código tiene acceso a las credenciales de producción.  
**Solución:** Usar `appsettings.json` o variables de entorno para las cadenas de conexión.

---

### ❌ Problema 2 — SQL Injection por concatenación de cadenas
**Qué hace:** Construye el query pegando directamente los valores del usuario.  
**Impacto:** Un atacante puede escribir `' OR '1'='1` y acceder a todos los datos.  
**Solución:** Usar parámetros SQL o Entity Framework que previenen este ataque automáticamente.

---

### ❌ Problema 3 — Uso de SELECT *
**Qué hace:** Trae todas las columnas de la tabla aunque solo se necesiten 2.  
**Impacto:** Desperdicia memoria, ancho de banda y tiempo de procesamiento.  
**Solución:** Especificar solo las columnas necesarias: `SELECT Id, Activo FROM Usuarios`.

---

### ❌ Problema 4 — No valida si el usuario existe
**Qué hace:** Asume que el usuario siempre existe y accede directamente a sus datos.  
**Impacto:** Si el usuario no existe, el código falla con una excepción sin mensaje claro.  
**Solución:** Verificar si el resultado es null antes de continuar.

---

### ❌ Problema 5 — Lógica de negocio en el Controller
**Qué hace:** El Controller decide si el usuario puede crear solicitudes.  
**Impacto:** Viola el principio de responsabilidad única. El Controller solo debe recibir y delegar.  
**Solución:** Mover esa validación al Service donde corresponde.

---

### ❌ Problema 6 — No verifica duplicados
**Qué hace:** Inserta la solicitud sin verificar si ya existe una del mismo tipo ese día.  
**Impacto:** Viola la RN-01: no se permiten solicitudes duplicadas del mismo tipo el mismo día.  
**Solución:** Consultar primero si existe duplicado antes de insertar.

---

### ❌ Problema 7 — No hay manejo de errores
**Qué hace:** No tiene ningún bloque try/catch.  
**Impacto:** Si algo falla, el usuario recibe un error genérico sin explicación y el sistema puede quedar en estado inconsistente.  
**Solución:** Envolver el código en try/catch y responder con mensajes claros según el tipo de error.

---

### ❌ Problema 8 — Conexión a base de datos no se libera correctamente
**Qué hace:** Cierra la conexión manualmente solo al final, sin garantía de que se ejecute.  
**Impacto:** Si hay un error antes del `Close()`, la conexión queda abierta. Con muchos usuarios esto agota el pool de conexiones.  
**Solución:** Usar `using` que garantiza que la conexión se cierre siempre, incluso si hay error.

---

### ❌ Problema 9 — No hay logging
**Qué hace:** No registra ningún evento, ni exitoso ni fallido.  
**Impacto:** En producción es imposible saber qué pasó, quién lo hizo ni cuándo ocurrió.  
**Solución:** Usar `ILogger` para registrar cada operación importante y cada error.

---

### ❌ Problema 10 — No genera seguimiento al crear la solicitud
**Qué hace:** Inserta la solicitud pero no crea el registro de seguimiento correspondiente.  
**Impacto:** Viola la RN-04: toda solicitud debe generar un seguimiento con fecha, usuario y comentario.  
**Solución:** Insertar un registro en la tabla Seguimientos después de crear la solicitud.

---

### ❌ Problema 11 — Respuesta HTTP incorrecta
**Qué hace:** Retorna `Ok("ok")` que es HTTP 200 con un string plano.  
**Impacto:** El estándar REST indica que una creación exitosa debe retornar HTTP 201 (Created) con el recurso creado.  
**Solución:** Retornar `CreatedAtAction` con el DTO de la solicitud creada.

---

## Resumen

| # | Problema | Categoría | Impacto |
|---|---------|-----------|---------|
| 1 | Cadena de conexión hardcodeada | Seguridad | Alto |
| 2 | SQL Injection | Seguridad | Crítico |
| 3 | SELECT * | Rendimiento | Medio |
| 4 | No valida si usuario existe | Calidad | Alto |
| 5 | Lógica de negocio en Controller | Arquitectura | Alto |
| 6 | No verifica duplicados | Negocio | Alto |
| 7 | No hay try/catch | Calidad | Alto |
| 8 | Conexión no liberada correctamente | Rendimiento | Alto |
| 9 | No hay logging | Operación | Alto |
| 10 | No genera seguimiento | Negocio | Alto |
| 11 | Respuesta HTTP incorrecta | API | Medio |