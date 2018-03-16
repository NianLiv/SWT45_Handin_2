using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT11CookControllerUserinterface
    {
        [Test]
        public void CookingController_OutputShowCookingDoneAfter2Sec()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            var timer = new Timer();
            var output = Substitute.For<IOutput>();
            var display = new Display(output);
            var powertube = new PowerTube(output);
            var cook = new CookController(timer, display, powertube);
            var ui = new UserInterface(new Button(), new Button(), new Button(), new Door(), display, new Light(output), cook);
            (cook as CookController).UI = ui;


            cook.StartCooking(50, 2);
            pause.WaitOne(2100);
            output.Received().OutputLine($"PowerTube turned off");
        }


    }
}
