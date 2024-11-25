using System;
using System.IO.Ports;

class Program
{
    static void Main(string[] args)
    {
        // Configurez votre port série
        string portName = "COM5"; // Remplacez par le port utilisé par votre Arduino
        int baudRate = 9600; // Doit correspondre au baud rate configuré sur l'Arduino

        // Instanciez l'objet SerialPort
        using (SerialPort serialPort = new SerialPort(portName, baudRate))
        {
            try
            {
                // Ouvrir le port série
                serialPort.Open();

                Console.WriteLine("Entrez un entier à envoyer à l'Arduino :");
                int valueToSend = int.Parse(Console.ReadLine());

                // Envoyer la donnée
                serialPort.WriteLine(valueToSend.ToString());
                Console.WriteLine($"Valeur envoyée : {valueToSend}");

                // Lire la réponse de l'Arduino
                string response = serialPort.ReadLine();
                Console.WriteLine($"Réponse de l'Arduino : {response}");

                // Fermer le port série
                serialPort.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }
}
