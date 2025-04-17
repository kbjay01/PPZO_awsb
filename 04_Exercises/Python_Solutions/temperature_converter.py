unitSelection = input("Podaj jednostke wejsciowa [C/F]: ").upper()
temperature = int(input("Podaj temperature: "))

if (unitSelection == "C"):
    temperatureInFahrenheit = (temperature*1.8)+32
    print("Podana temperatura to " + str(temperatureInFahrenheit) + " F")

elif (unitSelection == "F"):
    temperatureInCelcius = (temperature-32)/1.8
    print("Podana temperatura to " + str(temperatureInCelcius) + " C")

else:
    print("Nieprawidlowa jednostka wejsciowa")