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

plt.plot(df["currentTimeStep"],df[" cubeRomeo.position.x"])
plt.ylabel("[s] = m")
plt.xlabel("[t] = s")
plt.title("Bewegung Würfel Romeo x")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeRomeo/x(t).png', dpi=300, bbox_inches='tight')
plt.show()

plt.plot(df["currentTimeStep"],df[" cubeRomeo.position.y"])
plt.ylabel("[s] = m")
plt.xlabel("[t] = s")
plt.title("Bewegung Würfel Romeo y")
plt.savefig('../Semesterprojekt Physik Engines/images/ropeRomeo/y(t).png', dpi=300, bbox_inches='tight')
plt.show()

plt.figure()

plt.plot(df["currentTimeStep"],df["alphaRomeo"])
plt.ylabel("[s] = alpha")
plt.xlabel("[t] = s")
plt.title("Auslenkung als Funktion der Zeit")
plt.ylim(25,54)
# plt.xticks([1, 2, 3, 4, 5,6,7,8,9,10,11,12,13])
plt.savefig('../Semesterprojekt Physik Engines/images/ropeRomeo/AuslenkungRad.png', dpi=300, bbox_inches='tight')
plt.show()

plt.plot(df["currentTimeStep"],df["degree"])
plt.ylabel("[s] = alpha")
plt.xlabel("[t] = s")
plt.title("Auslenkung in Grad als Funktion der Zeit")
plt.ylim(25,54)
# plt.xticks([0,1, 2, 3, 4, 5,6,7,8,9,10,11,12,13])
plt.savefig('../Semesterprojekt Physik Engines/images/ropeRomeo/AuslenkungDeg.png', dpi=300, bbox_inches='tight')
plt.show()