# Bank Account Event Sourcing

Este proyecto implementa un sistema de **Event Sourcing** para la gestión de cuentas bancarias.
La solución está compuesta por tres servicios principales:

- **BankAccount.API**: expone los comandos para crear y modificar cuentas bancarias.
- **BankAccount.Proyecciones.Worker**: procesa los eventos y genera las proyecciones de lectura.
- **BankAccount.Read.API**: expone las proyecciones a través de una API de solo lectura.

Al ejecutar el proyecto, se levantan automáticamente estos tres servicios para permitir la operación completa del sistema.

## Requisitos previos

Antes de iniciar los servicios, es necesario levantar la base de datos ejecutando el siguiente comando:

```bash
cd bank-account-event-sourcing/BankAccount
docker compose up -d bankaccountdatabase
```

Este comando inicia el contenedor de base de datos requerido para almacenar los eventos y las proyecciones.

### Detener la base de datos

Para detener la base de datos:

```bash
docker compose down
```

### Eliminar datos persistidos

En caso de requerir una limpieza completa de los datos almacenados:

```bash
docker compose down
docker volume rm bankaccount_BankAccountDb
```

## Servicios disponibles

### API de Comandos (BankAccount.API)

Esta API permite la ejecución de comandos que modifican el estado de las cuentas bancarias.
Disponible en: [http://localhost:5062/swagger/](http://localhost:5062/swagger/)

#### Endpoints principales:

- **Crear una cuenta bancaria**
  - `POST /CuentasBancarias`
  - **Body**:
    ```json
    {
      "idCliente": "string (opcional)"
    }
    ```

- **Depositar fondos en una cuenta bancaria**
  - `POST /CuentasBancarias/{idCuentaBancaria}/Transacciones/Depositos`
  - **Body**:
    ```json
    {
      "monto": 100.0
    }
    ```

- **Retirar fondos de una cuenta bancaria**
  - `POST /CuentasBancarias/{idCuentaBancaria}/Transacciones/Retiros`
  - **Body**:
    ```json
    {
      "monto": 50.0
    }
    ```

- **Consultar información básica de una cuenta**
  - `GET /CuentasBancarias/{idCuentaBancaria}`

---

### API de Lectura (BankAccount.Read.API)

Esta API permite consultar las proyecciones generadas a partir de los eventos históricos.
Disponible en: [http://localhost:5268/swagger/](http://localhost:5268/swagger/)

#### Endpoints principales:

- **Consultar el estado de una cuenta bancaria**
  - `GET /estado-cuenta/{id}`

- **Consultar los movimientos de una cuenta bancaria**
  - `GET /estado-cuenta/{id}/movimientos`

- **Consultar el saldo total de un cliente**
  - `GET /clientes/{idCliente}/saldo-total`

---

## Resumen de componentes

| Componente | Descripción | URL de documentación |
|:-----------|:------------|:----------------------|
| **API de comandos** | Gestión de cuentas mediante comandos | [http://localhost:5062/swagger/](http://localhost:5062/swagger/) |
| **API de lectura** | Consulta de estados y movimientos | [http://localhost:5268/swagger/](http://localhost:5268/swagger/) |
| **Worker de proyecciones** | Procesamiento de eventos y generación de proyecciones | No aplica |

---

## Ejercicios
- [Proyecciones](Ejercicios-MultiStreamProjection.md)
