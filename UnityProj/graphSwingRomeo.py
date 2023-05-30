# -*- coding: utf-8 -*-
"""
Created on Mon May 29 11:02:14 2023

@author: Asha
"""

import pandas as pd
import matplotlib.pyplot as plt


df = pd.read_csv("../UnityProj/timeSeriesRopeRomeo.csv")

plt.figure(figsize=(20,20))

plt.plot(df[" cubeRomeo.position.x"], df[" cubeRomeo.position.y"])
plt.ylabel("[s] = m")
plt.xlabel("[s] = m")
plt.title("Bewegung Würfel Romeo")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeRomeo/Ortdiagramm.png', dpi=300, bbox_inches='tight')
plt.show()


plt.figure(figsize=(20,20))

plt.plot(df["alphaRomeo"], df["currentTimeStep"])
plt.ylabel("[t] = s")
plt.xlabel("[s] = alpha")
plt.title("Auslenkung als Funktion der Zeit")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeRomeo/Auslenkung.png', dpi=300, bbox_inches='tight')
plt.show()