using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.Write("Введите число N: ");
        if (int.TryParse(Console.ReadLine(), out int n) && n >= 1)
        {
            // Получаем количество логических процессоров
            int maxThreads = Environment.ProcessorCount;

            // Настраиваем параллельные опции
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxThreads
            };

            // Коллекция для хранения результатов
            var results = new ConcurrentDictionary<int, List<int>>();

            // Параллельно обрабатываем каждое число от 1 до N
            Parallel.For(1, n + 1, parallelOptions, i =>
            {
                List<int> factors = GetPrimeFactors(i);
                results.TryAdd(i, factors);
            });

            // Выводим результаты
            foreach (var kvp in results.OrderBy(kvp => kvp.Key))
            {
                Console.WriteLine($"Число {kvp.Key}: {string.Join(" * ", kvp.Value)}");
            }
        }
        else
        {
            Console.WriteLine("Введите корректное положительное целое число.");
        }
    }

    static List<int> GetPrimeFactors(int number)
    {
        List<int> primeFactors = new List<int>();

        // Удаляем все множители 2
        while (number % 2 == 0)
        {
            primeFactors.Add(2);
            number /= 2;
        }

        // Проверяем все нечетные числа от 3 и выше
        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            while (number % i == 0)
            {
                primeFactors.Add(i);
                number /= i;
            }
        }

        // Если number - простое число больше 2
        if (number > 2)
        {
            primeFactors.Add(number);
        }

        return primeFactors;
    }
}
