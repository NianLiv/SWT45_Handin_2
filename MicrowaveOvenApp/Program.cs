using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
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



            for (int i = 0; i < 19; i++)
                btnPower.Press();
            btnTimer.Press();
            btnStartCancel.Press();

            pause.WaitOne(5000);

            btnStartCancel.Press();



            // Wait while the classes, including the timer, do their job
            System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
            System.Console.ReadLine();
        }
    }
}
