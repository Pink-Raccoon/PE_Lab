# -*- coding: utf-8 -*-
"""
Created on Sun Feb 26 20:15:00 2023

@author: Asha
"""

import pandas as pd
import matplotlib.pyplot as plt


df = pd.read_csv("D:/ZHAW_Code/physicslab/time_series.csv")



plt.plot(df["t"], df["x(t)"], label = "Ort = s in Meter" )
#plt.ylabel("Ort = s in Meter")
plt.xlabel("Zeit = t in s")
plt.title("Ort und Geschwindigkeit als Funktion der Zeit")
#plt.show()


plt.plot(df["t"], df["v(t)"], label = "Geschwindigkeit = v in m/s")
#plt.ylabel("Geschwindigkeit = v in m/s")
plt.xlabel("Zeit = t in s")
#plt.title("Geschwindigkeit als Funktion der Zeit")
plt.legend()
plt.show()

plt.plot(df["t"], df["F(t) (added)"])
plt.ylabel("Kraft = F in N")
plt.xlabel("Zeit = t in s")
plt.title("Kraft als Funktion der Zeit")
plt.show()