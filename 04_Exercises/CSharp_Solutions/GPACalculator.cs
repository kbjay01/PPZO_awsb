using System;

public class HelloWorld{
    public static void Main(string[] args){
       Console.Write("Podaj liczbe ocen: ");
       int gradesAmount = int.Parse(Console.ReadLine());
       double gradesSum = 0;

       for (int i=0; i<gradesAmount; i++){
        Console.Write("Podaj ocene: ");
        gradesSum += Convert.ToDouble(Console.ReadLine());
       }

        double GPA = Convert.ToDouble(gradesSum/gradesAmount);
        GPA = Math.Round(GPA, 2);
        Console.WriteLine("Srednia wynosi " + GPA);

        if (GPA >= 3.0){
            Console.WriteLine("Uczen zdal.");
        }
        else{
            Console.WriteLine("Uczen nie zdal.");
        }
    }
}
