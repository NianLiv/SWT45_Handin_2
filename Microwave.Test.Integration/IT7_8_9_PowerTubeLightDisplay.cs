using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT7_8_9_PowerTubeLightDisplay
    {
        private IOutput _output;
        private IPowerTube _powertube;
        private ILight _light;
        private IDisplay _display;

        [SetUp]
        public void SetUp()
        {
            _output = new Output();
            _powertube = new PowerTube(_output);
            _light = new Light(_output);
            _display = new Display(_output);
        }

        [Test]
        public void PowerTube_TurnOn_OutputShowTurnOn()
        {
            _powertube.TurnOn(50);
            //WHAT TO DO? GODNAT
            
        }
    }
}
