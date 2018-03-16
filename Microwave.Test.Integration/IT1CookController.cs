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
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using NUnit.Framework;
using ManualResetEvent = System.Threading.ManualResetEvent;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1CookController
    {
        private CookController _uut;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IUserInterface _userInterface;
        private IOutput _output;
        private ITimer _timer;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _userInterface = Substitute.For<IUserInterface>();
            _timer = Substitute.For<ITimer>();

            _uut = new CookController(_timer, _display, _powerTube, _userInterface);
        }

        [Test]
        public void StartCooking_OutputShowsPowerStatus_PowerTurnOn()
        {
            int power = 55;
            _uut.StartCooking(power, 30);
            _output.Received().OutputLine($"PowerTube works with {power} %");
        }

        [Test]
        public void StartCooking_InputPowerOutOfRange_PowerTubeThrowException()
        {
            Assert.That(() => _uut.StartCooking(-200, 30), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void StartCooking_PowerIsOn_PowerTubeThrowException()
        {
            _uut.StartCooking(50, 30);
            Assert.That(() => _uut.StartCooking(50, 30), Throws.TypeOf<ApplicationException>());
        }

        [Test]
        public void Stop_OutputShowsPowerOff()
        {
            _uut.StartCooking(50, 30);
            _uut.Stop();
            _output.Received().OutputLine($"PowerTube turned off");
        }

        [TestCase(0,30)]
        public void OnTimerTick_InputIs30Sec_OutputShows0030(int min, int sec)
        {
            _uut.StartCooking(50, sec);
            _timer.TimeRemaining.Returns(sec);
            _uut.OnTimerTick(_timer, EventArgs.Empty);
            _output.Received().OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }

        [Test]
        public void a()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            ITimer t = new MicrowaveOvenClasses.Boundary.Timer();
            ICookController c = new CookController(t, _display, _powerTube, _userInterface);
            c.StartCooking(50, 5);
            pause.WaitOne(2100);
            _output.Received().OutputLine($"Display shows: {0:D2}:{3:D2}");
        }

        [Test]
        public void OnTimerExpired_OutputShowsPowerOff()
        {
            _uut.StartCooking(50, 30);
            _uut.OnTimerExpired(_timer, EventArgs.Empty);
            _output.Received().OutputLine($"PowerTube turned off");
        }
    }
}
