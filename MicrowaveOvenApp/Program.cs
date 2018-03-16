using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using Timer = MicrowaveOvenClasses.Boundary.Timer;



namespace MicrowaveOvenApp
{

    

    class Program
    {
        static void Print(string s) => System.Console.WriteLine(s);

        static void Main(string[] args)
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            var btnStartCancel = new Button();
            var btnPower = new Button();
            var btnTimer = new Button();
            var door = new Door();
            var output = new Output();
            var timer = new Timer();
            var display = new Display(output);
            var light = new Light(output);
            var powerTube = new PowerTube(output);
            var cookController = new CookController(timer, display, powerTube);
            var userInterface = new UserInterface(btnPower, btnTimer, btnStartCancel, door, display, light, cookController);
            cookController.UI = userInterface;

            Print("--- Main Scenario ---");

            Print("--- User Opens the door ---");
            door.Open();
            Print("");

            Print("--- User Closes the door ---");
            door.Close();
            Print("");

            Print("--- User presses the power button 3 times ---");
            for (int i = 0; i < 3; i++)
                btnPower.Press();
            Print("");

            Print("--- User presses the time button ---");
            btnTimer.Press();
            Print("");

            Print("--- User presses the start/cancel button ---");
            btnStartCancel.Press();
            Print("");

            pause.WaitOne(60100);
            Print("");

            Print("--- User Opens the door after the dish is done ---");
            door.Open();
            Print("");

            Print("--- User Closes the door ---");
            door.Close();
            Print("");

            Print("--- Extension 3: The user presses the Start-Cancel button during cooking --- ");

            Print("--- User Opens the door ---");
            door.Open();
            Print("");

            Print("--- User Closes the door ---");
            door.Close();
            Print("");

            Print("--- User presses the power button 3 times ---");
            for (int i = 0; i < 3; i++)
                btnPower.Press();
            Print("");

            Print("--- User presses the time button ---");
            btnTimer.Press();
            Print("");

            Print("--- User presses the start/cancel button ---");
            btnStartCancel.Press();
            Print("");

            pause.WaitOne(7100);
            Print("");

            Print("--- User presses the start/cancel button after 7s ---");
            btnStartCancel.Press();
            Print("");




            // Wait while the classes, including the timer, do their job
            System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
            System.Console.ReadLine();
        }

        

    }
}
