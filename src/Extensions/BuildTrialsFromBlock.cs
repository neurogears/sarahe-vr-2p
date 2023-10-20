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

    // public IObservable<List<Trial>> Process(IObservable<CorridorSession> source)
    // {
    //     return source.Select(value => {
    //         var blocks = value.Metadata.TrialOrder.Blocks;
    //         if (value.Metadata.RandomSeed.HasValue){
    //             Random = new Random(value.Metadata.RandomSeed.Value);
    //         }
    //         if ((blocks.Count() == 0) | (value.Trials.Count() == 0)){
    //             throw new Exception("No blocks or trials defined in the .yml file.");
    //         }
    //         var trials = new List<Trial>();
    //         foreach (var block in blocks){
    //             if (block.TrialProbability.Count() != value.Trials.Count()){
    //                 throw new Exception("Number of trials in probability array does not match number of trials defined in the .yml file.");
    //             }
    //             var prob = CumSumProbabilities(block);
    //             var trialsInBlock = Random.Next(block.Size, block.Size + block.TailSize);
    //             for (int i = 0; i < trialsInBlock; i++){
    //                 int trialIndex = 0;
    //                 double r = Random.NextDouble();
    //                 while (prob[trialIndex] < r){
    //                     trialIndex++;
    //                 }
    //                 trials.Add(value.Trials[trialIndex]);
    //             }
    //         }
    //         return trials;
    //     });
    // }
    public IObservable<List<Trial>> Process(IObservable<CorridorSession> source)
    {
        return source.Select(session => {
            var blocks = session.Metadata.TrialOrder.Blocks;

            if (session.Metadata.RandomSeed.HasValue) {
                Random = new Random(session.Metadata.RandomSeed.Value);
            }

            if (!blocks.Any() || !session.Trials.Any()) {
                throw new Exception("No blocks or trials defined in the .yml file.");
            }

            var trials = new List<Trial>();

            foreach (var block in blocks) {
                var trialsForThisBlock = new List<Trial>();
                
                if (block.TrialProbability.Count() != session.Trials.Count()) {
                    throw new Exception("Mismatch between number of trials in probability array and those defined in the .yml file.");
                }

                var totalTrialsInBlock = Random.Next(block.Size, block.Size + block.TailSize);
                var trialsAllocated = 0;

                for (int trialTypeIndex = 0; trialTypeIndex < block.TrialProbability.Count() - 1; trialTypeIndex++) {
                    int numberOfTrialsOfType = (int)(totalTrialsInBlock * block.TrialProbability[trialTypeIndex]);
                    for (int i = 0; i < numberOfTrialsOfType; i++) {
                        trialsForThisBlock.Add(session.Trials[trialTypeIndex]);
                    }
                    trialsAllocated += numberOfTrialsOfType;
                }

                // For the last trial type, ensure we hit our totalTrialsInBlock
                for (int i = 0; i < totalTrialsInBlock - trialsAllocated; i++) {
                    trialsForThisBlock.Add(session.Trials.Last());
                }

                // Shuffle trialsForThisBlock before appending to allTrials
                int n = trialsForThisBlock.Count;
                while (n > 1) {
                    n--;
                    int k = Random.Next(n + 1);
                    Trial tempTrial = trialsForThisBlock[k];
                    trialsForThisBlock[k] = trialsForThisBlock[n];
                    trialsForThisBlock[n] = tempTrial;
                }

                trials.AddRange(trialsForThisBlock);
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
