namespace CarRentalApp{
    class Car{
        public int CarId {get; set;}
        public string Brand {get; set;}
        public string Model {get; set;}
        public string CarType {get; set;}
        public decimal PricePerDay {get; set;}
        public bool Available {get; set;} = true;

        public Car(int carId, string brand, string model, string carType, decimal pricePerDay){
            // Definicja samochodu - ID, marka, model, rodzaj, cena za dobe
            CarId = carId;
            Brand = brand;
            Model = model;
            CarType = carType;
            PricePerDay = pricePerDay;
        }

        public override string ToString(){
            // Zwraca dane o samochodzie razem ze statusem wypozyczenia
            string status = Available ? "DOSTEPNY" : "NIEDOSTEPNY";
            return $"{CarId}: {Brand} {Model} ({CarType}) - {PricePerDay} zl/dzien [{status}]";
        }
    }

    class Customer{
        public int CustomerId {get; set;}
        public string Name {get; set;}

        public Customer(int id, string name){
            CustomerId = id;
            Name = name;
        }

        public override string ToString(){
            // Zwraca dane o kliencie
            return $"{CustomerId}: {Name}";
        }
    }

    class Rental{
        public Customer Customer {get; set;}
        public Car Car {get; set;}
        public int Days {get; set;}
        public bool IsActive {get; set;} = true;

        public Rental(Customer customer, Car car, int days){
            Customer = customer;
            Car = car;
            Days = days;
        }

        public decimal calculateCost(){
            // Zwraca cene wypozyczenia za n dni
            return Days * Car.PricePerDay;
        }

        public override string ToString(){
            // Zwraca dane o wynajmie - czy jest aktywny czy zakonczony, kto wypozyczyl samochod, jaki i na ile dni
            string status = IsActive ? "AKTYWNA" : "ZAKONCZONA";
            return $"[{status}]\nKlient: {Customer.Name} (ID: {Customer.CustomerId})\n" +
                   $"Samochod: {Car.Brand} {Car.Model} ({Car.CarType})\n" +
                   $"Liczba dni: {Days}\nKoszt: {calculateCost()} zl";
        }
    }


    class Program{
        static List<Car> cars = new List<Car>(){
            // Trzy przykladowe samochody z kazdej grupy: Premium, Standard, Economy
            new Car(1, "BMW", "540i", "Premium", 250),
            new Car(2, "Ford", "Mondeo", "Standard", 150),
            new Car(3, "Skoda", "Citygo", "Economy", 80)
        };

        static List<Customer> customers = new List<Customer>();
        static List<Rental> rentals = new List<Rental>();

        static void Main(string[] args){
            while (true){
                // Menu wypozyczalni
                Console.WriteLine("\n=== Wypozyczalnia samochodow ===");
                Console.WriteLine("1. Pokaz dostepne samochody");
                Console.WriteLine("2. Dodaj klienta");
                Console.WriteLine("3. Zarezerwuj samochod");
                Console.WriteLine("4. Pokaz wszystkie rezerwacje");
                Console.WriteLine("5. Zwroc samochod");
                Console.WriteLine("6. Importuj klientow z pliku");
                Console.WriteLine("7. Importuj wypozyczenia z pliku");
                Console.WriteLine("0. Wyjdz");
                Console.Write("Wybierz opcje: ");

                string choice = Console.ReadLine();

                switch (choice){
                    case "1":
                        // Wyswietla dostepne samochody
                        showAvailableCars();
                        break;
                    case "2":
                        // Dodaje klienta
                        addClient();
                        break;
                    case "3":
                        // Realizuje wypozyczenie samochodu
                        makeRental();
                        break;
                    case "4":
                        // Wyswietla wypozyczenia
                        showRentals();
                        break;
                    case "5":
                        // Umozliwia zwrot wypozyczonego samochodu
                        returnCar();
                        break;
                    case "6":
                        // importuje klientow z  pliku
                        importCustomersFromFile();
                        break;
                    case "7":
                        // importuje wypozyczenia z pliku
                        importRentalsFromFile();
                        break;
                    case "0":
                        // Zamyka program
                        return;
                    default:
                        Console.WriteLine("[!] Niepoprawna opcja.");
                        break;
                }
            }
        }

        static void showAvailableCars(){
            // Wyswietla dostepne samochody
            Console.WriteLine("\nSamochody:");
            foreach (var car in cars)
                Console.WriteLine(car);
        }

        static void addClient(){
            // Funkcja do dodawania nowego klienta do bazy klientow
            Console.Write("Podaj imie i nazwisko klienta: ");
            string name = Console.ReadLine();
            int id = customers.Count + 1;
            var customer = new Customer(id, name);
            customers.Add(customer);

            File.AppendAllText("clients.txt", $"{id};{name}\n");
            Console.WriteLine($"[i] Dodano klienta: {customer}");
        }

