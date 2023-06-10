using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using AutomaticGenerators;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class BuildTrialsFromBlock
{

    private Random random = new Random();
    public Random Random
    {
        get { return random; }
        set { random = value; }
    }

    public IObservable<IList<Trial>> Process(IObservable<CorridorSession> source)
    {
        return source.Select(value => {
            var blocks = value.Metadata.TrialOrder.Blocks;
            if (value.Metadata.RandomSeed.HasValue){
                Random = new Random(value.Metadata.RandomSeed.Value);
            }
            if ((blocks.Count() == 0) | (value.Trials.Count() == 0)){
                throw new Exception("No blocks or trials defined in the .yml file.");
            }
            var trials = new List<Trial>();
            var j = 0;
            foreach (var block in blocks){
                if (block.TrialProbability.Count() != value.Trials.Count()){
                    throw new Exception("Number of trials in probability array does not match number of trials defined in the .yml file.");
                }
                var prob = CumSumProbabilities(block);
                var trialsInBlock = Random.Next(block.Size, block.Size + block.TailSize);
                for (int i = 0; i < trialsInBlock; i++){
                    int trialIndex = 0;
                    double r = Random.NextDouble();
                    while (prob[trialIndex] < r){
                        trialIndex++;
                    }
                    trials.Add(value.Trials[trialIndex]);
                }
            }
            return trials;
        });
    }


    private static double[] CumSumProbabilities(Block block){
        var prob = block.TrialProbability;
        double sum = prob.Sum();
        var probNormalized = prob.Select(p => p/sum).ToArray();
        var cumSum = new double[probNormalized.Length];
        cumSum[0] = probNormalized[0];
        for (int i = 1; i < probNormalized.Length; i++){
            cumSum[i] = probNormalized[i] + cumSum[i-1];
        }
        cumSum[probNormalized.Length-1] = 1.0; //Make sure no rounding errors occur
        return cumSum;
    }

}
