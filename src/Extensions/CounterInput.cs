using System;
using System.Reactive.Linq;
using OpenCV.Net;
using NationalInstruments.DAQmx;
using System.Reactive.Disposables;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bonsai.DAQmx
{
    public class CounterInput : Source<Mat>
    {
        /// <summary>
        /// Gets or sets the optional source terminal of the clock. If not specified,
        /// the internal clock of the device will be used.
        /// </summary>
        [Description("The optional source terminal of the clock. If not specified, the internal clock of the device will be used.")]
        private string signalSource = string.Empty;
        public string SignalSource
        {
            get { return signalSource; }
            set { signalSource = value; }
        }

        /// <summary>
        /// Gets or sets the sampling rate for acquiring voltage measurements, in
        /// samples per second.
        /// </summary>
        [Description("The sampling rate for acquiring voltage measurements, in samples per second.")]
        public double SampleRate { get; set; }

        /// <summary>
        /// Gets or sets a value specifying on which edge of a clock pulse sampling takes place.
        /// </summary>
        [Description("Specifies on which edge of a clock pulse sampling takes place.")]
        private SampleClockActiveEdge activeEdge = SampleClockActiveEdge.Rising;
        public SampleClockActiveEdge ActiveEdge
        {
            get { return activeEdge; }
            set { activeEdge = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying whether the acquisition task will acquire
        /// a finite number of samples or if it continuously acquires samples.
        /// </summary>
        [Description("Specifies whether the acquisition task will acquire a finite number of samples or if it continuously acquires samples.")]
        private SampleQuantityMode sampleMode = SampleQuantityMode.ContinuousSamples;
        public SampleQuantityMode SampleMode
        {
            get { return sampleMode; }
            set { sampleMode = value; }
        }


        /// <summary>
        /// Gets or sets the number of samples to acquire, for finite samples, or the
        /// size of the buffer for continuous sampling.
        /// </summary>
        [Description("The number of samples to acquire, for finite samples, or the size of the buffer for continuous sampling.")]
        private int bufferSize = 1000;
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        /// <summary>
        /// Gets or sets the number of samples per channel in each output buffer.
        /// If not specified, the number of samples will be set to the size of the buffer.
        /// </summary>
        [Description("The number of samples in each output buffer. If not specified, the number of samples will be set to the size of the buffer.")]
        public int? SamplesPerChannel { get; set; }

        /// <summary>
        /// Gets the collection of analog input channels from which to acquire voltage samples.
        /// </summary>
        // [Editor("Bonsai.Design.DescriptiveCollectionEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        // [Description("The collection of analog input channels from which to acquire voltage samples.")]
        // public Collection<AnalogInputChannelConfiguration> Channels
        // {
        //     get { return channels; }
        // }
        private string channel;
        public string Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        Task CreateTask()
        {
            var task = new Task();

            task.CIChannels.CreateAngularEncoderChannel(
                channel,
                "citask",
                CIEncoderDecodingType.X1,
                false,
                0,
                CIEncoderZIndexPhase.AHighBHigh,
                1000,
                0,
                CIAngularEncoderUnits.Degrees);

            return task;
        }

        /// <summary>
        /// Generates an observable sequence of voltage measurements from one or
        /// more DAQmx counter input channels.
        /// </summary>
        /// <returns>
        /// A sequence of 2D <see cref="Mat"/> objects storing the voltage samples.
        /// Each row corresponds to a channel in the acquisition task, and each column
        /// to a sample from each of the channels. The order of the channels follows
        /// the order in which you specify the channels in the <see cref="Channels"/>
        /// property.
        /// </returns>
        public override IObservable<Mat> Generate()
        {
            return Observable.Create<Mat>(observer =>
            {
                var task = CreateTask();
                var bufferSize = BufferSize;
                task.Timing.ConfigureSampleClock(SignalSource, SampleRate, ActiveEdge, SampleMode, bufferSize);
                task.Control(TaskAction.Verify);
                var counterInReader = new CounterReader(task.Stream);
                var samplesPerChannel = SamplesPerChannel.GetValueOrDefault(bufferSize);
                AsyncCallback counterCallback = null;
                counterCallback = new AsyncCallback(result =>
                {
                    var data = counterInReader.EndReadMultiSampleInt32(result);
                    var output = Mat.FromArray(data);
                    observer.OnNext(output);
                    counterInReader.BeginReadMultiSampleInt32(samplesPerChannel, counterCallback, null);
                });

                counterInReader.SynchronizeCallbacks = true;
                counterInReader.BeginReadMultiSampleInt32(samplesPerChannel, counterCallback, null);
                return Disposable.Create(() =>
                {
                    task.Stop();
                    task.Dispose();
                });
            });
        }

        /// <summary>
        /// Generates an observable sequence of voltage measurements from one or
        /// more DAQmx counter input channels, where each new buffer is emitted only
        /// when an observable sequence emits a notification.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting sample buffers.
        /// </param>
        /// <returns>
        /// A sequence of 2D <see cref="Mat"/> objects storing the voltage samples.
        /// Each row corresponds to a channel in the acquisition task, and each column
        /// to a sample from each of the channels. The order of the channels follows
        /// the order in which you specify the channels in the <see cref="Channels"/>
        /// property.
        /// </returns>
        public IObservable<Mat> Generate<TSource>(IObservable<TSource> source)
        {
            return Observable.Defer(() =>
            {
                var task = CreateTask();
                var bufferSize = BufferSize;
                var sampleRate = SampleRate;
                if (sampleRate > 0)
                {
                    task.Timing.ConfigureSampleClock(SignalSource, sampleRate, ActiveEdge, SampleMode, bufferSize);
                }
                task.Control(TaskAction.Verify);
                var counterInReader = new CounterReader(task.Stream);
                var samplesPerChannel = SamplesPerChannel.GetValueOrDefault(bufferSize);
                return Observable.Using(() => Disposable.Create(
                    () =>
                    {
                        task.Stop();
                        task.Dispose();
                    }),
                    resource => source.Select(_ =>
                    {
                        var data = counterInReader.ReadMultiSampleInt32(samplesPerChannel);
                        return Mat.FromArray(data);
                    }));
            });
        }
    }
}