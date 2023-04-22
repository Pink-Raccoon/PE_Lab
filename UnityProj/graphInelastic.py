import pandas as pd
import matplotlib.pyplot as plt


df = pd.read_csv("../UnityProj/time_seriesInelastic.csv")

plt.figure(figsize=(20,20))
plt.subplot(4,1,1)
plt.plot(df["cubeJuliaTimeStep"], df[" GesamtImpluls"])
plt.ylabel("[p] = Ns")
plt.xlabel("[t] = s")
plt.xlim(0,20)
plt.title("Gesamtimpuls als Funktion der Zeit")
plt.show()

plt.figure(figsize=(20,20))
plt.subplot(4,1,1)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeJulia.position.x"])
plt.ylabel("[s] = m")
plt.xlabel("[t] = s")
plt.xlim(15,18)
plt.title("Ort Julia als Funktion der Zeit")
plt.show()

plt.subplot(4,1,2)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeRomeo.velocity.x"])
plt.ylabel("[v] = m/s")
plt.xlabel("[t] = s")
plt.title("Geschwindigkeit Romeo als Funktion der Zeit")
plt.show()

plt.subplot(4,1,3)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeJulia.velocity.x"])
plt.ylabel("[v] = m/s")
plt.xlabel("[t] = s")
plt.title("Geschwindigkeit Julia als Funktion der Zeit")
plt.show()

plt.subplot(4,1,4)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeRomeoKinetic"])
plt.ylabel("[J] = Nm")
plt.xlabel("[t] = s")
plt.title("Energie Romeo als Funktion der Zeit")

plt.show()

plt.plot(df["cubeJuliaTimeStep"], df[" cubeJuliaImpulse"])
plt.ylabel("[p] = Ns")
plt.xlabel("[t] = s")
plt.title("Impuls Julia als Funktion der Zeit")
plt.show()

plt.figure(figsize=(20,20))
plt.subplot(4,1,1)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeRomeoImpulse"])
plt.ylabel("[p] = Ns")
plt.xlabel("[t] = s")
plt.title("Impuls Romeo als Funktion der Zeit")
plt.show()

plt.subplot(4,1,2)
plt.plot(df["cubeJuliaTimeStep"], df[" velocityEnd"])
plt.ylabel("[v] = m/s")
plt.xlabel("[t] = s")
plt.title("Endgeschwindigkeit als Funktion der Zeit")
plt.show()

plt.subplot(4,1,3)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeKineticEnd"])
plt.ylabel("[J] = Nm")
plt.xlabel("[t] = s")
plt.title("Endkinetik als Funktion der Zeit")
plt.show()

plt.subplot(4,1,4)
plt.plot(df["cubeJuliaTimeStep"], df[" forceOnJulia"])
plt.ylabel("N")
plt.xlabel("[t] = s")
plt.title("Kraft auf Julia als Funktion der Zeit")
plt.show()
