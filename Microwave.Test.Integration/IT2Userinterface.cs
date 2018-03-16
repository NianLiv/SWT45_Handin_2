using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT2Userinterface
    {
        private UserInterface _uut;
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private IOutput _output;
        private IPowerTube _powerTube;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(new Timer(), _display, _powerTube);

            _uut = new UserInterface(new Button(), new Button(), new Button(), new Door(), _display, _light, _cookController);
        }

        [Test]
        public void OnDoorOpened_StateReady_OutputShowsLightOn()
        {
            _uut.OnDoorOpened(new Door(), EventArgs.Empty);
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnDoorOpened_StateCooking_OutputShowsPowerTubeOff()
        {
            _uut.OnPowerPressed(new Button(), EventArgs.Empty); 
            _uut.OnTimePressed(new Button(), EventArgs.Empty);
            _uut.OnStartCancelPressed(new Button(), EventArgs.Empty);
            _uut.OnDoorOpened(new Door(), EventArgs.Empty);

            _output.Received().OutputLine($"PowerTube turned off");
        }

        [Test]
        public void OnDoorOpened_StateCooking_OutputShowsDisplayClear()
        {
            _uut.OnPowerPressed(new Button(), EventArgs.Empty);
            _uut.OnTimePressed(new Button(), EventArgs.Empty);
            _uut.OnStartCancelPressed(new Button(), EventArgs.Empty);
            _uut.OnDoorOpened(new Door(), EventArgs.Empty);

            _output.Received().OutputLine($"Display cleared");
        }

        [Test]
        public void OnDoorClosed_StateDoorIsOpen_OutputShowsLightOff()
        {
            _uut.OnDoorOpened(new Door(), EventArgs.Empty);
            _uut.OnDoorClosed(new Door(), EventArgs.Empty);
            _output.Received().OutputLine("Light is turned off");
        }

        [TestCase(1, 50)] //State READY
        [TestCase(14, 700)] //State SETPOWER
        [TestCase(15, 50)] //State SETPOWER
        public void OnPowerPressed_StateReady_and_StateSetPower_OutputShowsPower(int pressCount, int expectedPower)
        {
            for (int i = 0; i < pressCount; i++)
            {
                _uut.OnPowerPressed(new Button(), EventArgs.Empty);
            }

            _output.Received().OutputLine($"Display shows: {expectedPower} W");
        }

        [TestCase(1)] //State SETPOWER
        [TestCase(1000)] //State SETTIME
        public void OnTimedPressed_StateSetPower_and_StateSetTime_OutputShowsTime(int min)
        {
            _uut.OnPowerPressed(new Button(), EventArgs.Empty);
            for (int i = 0; i < min; i++)
            {
                _uut.OnTimePressed(new Button(), EventArgs.Empty);
            }
            _output.Received().OutputLine($"Display shows: {min:D2}:{0:D2}");
        }

        [Test]
        public void OnStartCancelPressed_OutputShowsPowerTubeOn()
        {
            _uut.OnPowerPressed(new Button(), EventArgs.Empty);
            _uut.OnTimePressed(new Button(), EventArgs.Empty);
            _uut.OnStartCancelPressed(new Button(), EventArgs.Empty);
            _output.Received().OutputLine("PowerTube works with 50 %");
        }

        [Test]
        public void OnStartCancelPressed_OutputShowsPowerTubeOff()
        {
            _uut.OnPowerPressed(new Button(), EventArgs.Empty);
            _uut.OnTimePressed(new Button(), EventArgs.Empty);
            _uut.OnStartCancelPressed(new Button(), EventArgs.Empty);
            _uut.OnStartCancelPressed(new Button(), EventArgs.Empty);
            _output.Received().OutputLine("PowerTube turned off");
        }      

    }
}
