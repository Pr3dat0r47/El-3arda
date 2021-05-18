import numpy as np
import sys
import scipy.stats as ss
import statistics
import pyodbc

yax = sys.argv[1]

if yax == 'Stamina':
    n = 6
elif yax == 'Sprint Speed':
    n = 5
elif yax == 'Overall':
    n = 8

xAxis = []
yAxis = []
connection=pyodbc.connect(
    "Driver={SQL Server};"
    "Server=eaziserver.database.windows.net;"
    "Database=Fel3arda_Data;"
    "Trusted_Connection=no;"
    "uid=EZGI;"
    "pwd=Cracker2019"
    )
sql_select_Query = "select * from Players_Sample"
cursor = connection.cursor()
cursor.execute(sql_select_Query)
records = cursor.fetchall()
for row in records:
    # age is taken here
    x = int((row[2]))
    xAxis.append(x)
    # here we re taking the desired row to be compared to age
    y = int((row[n]))
    yAxis.append(y)
cursor.close()

print(statistics.mean(xAxis))
print(statistics.mean(yAxis))
standered_deviation_ages=statistics.stdev(xAxis)
print(standered_deviation_ages)
standered_deviation_n=statistics.stdev(yAxis)
print(standered_deviation_n)
varianceOfAges =statistics.variance(xAxis,(statistics.mean(xAxis)))
print(varianceOfAges)
varianceOfn = statistics.variance(yAxis,statistics.mean(yAxis))
print(varianceOfn)
# here we are taking mean sample and this function will print a bracket containing 2 values that the population mean is between
print(ss.norm.interval(0.95, loc=statistics.mean(xAxis), scale=statistics.stdev(xAxis)))
q75, q25 = np.percentile(xAxis, [75, 25])
iqr = q75 - q25
print(iqr)
q75, q25 = np.percentile(yAxis, [75, 25])
iqr = q75 - q25
print(iqr)
