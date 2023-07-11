
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
    public partial class StimControl
    {
        public IObservable<StimControl> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new StimControl
                {
					Delay = Delay,
					LeftInitStimState = LeftInitStimState,
					Location = Location,
					RightInitStimState = RightInitStimState,
					StimTextureLeft = StimTextureLeft,
					StimTextureRight = StimTextureRight,
					AdditionalProperties = AdditionalProperties,

                };
                return Observable.Return(value);
            });
        }
    }
}
