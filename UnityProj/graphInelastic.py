import pandas as pd
import matplotlib.pyplot as plt


df = pd.read_csv("../UnityProj/TimeSeries/time_seriesInelastic.csv")

plt.figure(figsize=(20,20))
plt.subplot(4,1,1)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeJulia.position.x"])
plt.ylabel("s")
plt.xlabel("t")
plt.xlim(15,23)
plt.title("Ort als Funktion der Zeit")


plt.subplot(4,1,2)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeRomeo.velocity.x"])
plt.ylabel("v")
plt.xlabel("t")
plt.title("Geschwindigkeit Cube 1 als Funktion der Zeit")


plt.subplot(4,1,3)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeJulia.velocity.x"])
plt.ylabel("v")
plt.xlabel("t")
plt.title("Geschwindigkeit Cube 2 als Funktion der Zeit")


plt.subplot(4,1,4)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeRomeoKinetic"])
plt.ylabel("J")
plt.xlabel("t")
plt.title("Energie Cube 1 als Funktion der Zeit")

plt.show()

plt.plot(df["cubeJuliaTimeStep"], df[" cubeJuliaImpulse"])
plt.ylabel("Ns")
plt.xlabel("t")
plt.title("Impuls Cube 2 als Funktion der Zeit")


plt.figure(figsize=(20,20))
plt.subplot(4,1,1)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeRomeoImpulse"])
plt.ylabel("Ns")
plt.xlabel("t")
plt.title("Impuls Cube 1 als Funktion der Zeit")

plt.subplot(4,1,2)
plt.plot(df["cubeJuliaTimeStep"], df[" velocityEnd"])
plt.ylabel("s")
plt.xlabel("t")
plt.title("Endgeschwindigkeit als Funktion der Zeit")


plt.subplot(4,1,3)
plt.plot(df["cubeJuliaTimeStep"], df[" cubeKineticEnd"])
plt.ylabel("J")
plt.xlabel("t")
plt.title("Endkinetik als Funktion der Zeit")


plt.subplot(4,1,4)
plt.plot(df["cubeJuliaTimeStep"], df[" forceOnJulia "])
plt.ylabel("N")
plt.xlabel("t")
plt.title("Kraft auf Cube 2 als Funktion der Zeit")
plt.show()
