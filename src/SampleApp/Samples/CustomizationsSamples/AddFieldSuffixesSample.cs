using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib;
using System.Reflection;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class AddFieldSuffixesSample : ISampleRunner
    {
        public class MovingEntity
        {
            public class Location
            {
                public double X { get; set; }
                public double Y { get; set; }
            }

            public class Velocity
            {
                public double Value { get; set; }

                public double Direction { get; set; }
            }

            public Location CurrentLocation { get; set; }

            public Velocity CurrentVelocity { get; set; }

            public TimeSpan LastUpdateTime { get; set; }
        }

        public void RunSample()
        {
            var sampleType = typeof(MovingEntity);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Set the field suffixes.
            generationOptions.AddFieldSuffix<MovingEntity.Location, double>("in meters");
            generationOptions.AddFieldSuffix<MovingEntity.Velocity>(nameof(MovingEntity.Velocity.Value), "in meters per seconds");
            generationOptions.AddFieldSuffix<MovingEntity.Velocity>(nameof(MovingEntity.Velocity.Direction), "in degrees");
            generationOptions.AddFieldSuffix<TimeSpan>("in utc");

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
