import random
import time
import requests

# Endpoint expuesto por tu API en .NET
API_URL = "http://localhost:5046/api/v1/Telemetria"

# Flota simulada de recorridos urbanos
FLOTA = [
    {"patente": "FLOT-101", "recorrido": "B01"},
    {"patente": "FLOT-102", "recorrido": "B02"},
    {"patente": "FLOT-103", "recorrido": "B16"},
    {"patente": "FLOT-104", "recorrido": "B04"},
]

print("=== INICIANDO SIMULADOR DE TELEMETRÍA (Python -> .NET) ===")

while True:
  bus = random.choice(FLOTA)

  # Generar telemetría aleatoria
  velocidad = round(random.uniform(0, 75), 1)
  en_paradero = True if velocidad == 0 and random.random() > 0.4 else False
  combustible = round(random.uniform(8, 95), 1)

  payload = {
      "patente": bus["patente"],
      "recorrido": bus["recorrido"],
      "velocidad": velocidad,
      "enParadero": en_paradero,
      "combustiblePct": combustible,
  }

  try:
    response = requests.post(API_URL, json=payload, timeout=3)
    if response.status_code == 200:
      data = response.json()
      alertas = data.get("alertas", [])
      alerta_txt = " | ".join(alertas) if alertas else "Sin alertas"
      print(
          f"[{bus['patente']} | {bus['recorrido']}] "
          f"Vel: {velocidad:4.1f} km/h | Comb: {combustible:4.1f}% | "
          f"Paradero: {en_paradero} -> Estado: {data.get('estadoOperacional')} [{alerta_txt}]"
      )
    else:
      print(f"Error {response.status_code}: {response.text}")
  except requests.exceptions.RequestException as e:
    print(f"Error de conexión con la API .NET: {e}")

  time.sleep(2)