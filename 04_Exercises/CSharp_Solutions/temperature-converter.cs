using System;

public class HelloWorld{
    public static void Main(string[] args){
        Console.Write("Podaj jednostke wejsciowa [C/F]: ");
        string unit = Console.ReadLine().ToUpper();

        Console.Write("Podaj temperature: ");
        int temperature = int.Parse(Console.ReadLine());

        if (unit == "C"){
            Console.WriteLine("Temperatura po konwersji wynosi " + Convert.ToDouble((temperature*1.8)+32) + " F");
        }

        else if (unit == "F"){
            Console.WriteLine("Temperatura po konwersji wynosi: " + Convert.ToDouble((temperature-32)/1.8) + " C");
        }

        else{
            Console.WriteLine("[!] Nieprawidlowa jednostka wejsciowa");
        }
    }
}