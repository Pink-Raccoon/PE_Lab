# -*- coding: utf-8 -*-
"""
Created on Mon May 29 11:15:58 2023

@author: Asha
"""

import pandas as pd
import matplotlib.pyplot as plt


df = pd.read_csv("../UnityProj/timeSeriesRopJulia.csv")

plt.figure(figsize=(20,20))
plt.plot(df[" cubeJulia.position.x"], df[" cubeJulia.position.y"])
plt.ylabel("[s] = m")
plt.xlabel("[s] = m")
plt.title("Bewegung Würfel Julia")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeJulia/Ortdiagramm.png', dpi=300, bbox_inches='tight')
plt.show()


plt.figure(figsize=(20,20))
plt.plot(df["alphaJulia"], df["currentTimeStep"])
plt.ylabel("[t] = s")
plt.xlabel("[s] = alpha")
plt.title("Auslenkung als Funktion der Zeit")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeJulia/Auslenkung.png', dpi=300, bbox_inches='tight')
plt.show()