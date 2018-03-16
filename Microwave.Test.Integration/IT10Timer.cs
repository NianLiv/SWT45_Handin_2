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
    class IT10Timer
    {
        private ITimer Timer;
        private ICookController CookController;
        private IOutput _output;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IUserInterface _userInterface;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _userInterface = Substitute.For<IUserInterface>();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            Timer = new Timer();
            CookController = new CookController(Timer, _display, _powerTube, _userInterface);
        }

        [Test]
        public void StartCooking_SetTimeRemaingCorrect()
        {
            int time = 5;
            CookController.StartCooking(50, time);
            Assert.That(Timer.TimeRemaining, Is.EqualTo(time));
        }

        [Test]
        public void StartCooking_TimerExpired()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            CookController.StartCooking(50, 2);
            pause.WaitOne(2100);
            _userInterface.Received().CookingIsDone();
        }

        [Test]
        public void CookController_OnTimerTickCalled2Times_Expected3SecLeft()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            CookController.StartCooking(50, 5);
            pause.WaitOne(2000);
            _output.Received().OutputLine($"Display shows: {0:D2}:{3:D2}");
        }

 


    }
}
