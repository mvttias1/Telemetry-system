Public Transport Fleet Telemetry & Monitoring System:

Sistema desacoplado de telemetría e ingesta de datos en tiempo real diseñado para la supervisión y control operacional 
de flotas de transporte público metropolitano (Red Movilidad).

---------------------------------------------------------------------------------------------------------------------------

Contexto Operacional:

En la operación de transporte urbano, el cumplimiento de los programas de frecuencia, la seguridad de ruta y la 
prevención de pannes mecánicas son factores críticos frente al ente regulador (DTPM). Este proyecto implementa una 
solución full stack backend que simula la recepción masiva de parámetros desde buses en tránsito, evalúa reglas de negocio 
en milisegundos y persiste los registros para trazabilidad y auditoría.

---------------------------------------------------------------------------------------------------------------------------

Arquitectura de la Solución:

El sistema implementa una arquitectura desacoplada basada en microservicios y clientes de telemetría:


┌─────────────────────────────────┐
│     Dispositivos de Flota       │
│  (Simulador GPS/Sensores en     │
│             Python)             │
└────────────────┬────────────────┘
                 │
                 │ HTTP POST / JSON (Cada 2s)
                 ▼
┌─────────────────────────────────┐
│       Backend Transaccional     │
│   (ASP.NET Core Web API / .NET) │
│  - Validación de esquemas       │
│  - Motor de reglas operativas   │
│  - Documentación Swagger/OpenAPI│
└────────────────┬────────────────┘
                 │
                 │ Entity Framework Core
                 ▼
┌─────────────────────────────────┐
│       Capa de Persistencia      │
│         (SQLite Database)       │
│  - Registro histórico           │
│  - Auditoría de infracciones    │
└─────────────────────────────────┘


Instrucciones de Instalación y Ejecución:


1. Levantar la API (.NET)
cd StuBackend
dotnet run

Acceso a Swagger: http://localhost:5046/swagger/
---------------------------------------------------------------------------------------------------------------------------
2. Iniciar el Simulador de Flota (Python)
En una terminal secundaria:

python -m venv .venv
# Activar entorno virtual según SO (Windows: .venv\Scripts\Activate.ps1)
pip install requests
python simulador.py
---------------------------------------------------------------------------------------------------------------------------
