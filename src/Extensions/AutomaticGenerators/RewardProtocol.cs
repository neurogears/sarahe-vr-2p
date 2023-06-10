
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
    public partial class RewardProtocol
    {
        public IObservable<RewardProtocol> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new RewardProtocol
                {
					AmountHigh = AmountHigh,
					AmountLow = AmountLow,
					Delay = Delay,
					Location = Location,
					ProbabilityHigh = ProbabilityHigh,

                };
                return Observable.Return(value);
            });
        }
    }
}
