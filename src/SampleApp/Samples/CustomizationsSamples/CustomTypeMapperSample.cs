using ProtoGenerationLib;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class CustomTypeMapperSample : ISampleRunner
    {
        public class DataType
        {
            public class NestedDataType
            {
                public int Id { get; set; }
            }

            public string Name { get; set; }
            public NestedDataType NestedType { get; set; }
        }

        public class CustomTypeMapper : ICustomTypeMapper
        {
            public bool CanHandle(Type type)
            {
                return type.Equals(typeof(DataType.NestedDataType));
            }

            public IProtoTypeBaseMetadata MapTypeToProtoMetadata(Type type)
            {
                return new ProtoTypeBaseMetadata
                {
                    Name = "ChangeName",
                    Package = "ChangePackage",
                    FilePath = "ChangeFilePath.proto",
                    ShouldCreateProtoType = true,
                };
            }
        }

        public void RunSample()
        {
            var sampleType = typeof(DataType);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Register the custom mapper.
            generationOptions.CustomTypeMappers.Add(new CustomTypeMapper());

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
