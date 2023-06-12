
using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using YamlDotNet.Serialization;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using AutomaticGenerators;

namespace AutomaticGenerators
{
    [Combinator]
    [Description("Constructor.")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public partial class OptoStimProtocol
    {
        public IObservable<OptoStimProtocol> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new OptoStimProtocol
                {
					Delay = Delay,
					Location = Location,
					WaveformIndex = WaveformIndex,

                };
                return Observable.Return(value);
            });
        }
    }
}