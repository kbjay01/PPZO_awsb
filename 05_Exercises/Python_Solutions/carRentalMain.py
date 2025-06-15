class Car:
    # Klasa definiujaca samochod - ID, marka, model, typ, cena za dobe
    def __init__(self, car_id, brand, model, car_type, price_per_day):
        self.car_id = car_id
        self.brand = brand
        self.model = model
        self.car_type = car_type
        self.price_per_day = price_per_day
        self.available = True

    def __str__(self):
        # Zwraca informacje o samochodzie razem ze statusem wypozyczenia
        status = "DOSTEPNY" if self.available else "NIEDOSTEPNY"
        return f"{self.car_id}: {self.brand} {self.model} ({self.car_type}) - {self.price_per_day} zl/dzien [{status}]"


class Customer:
    # Klasa definiujaca klienta - ID, imie (nazwa/imie i nazwisko)
    def __init__(self, customer_id, name):
        self.customer_id = customer_id
        self.name = name

    def __str__(self):
        # Zwraca dane klienta
        return f"{self.customer_id}: {self.name}"


class Rental:
    # Klasa definiujaca wynajem - kto wynajal, jaki samochod i na ile dni
    def __init__(self, customer, car, days):
        self.customer = customer
        self.car = car
        self.days = days
        self.is_active = True

    def calculateCost(self):
        # Zwraca koszt wynajmu na n dni
        return self.days * self.car.price_per_day

    def __str__(self):
        # Zwraca informacje o najmie
        status = "AKTYWNA" if self.is_active else "ZAKONCZONA"
        return (
                f"[{status}]\n"
                f"Klient: {self.customer.name} (ID: {self.customer.customer_id})\n"
                f"Samochod: {self.car.brand} {self.car.model} ({self.car.car_type})\n"
                f"Liczba dni: {self.days}\n"
                f"Koszt: {self.calculateCost()} zl")


def saveRentalInfoToFile(rental):
    # Zapisuje dane wynajmow do pliku
    with open("rentals.txt", "a", encoding="utf-8") as file:
        file.write(f"{rental.customer.customer_id};{rental.car.car_id};{rental.days}\n")
        return

def saveCustomerNamesToFile(customer):
    # Zapisuje dane o klientach do pliku
    with open("clients.txt", "a", encoding="utf-8") as file:
        file.write(f"{customer.customer_id};{customer.name}\n")
        return


def importCustomerNamesFromFile():
    # Importuje dane o klientach z pliku clients.txt
    try:
        with open("clients.txt", "r", encoding="utf-8") as file:
            for line in file:
                parts = line.strip().split(";")
                if len(parts) == 2:
                    customer = Customer(int(parts[0 ]), parts[1])
                    customers.append(customer)
            print("Zaimportowano klientów.")
            return
    except FileNotFoundError:
        print("Plik clients.txt nie istnieje.")
        return


def importRentalsFromFile():
    # Importuje dane o wynajmach z pliku rentals.txt
    try:
        with open("rentals.txt", "r", encoding="utf-8") as file:
            for line in file:
                parts = line.strip().split(";")
                if len(parts) == 3:
                    cust_id = int(parts[0])
                    car_id = int(parts[1])
                    days = int(parts[2])
                    customer = next((c for c in customers if c.customer_id == cust_id), None)
                    car = next((c for c in cars if c.car_id == car_id), None)
                    if customer and car:
                        rental = Rental(customer, car, days)
                        rentals.append(rental)
                        car.available = False
            print("Zaimportowano wypozyczenia.")
            return
    except FileNotFoundError:
        print("Plik rentals.txt nie istnieje.")
        return


def showRentals():
    # Wyświetla wszystkie wypozyczenia
    if not rentals:
        print("Brak rezerwacji.")
        return

    print("\n1. Pokaz tylko aktywne\n2. Pokaz wszystkie")
    choice = input("Wybierz opcje: ")

    if choice == "1":
        to_show = [r for r in rentals if r.is_active]
    else:
        to_show = rentals

    if not to_show:
        print("[i] Brak wypozyczen.")
        return

    for r in to_show:
        print("\n" + str(r))

    return
