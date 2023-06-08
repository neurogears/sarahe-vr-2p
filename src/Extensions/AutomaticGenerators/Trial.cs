
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
    public partial class Trial
    {
        public IObservable<Trial> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new Trial
                {
					CorridorType = CorridorType,
					PassiveMode = PassiveMode,
					StimulusTextures = StimulusTextures,
					Reward = Reward,
					LedStimulation = LedStimulation,
					Teleport = Teleport,

                };
                return Observable.Return(value);
            });
        }
    }
}
