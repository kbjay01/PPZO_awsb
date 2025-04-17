amountOfGrades = int(input("Podaj liczbe ocen: "))
gradesSum=1

for i in range(amountOfGrades):
    gradesSum += int(input("Podaj ocene: "))

gradesAverage = round(float(gradesSum/amountOfGrades),2)
print("Srednia wynosi: " + str(gradesAverage))
if gradesAverage >= 3.0:
    print("Uczen zdal.")
else:
    print("Uczen nie zdal.")