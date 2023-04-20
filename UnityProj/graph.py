# -*- coding: utf-8 -*-
"""
Created on Sun Feb 26 20:15:00 2023

@author: Asha
"""

import pandas as pd
import matplotlib.pyplot as plt



df = pd.read_csv("D:/ZHAW_Code/PE_Lab/UnityProj/TimeSeries/time_seriesElastic.csv")

# plt.plot(df["currentTimeStep"], df[" cubeRomeo.position.x"])
# plt.ylabel("v")
# plt.xlabel("t")
# plt.xlim(0,15)
# plt.title("Ort als Funktion der Zeit")
# #plt.legend()
# plt.show()

# plt.plot(df["currentTimeStep"], df[" cubeRomeo.velocity.x"])
# plt.ylabel("v")
# plt.xlabel("t")
# plt.title("Geschwindigkeit als Funktion der Zeit")
# plt.xlim(0,15)
# plt.show()


# plt.plot(df["currentTimeStep"], df[" cubeRomeoKinetic"])
# plt.ylabel("J")
# plt.xlabel("t")
# plt.title("Kinetische Energie als Funktion der Zeit")
# plt.xlim(0,15)
# plt.show()

# plt.plot(df["currentTimeStep"], df[" springPotentialEnergy"])
# plt.ylabel("J")
# plt.xlabel("t")
# plt.title("Potentielle Energie als Funktion der Zeit")
# plt.xlim(0,15)
# plt.show()