        static void makeRental(){
            // Funkcja do realizacji wypozyczenia razem z obsluga bledow 
            if (customers.Count == 0){
                Console.WriteLine("[i] Najpierw dodaj klienta lub wczytaj klientow z pliku.");
                return;
            }

            Console.WriteLine("\nDostepni klienci:");
            foreach (var cust in customers)
                Console.WriteLine(cust);

            Console.Write("Podaj ID klienta: ");
            if (!int.TryParse(Console.ReadLine(), out int custId)){
                Console.WriteLine("[!] Nieprawidłowy ID.");
                return;
            }

            var customer = customers.FirstOrDefault(c => c.CustomerId == custId);
            if (customer == null){
                Console.WriteLine("[!] Nie znaleziono klienta.");
                return;
            }

            var availableCars = cars.Where(c => c.Available).ToList();
            if (availableCars.Count == 0){
                Console.WriteLine("[i] Brak dostepnych samochodow.");
                return;
            }

            Console.WriteLine("\nDostepne samochody:");
            foreach (var car in availableCars)
                Console.WriteLine(car);

            Console.Write("Podaj ID samochodu: ");
            if (!int.TryParse(Console.ReadLine(), out int carId)){
                Console.WriteLine("[!] Nieprawidlowy ID.");
                return;
            }

            var carToRent = availableCars.FirstOrDefault(c => c.CarId == carId);
            if (carToRent == null){
                Console.WriteLine("Nie znaleziono samochodu.");
                return;
            }

            Console.Write("Na ile dni chcesz wynajac samochod?: ");
            if (!int.TryParse(Console.ReadLine(), out int days)){
                Console.WriteLine("[!] Nieprawidłowa liczba dni.");
                return;
            }

            var rental = new Rental(customer, carToRent, days);
            rentals.Add(rental);
            carToRent.Available = false;

            File.AppendAllText("rentals.txt", $"{customer.CustomerId};{carToRent.CarId};{days}\n");
            Console.WriteLine("\n[i] Rezerwacja zakonczona:");
            Console.WriteLine(rental);
        }

        static void showRentals(){
            // Funckja wyswietlajaca wszystkie wypozyczenia razem z obsluga braku rezerwacji
            if (rentals.Count == 0){
                Console.WriteLine("[i] Brak rezerwacji.");
                return;
            }

            Console.WriteLine("\n1. Pokaz tylko aktywne\n2. Pokaz wszystkie");
            Console.Write("Wybierz opcje: ");
            string choice = Console.ReadLine();

            IEnumerable<Rental> listToShow = rentals;

            if (choice == "1")
                listToShow = rentals.Where(r => r.IsActive);

            foreach (var rental in listToShow)
                Console.WriteLine("\n" + rental);
        }


        static void returnCar(){
            // Funkcja umozliwiajaca zwrot wypozyczonego samochodu
            var activeRentals = rentals.Where(r => r.IsActive).ToList();
            if (activeRentals.Count == 0){
                Console.WriteLine("[i] Brak aktywnych wypozyczen.");
                return;
            }

            for (int i = 0; i < activeRentals.Count; i++){
                var r = activeRentals[i];
                Console.WriteLine($"{i + 1}. {r.Car.Brand} {r.Car.Model} - {r.Customer.Name}");
            }

            Console.Write("Wybierz numer samochodu do zwrotu: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > activeRentals.Count){
                Console.WriteLine("[!] Niepoprawny wybor.");
                return;
            }

            var selectedRental = activeRentals[idx - 1];
            selectedRental.Car.Available = true;
            selectedRental.IsActive = false;
            Console.WriteLine($"[i] Samochod {selectedRental.Car.Brand} {selectedRental.Car.Model} zwrocony.");
        }


        static void importCustomersFromFile(){
            // funkcja importujaca dane o klientach z pliku clients.txt
            if (!File.Exists("clients.txt")){
                Console.WriteLine("[!] Plik clients.txt nie istnieje.");
                return;
            }

            foreach (var line in File.ReadAllLines("clients.txt")){
                var parts = line.Split(';');
                if (parts.Length == 2){
                    int id = int.Parse(parts[0]);
                    string name = parts[1];
                    customers.Add(new Customer(id, name));
                }
            }

            Console.WriteLine("[i] Zaimportowano klientow.");
        }

        static void importRentalsFromFile(){
            // Funkcja importujaca dane o wypozyczeniach z pliku rentals.txt
            if (!File.Exists("rentals.txt")){
                Console.WriteLine("[!] Plik rentals.txt nie istnieje.");
                return;
            }

            foreach (var line in File.ReadAllLines("rentals.txt")){
                var parts = line.Split(';');
                if (parts.Length == 3){
                    int custId = int.Parse(parts[0]);
                    int carId = int.Parse(parts[1]);
                    int days = int.Parse(parts[2]);

                    var customer = customers.FirstOrDefault(c => c.CustomerId == custId);
                    var car = cars.FirstOrDefault(c => c.CarId == carId);

                    if (customer != null && car != null)
                    {
                        rentals.Add(new Rental(customer, car, days));
                        car.Available = false;
                    }
                }
            }

            Console.WriteLine("[i] Zaimportowano wypozyczenia.");
        }
    }
}
