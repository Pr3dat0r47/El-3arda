import math
import sys
import pyodbc

yax = sys.argv[1]
n = 0


def average(x1):
    assert len(x1) > 0
    return float(sum(x1)) / len(x1)


def pearson_def(xa, y):
    assert len(xa) == len(y)
    n = len(xa)
    assert n > 0
    avg_x = average(xa)
    avg_y = average(y)
    diffprod = 0
    xdiff2 = 0
    ydiff2 = 0
    for idx in range(n):
        xdiff = xa[idx] - avg_x
        ydiff = y[idx] - avg_y
        diffprod += xdiff * ydiff
        xdiff2 += xdiff * xdiff
        ydiff2 += ydiff * ydiff

    return diffprod / math.sqrt(xdiff2 * ydiff2)


connection=pyodbc.connect(
    "Driver={SQL Server};"
    "Server=eaziserver.database.windows.net;"
    "Database=Fel3arda_Data;"
    "Trusted_Connection=no;"
    "uid=EZGI;"
    "pwd=Cracker2019"
)

if yax == 'Stamina':
    n = 6
elif yax == 'Sprint Speed':
    n = 5
elif yax == 'Overall':
    n = 8

xAxis = []
yAxis = []

sql_select_Query = "select * from Players_Sample"
cursor = connection.cursor()
cursor.execute(sql_select_Query)
records = cursor.fetchall()
for row in records:
    x = int((row[2]))
    xAxis.append(x)
    y = int((row[n]))
    yAxis.append(y)
cursor.close()

print(round(pearson_def(xAxis, yAxis), 5))


