using System;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.WriteLine("Таймер выключения/сна компьютера");
        Console.WriteLine("================================\n");

        // Выбор действия
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1 - Выключение компьютера");
        Console.WriteLine("2 - Спящий режим");
        Console.Write("Ваш выбор (1 или 2): ");

        string choice = Console.ReadLine();

        // Ввод времени
        Console.Write("Введите время задержки в минутах: ");
        string timeInput = Console.ReadLine();

        if (!double.TryParse(timeInput.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double delayMinutes))
        {
            Console.WriteLine("Некорректный ввод времени!");
            Console.ReadKey();
            return;
        }

        if (delayMinutes < 0)
        {
            Console.WriteLine("Время не может быть отрицательным!");
            Console.ReadKey();
            return;
        }
        
        // Расчет времени в миллисекундах
        int delayHours = (int)(delayMinutes / 60);
        int delayMs = (int)(delayMinutes * 60 * 1000);
        int delaySeconds = (int)(delayMinutes * 60);

        Console.WriteLine($"\nДействие будет выполнено через:");
        Console.WriteLine($"- {delayHours} часа");
        Console.WriteLine($"- {delayMinutes} минут(ы)");
        //Console.WriteLine($"- {delaySeconds} секунд(ы)");
        Console.WriteLine($"- {TimeSpan.FromMinutes(delayMinutes):hh\\:mm\\:ss}");
        Console.WriteLine("\nДля отмены нажмите Ctrl+C\n");

        switch (choice)
        {
            case "1":
                Console.WriteLine("Запланировано выключение компьютера");
                ShutdownComputer(delayMs, delaySeconds);
                break;
            case "2":
                Console.WriteLine("Запланирован переход в спящий режим");
                SleepComputer(delayMs);
                break;
            default:
                Console.WriteLine("Некорректный выбор!");
                Console.ReadKey();
                break;
        }
    }

    static void ShutdownComputer(int delayMs, int delaySeconds)
    {
        if (delayMs > 0)
        {
            // Альтернативный способ через shutdown с таймером
            Process.Start("shutdown", $"/s /f /t {delaySeconds}");
            Console.WriteLine($"Компьютер будет выключен через {delaySeconds} секунд");
            Console.WriteLine("Для отмены введите: shutdown /a");
        }
        else
        {
            Process.Start("shutdown", "/s /f /t 0");
        }

        Thread.Sleep(3000); 
    }

    static void SleepComputer(int delayMs)
    {
        if (delayMs > 0)
        {
            Console.WriteLine($"Ожидание {delayMs / 60000} минут...");

            //Прогресс ожидания
            DateTime startTime = DateTime.Now;
            DateTime targetTime = startTime.AddMilliseconds(delayMs);

            while (DateTime.Now < targetTime)
            {
                TimeSpan remaining = targetTime - DateTime.Now;
                Console.Write($"\rОсталось: {remaining:mm\\:ss}     ");
                Thread.Sleep(1000);
            }

            Console.WriteLine("\n");
        }

        Console.WriteLine("Переводим компьютер в спящий режим...");
        SetSuspendState(false, true, true);
    }
    //Библиотекаа лоя выкл пк . Потом на питоне попробовать сделать
    [System.Runtime.InteropServices.DllImport("powrprof.dll")]
    private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
}