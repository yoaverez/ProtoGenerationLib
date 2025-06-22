using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib;
using System.Reflection;
using ProtoGenerationLib.Converters.CustomConverters;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class CustomConverterToChageDataTypeFields : ISampleRunner
    {
        public class Velocity
        {
            private double valueInMeterPerSec;
            public double MetersPerSec => valueInMeterPerSec;
            public double KiloMetersPerHour => valueInMeterPerSec * 3.6;
            public double MilesPerHour => valueInMeterPerSec * 2.2369;
            public double Knots => valueInMeterPerSec * 1.943;
        }

        public class CustomConverter : CSharpDataTypeToDataTypeMetadataCustomConverter
        {
            public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
            {
                return type.Equals(typeof(Velocity));
            }

            protected override IDataTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
            {
                var dataType = typeof(Velocity);
                return new DataTypeMetadata()
                {
                    Type = dataType,
                    Fields = new List<IFieldMetadata>
                    {
                        new FieldMetadata(typeof(double), "ValueInMetersPerSec", Array.Empty<Attribute>(), dataType),
                    }
                };
            }
        }

        public void RunSample()
        {
            var sampleType = typeof(Velocity);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Register the custom converter.
            protoGenerator.Registry.RegisterDataTypeCustomConverter(new CustomConverter());

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
