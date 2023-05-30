# -*- coding: utf-8 -*-
"""
Created on Mon May 29 11:15:58 2023

@author: Asha
"""

import pandas as pd
import matplotlib.pyplot as plt


df_rom = pd.read_csv("./TimeSeries/timeSeriesRopeRomeo.csv")
df_jul = pd.read_csv("./TimeSeries/timeSeriesRopeJulia.csv")

#plt.figure(figsize=(20,20))
plt.plot(df_rom[" cubeRomeo.position.x"], df_rom[" cubeRomeo.position.y"], label="Romeo")
plt.plot(df_jul[" cubeJulia.position.x"], df_jul[" cubeJulia.position.y"], label="Julia")
plt.xlabel("Position x [s] = m")
plt.ylabel("Position y [s] = m")
plt.title("Bewegung Würfel Romeo & Julia")
plt.legend()
plt.savefig('../Semesterprojekt Physik Engines/images/ropeJulia/Ortdiagramm.png', dpi=300, bbox_inches='tight')
plt.show()


plt.plot(df_rom["currentTimeStep"],df_rom["degree"], label="Romeo")
plt.plot(df_jul["currentTimeStep"],df_jul[" degree"], label="Julia")
plt.ylabel("[s] = alpha")
plt.xlabel("[t] = s")
plt.legend()
plt.title("Auslenkung in Grad als Funktion der Zeit")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeJulia/AuslenkungDeg.png', dpi=300, bbox_inches='tight')
plt.show()


#%%
#plt.figure(figsize=(20,20))
plt.plot(df[" alphaJulia"], df["currentTimeStep"])
plt.ylabel("[t] = s")
plt.xlabel("[s] = alpha")
plt.title("Auslenkung in Rad als Funktion der Zeit")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeJulia/Auslenkung.png', dpi=300, bbox_inches='tight')
plt.show()