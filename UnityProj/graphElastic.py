import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv("./TimeSeries/time_seriesElastic_pt2.csv")

#plt.figure(figsize=(20,20))
#plt.subplot(4,1,1)
plt.plot(df["currentTimeStep"], df[" cubeRomeo.position.x"])
plt.ylabel("[s] = m")
plt.xlabel("[t] = s")
plt.title("Ort als Funktion der Zeit")
plt.savefig('../Semesterprojekt Physik Engines/images/Elastisch/OrtAlsFunktionDerZeit.png', dpi=300, bbox_inches='tight')
plt.show()

#plt.subplot(4,1,2)
plt.plot(df["currentTimeStep"], df[" cubeRomeo.velocity.x"])
plt.ylabel("[v] = m/s")
plt.xlabel("[t] = s")
plt.title("Geschwindigkeit als Funktion der Zeit")
plt.savefig('../Semesterprojekt Physik Engines/images/Elastisch/GeschwindigkeitAlsFunktionDerZeit.png', dpi=300, bbox_inches='tight')
plt.show()

#%%
plt.subplot(4,1,3)
plt.plot(df["currentTimeStep"], df[" cubeRomeoKinetic"])
plt.ylabel("[J] = Nm")
plt.xlabel("[t] = s")
plt.title("Kinetische Energie als Funktion der Zeit")
plt.show()

plt.subplot(4,1,4)
plt.plot(df["currentTimeStep"], df[" springPotentialEnergy"])
plt.ylabel("[J] = Nm")
plt.xlabel("[t] = s")
plt.title("Potentielle Energie als Funktion der Zeit")

plt.show()


