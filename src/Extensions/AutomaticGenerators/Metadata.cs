
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
    public partial class Metadata
    {
        public IObservable<Metadata> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new Metadata
                {
					AnimalId = AnimalId,
					DefaultEncoderGain = DefaultEncoderGain,
					RandomSeed = RandomSeed,
					RewardCalibration = RewardCalibration,
					RootPath = RootPath,
					TrialOrder = TrialOrder,
					AdditionalProperties = AdditionalProperties,

                };
                return Observable.Return(value);
            });
        }
    }
}
