using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using YamlDotNet.Serialization;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using AutomaticGenerators;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class Serialize
{
    public IObservable<string> Process(IObservable<object> source)
    {
        return source.Select(value =>
        {
            string serializedObject;
            var serializer = new SerializerBuilder()
                .Build();
            serializedObject =  serializer.Serialize(value);
            return serializedObject;
        });
    }
}


