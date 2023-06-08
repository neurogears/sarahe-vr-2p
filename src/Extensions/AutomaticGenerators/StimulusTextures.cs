
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
    public partial class StimulusTextures
    {
        public IObservable<StimulusTextures> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new StimulusTextures
                {
					StimulusA = StimulusA,
					StimulusB = StimulusB,
					StimulusC = StimulusC,

                };
                return Observable.Return(value);
            });
        }
    }
}
