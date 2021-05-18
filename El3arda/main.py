import pyodbc
import sys
nat=sys.argv[1]
DesiredSample =sys.argv[2]
try:
    connection=pyodbc.connect(
        "Driver={SQL Server};"
        "Server=eaziserver.database.windows.net;"
        "Database=Fel3arda_Data;"
        "Trusted_Connection=no;"
        "uid=EZGI;"
        "pwd=Cracker2019"
    )
    cursor=connection.cursor()
    deletionQuery="delete from Players_Sample"
    cursor.execute(deletionQuery)
    cursor.commit()
    cursor.close()

    samplingQuery1 ="insert into Players_Sample select * from Players_DataSet where Nationality='"+nat+"'"
    cursor=connection.cursor()
    cursor.execute(samplingQuery1)
    cursor.commit()
    cursor.close()

    samplingQuery2="select count(*) from Players_Sample"
    cursor=connection.cursor()
    cursor.execute(samplingQuery2)
    WholeSample= cursor.fetchone()
    WholeSampleAslist = [x for x in WholeSample]
    cursor.commit()
    cursor.close()

    NtoDelete=WholeSampleAslist[0]-int(DesiredSample)
    samplingQuery3 ="delete top("+str(NtoDelete)+") from Players_Sample"
    cursor=connection.cursor()
    cursor.execute(samplingQuery3)
    cursor.commit()
    cursor.close()
    connection.close()




