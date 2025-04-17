print("=== KALKULATOR ===")
print("1. +")
print("2. -")
print("3. *")
print("4. /")

operationType = str(input("Wybierz typ operacji: "))
print("Podaj dane wejsciowe")
firstNumber = float(input("a = "))
secondNumber = float(input("b = "))

if (operationType == "1" or operationType == "+"):
    print("Suma a i b wynosi " + str(firstNumber+secondNumber))
elif (operationType == "2" or operationType == "-"):
    print("Roznica a i b wynosi " + str(firstNumber-secondNumber))
elif (operationType == "3" or operationType == "*"):
    print("Iloczyn a i b wynosi " + str(firstNumber*secondNumber))
elif (operationType == "4" or operationType == "/"):
    if (secondNumber == 0):
        print("[!] Nie mozna dzielic przez zero")
        exit()
    print("Iloraz a i b wynosi " + str(firstNumber/secondNumber))

else:
    print("Nieznany typ operacji")
