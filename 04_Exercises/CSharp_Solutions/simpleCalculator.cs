using System;

public class HelloWorld{
    public static void Main(string[] args){
        Console.WriteLine("=== KALKULATOR ===");
        Console.WriteLine("1. +");
        Console.WriteLine("2. -");
        Console.WriteLine("3. *");
        Console.WriteLine("4. /");
        Console.Write("Wybierz typ operacji: ");
        string operationType = Console.ReadLine();

        Console.WriteLine("Podaj dane wejsciowe");
        Console.Write("a = ");
        double firstNumber = Convert.ToDouble(Console.ReadLine());

        Console.Write("b = ");
        double secondNumber = Convert.ToDouble(Console.ReadLine());

        if (operationType == "+" || operationType == "1"){
            Console.WriteLine("Suma liczb a i b wynosi " + (firstNumber+secondNumber));
        }

        else if (operationType == "-" || operationType == "2"){
            Console.WriteLine("Roznica liczb a i b wynosi " + (firstNumber-secondNumber));
        }

        else if (operationType == "*" || operationType == "3"){
            Console.WriteLine("Iloczyn liczb a i b wynosi " + (firstNumber*secondNumber));
        }

        else if (operationType == "/" || operationType == "4"){
            if (secondNumber == 0){
                Console.WriteLine("[!] Nie mozna dzielic przez zero.");
                return;
            }
            Console.WriteLine("Iloraz liczb a i b wynosi " + (firstNumber/secondNumber));
        }
        
        else{
            Console.WriteLine("[!] Nieznany typ dzialania.");
        }
    }
}