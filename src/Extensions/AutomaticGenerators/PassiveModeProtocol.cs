
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
    public partial class PassiveModeProtocol
    {
        public IObservable<PassiveModeProtocol> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new PassiveModeProtocol
                {
					Gain = Gain,
					LocationEnd = LocationEnd,
					LocationStart = LocationStart,
					AdditionalProperties = AdditionalProperties,

                };
                return Observable.Return(value);
            });
        }
    }
}