def returnCar():
    # Funkcja obslugujaca zwrot samochodu
    active_rentals = [r for r in rentals if r.is_active]
    if not active_rentals:
        print("[i] Brak aktywnych wypozyczen.")
        return

    for i, r in enumerate(active_rentals):
        print(f"{i + 1}. {r.car.brand} {r.car.model} - {r.customer.name}")

    try:
        choice = int(input("Wybierz numer samochodu do zwrotu: ")) - 1
        rental = active_rentals[choice]
        rental.car.available = True
        rental.is_active = False
        print(f"[i] Samochod {rental.car.brand} {rental.car.model} zwrocony.")
    except (ValueError, IndexError):
        print("[!] Niepoprawny wybor.")
        return

def showAllCars():
    # Funkcja wyswietlajaca wszystkie samochody
    print("\nSamochody:")
    for car in cars:
        print(car)
    return

def rentCar():
    # Funkcja obslugujaca wypozyczenie samochodu
    if not customers:
        print("[!] Brak klientow w bazie. Najpierw dodaj klienta lub zaimportuj klientow z pliku.")
        return

    print("\nWybierz klienta:")
    for customer in customers:
        print(customer)
    cust_id = int(input("Podaj ID klienta: "))
    customer = next((c for c in customers if c.customer_id == cust_id), None)

    if not customer:
        print("[!] Nie znaleziono klienta.")
        return

    print("\nWybierz dostępny samochod:")
    available_cars = [c for c in cars if c.available]
    if not available_cars:
        print("[i] Brak dostepnych samochodow.")
        return

    for car in available_cars:
        print(car)
    car_id = int(input("Podaj ID samochodu: "))
    car = next((c for c in available_cars if c.car_id == car_id), None)

    if not car:
        print("[!] Niepoprawny wybor samochodu.")
        return

    days = int(input("Na ile dni wynajem?: "))
    rental = Rental(customer, car, days)
    rentals.append(rental)
    car.available = False
    saveRentalInfoToFile(rental)
    print("\n[i] Rezerwacja utworzona:")
    print(rental)
    return

def addClient():
    # Funckja sluzaca do dodania klienta do bazy klientow
    name = input("Podaj imie i nazwisko klienta: ")
    customer_id = len(customers) + 1
    customer = Customer(customer_id, name)
    customers.append(customer)
    saveCustomerNamesToFile(customer)
    print(f"[i] Dodano klienta: {customer}")
    return


# Przykladowe samochody
cars = [
    Car(1, "BMW", "530e", "Premium", 250),
    Car(2, "Dacia", "Jogger", "Standard", 130),
    Car(3, "Fiat", "500", "Economy", 80),
]

customers = []
rentals = []

def main():
    print("=== Wypozyczalnia samochodow ===")
    while True:
        print("\n1. Pokaz dostepne samochody")
        print("2. Dodaj klienta")
        print("3. Zarezerwuj samochod")
        print("4. Pokaz wszystkie rezerwacje")
        print("5. Zwroc samochod")
        print("6. Importuj klientow z pliku")
        print("7. Importuj wypozyczenia z pliku")
        print("0. Wyjdz")
        choice = input("Wybierz opcje: ")

        if choice == "1":
           showAllCars()

        elif choice == "2":
            addClient()

        elif choice == "3":
            rentCar()

        elif choice == "4":
            showRentals()

        elif choice == "5":
            # showRentals()
            returnCar()

        elif choice == "6":
            importCustomerNamesFromFile()

        elif choice == "7":
            importRentalsFromFile()

        elif choice == "0":
            print("[i] Program konczy prace.")
            break

        else:
            print("[!] Niepoprawna opcja.")

if __name__ == "__main__":
    main()
