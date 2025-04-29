
# 📜 Ejercicios de Proyecciones y Endpoints - MultiStreamProjection

## 🔖 Enunciado General

En este conjunto de ejercicios trabajaremos sobre **proyecciones de lectura** construidas a partir de eventos de distintas cuentas bancarias de un mismo cliente. El objetivo es exponer estas lecturas a través de **endpoints RESTful** en una API.

### Condiciones obligatorias para todos los ejercicios:

- Cada ejercicio debe resolverse utilizando **MultiStreamProjection** de Marten.
- El procesamiento de eventos debe ser **asíncrono** (`ProjectionLifecycle.Async`).
- La consulta de las proyecciones para los endpoints debe realizarse mediante **IQuerySession**.
- Las respuestas deben seguir el formato JSON indicado en los ejemplos de respuesta.



---

# 📚 Ejercicios

---

## 1. Saldo total por cliente

### 📂 Proyección: `SaldoTotalCliente`

Esta proyección almacena el **saldo total acumulado** de todas las cuentas bancarias asociadas a un cliente.

### 🎯 Ejercicio

Crear un endpoint que permita consultar el saldo total de un cliente.

### 🛠 Especificaciones del endpoint:

- **Método HTTP:** `GET`
- **Ruta:** `/clientes/{idCliente}/saldo-total`
- **Descripción:** Devuelve el saldo total disponible de todas las cuentas asociadas al cliente.
- **Respuesta esperada:**
  - `200 OK` con el saldo.
  - `404 Not Found` si no se encuentra el cliente.

### 📦 Ejemplo de respuesta `200 OK`

```json
{
  "id": "cliente-123",
  "saldoTotal": 15420.75
}
```

---

## 2. Historial de movimientos recientes por cliente

### 📂 Proyección: `HistorialMovimientosCliente`

Esta proyección guarda el **historial de los últimos 10 movimientos** de un cliente en todas sus cuentas bancarias.

Cada movimiento refleja:

- El tipo de movimiento (depósito o retiro),
- El monto del movimiento,
- La cuenta bancaria relacionada,
- La fecha del movimiento.

### 🎯 Ejercicio

Crear un endpoint que permita consultar el historial de movimientos de un cliente.

### 🛠 Especificaciones del endpoint:

- **Método HTTP:** `GET`
- **Ruta:** `/clientes/{idCliente}/movimientos`
- **Descripción:** Devuelve **los últimos 10 movimientos recientes** realizados por el cliente en sus cuentas.
- **Respuesta esperada:**
  - `200 OK` con la lista de movimientos.
  - `404 Not Found` si no hay historial disponible.

### 📦 Ejemplo de respuesta `200 OK`

```json
[
  {
    "idCuentaBancaria": "e1a3a8a7-334d-4d08-b2a1-0d2e1c582c6f",
    "tipoMovimiento": "Deposito",
    "monto": 5000.00,
    "fecha": "2025-04-28T14:30:00Z"
  },
  {
    "idCuentaBancaria": "e1a3a8a7-334d-4d08-b2a1-0d2e1c582c6f",
    "tipoMovimiento": "Retiro",
    "monto": 1200.00,
    "fecha": "2025-04-27T10:15:00Z"
  }
]
```

---

## 3. Totales detallados de movimientos por cliente

### 📂 Proyección: `TotalesCliente`

Esta proyección contiene:

- El **total de dinero depositado** por un cliente.
- El **total de dinero retirado**.
- Un **detalle por cada cuenta bancaria** con sus respectivos totales de depósitos y retiros.

### 🎯 Ejercicio

Crear un endpoint que permita consultar los totales generales y el detalle de movimientos por cuenta.

### 🛠 Especificaciones del endpoint:

- **Método HTTP:** `GET`
- **Ruta:** `/clientes/{idCliente}/totales`
- **Descripción:** Devuelve el resumen total de dinero depositado y retirado, junto con el detalle individual por cada cuenta bancaria del cliente.
- **Respuesta esperada:**
  - `200 OK` con los totales generales y por cuenta.
  - `404 Not Found` si el cliente no tiene movimientos registrados.

### 📦 Ejemplo de respuesta `200 OK`

```json
{
  "id": "cliente-123",
  "totalDepositado": 25000.00,
  "totalRetirado": 7500.00,
  "totalesPorCuenta": [
    {
      "idCuentaBancaria": "11111111-1111-1111-1111-111111111111",
      "totalDepositado": 15000.00,
      "totalRetirado": 5000.00
    },
    {
      "idCuentaBancaria": "22222222-2222-2222-2222-222222222222",
      "totalDepositado": 10000.00,
      "totalRetirado": 2500.00
    }
  ]
}
```

---

# 📋 Resumen de ejercicios

| Ejercicio | Proyección usada | Endpoint esperado | Resultado esperado |
|:----------|:-----------------|:------------------|:-------------------|
| 1 | `SaldoTotalCliente` | `GET /clientes/{idCliente}/saldo-total` | Saldo total |
| 2 | `HistorialMovimientosCliente` | `GET /clientes/{idCliente}/movimientos` | Últimos 10 movimientos |
| 3 | `TotalesCliente` | `GET /clientes/{idCliente}/totales` | Totales generales y por cuenta |
