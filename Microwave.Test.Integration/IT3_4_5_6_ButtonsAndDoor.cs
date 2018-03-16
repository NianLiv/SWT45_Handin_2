using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT3_4_5_6_ButtonsAndDoor
    {
        private Button StartCancel, Power, Time;
        private Door Door;
        private ICookController _cookController;
        private IUserInterface _userInterface;
        private IOutput _output;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private ILight _light;
        private ITimer _timer;

        [SetUp]
        public void SetUp()
        {
            StartCancel = new Button();
            Power = new Button();
            Time = new Button();
            Door = new Door();
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _light = new Light(_output);
            _timer = new Timer();
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(Power, Time, StartCancel, Door, _display, _light, _cookController);
            (_cookController as CookController).UI = _userInterface;
        }

        [Test]
        public void PowerPress_OutputShowsPower50()
        {
            Power.Press();
            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void TimePress_OutputShowsTime()
        {
            Power.Press();
            Time.Press();
            _output.Received().OutputLine("Display shows: 01:00");
        }

        [Test]
        public void StartCancel_StateSetTime_OutputShowsPowerTubeStart()
        {
            Power.Press();
            Time.Press();
            StartCancel.Press();
            _output.Received().OutputLine("PowerTube works with 50 %");
        }

        [Test]
        public void StartCancel_StartCooking_OutputShowsPowerTubeOff()
        {
            Power.Press();
            Time.Press();
            StartCancel.Press();
            StartCancel.Press();
            _output.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void DoorOpen_StateReady_OutputShowsLightOn()
        {
            Door.Open();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void DoorClose_StateDoorOpen_OutputShowsLightOff()
        {
            Door.Open();
            Door.Close();
            _output.Received().OutputLine("Light is turned off");
        }
    }
}
