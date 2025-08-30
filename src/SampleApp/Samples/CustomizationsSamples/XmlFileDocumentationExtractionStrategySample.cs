using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib;
using ProtoGenerationLib.Attributes;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class XmlFileDocumentationExtractionStrategySample : ISampleRunner
    {
        /// <summary>
        /// A contract for representing an animal service.
        /// </summary>
        [ProtoService]
        public interface IAnimalService
        {
            /// <summary>
            /// Gets the name of the given <paramref name="animal"/> owner.
            /// </summary>
            /// <returns>A string representing this animal's owner id.</returns>
            [ProtoRpc(ProtoRpcType.Unary)]
            string GetOwnerName(Animal animal);

            /// <summary>
            /// Gets the id of the given <paramref name="animal"/> owner.
            /// </summary>
            /// <returns>A string representing this animal's owner id.</returns>
            [ProtoRpc(ProtoRpcType.Unary)]
            string GetOwnerId(Animal animal);

            /// <summary>
            /// Gets all the available <see cref="Dog"/>s.
            /// </summary>
            /// <returns>All the available <see cref="Dog"/>s.</returns>
            [ProtoRpc(ProtoRpcType.Unary)]
            IEnumerable<Dog> GetAllDogs();
        }

        public abstract class Animal
        {
            /// <summary>
            /// The name of the animal.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The age of the animal.
            /// </summary>
            public double Age { get; set; }

            /// <summary>
            /// The age of the animal in human years.
            /// </summary>
            public virtual double AgeInHumanYears => Age;
        }

        /// <summary>
        /// The best friend of a human.
        /// </summary>
        public class Dog : Animal
        {
            /// <summary>
            /// The type of the <see cref="Dog"/> e.g Bulldog or Golden Retriever etc.
            /// </summary>
            public string Breed { get; set; }

            /// <summary>
            /// <inheritdoc cref="Animal.Name"/> This name is used
            /// by close family.
            /// </summary>
            public string NickName { get; set; }
        }

        public void RunSample()
        {
            var sampleType = typeof(IAnimalService);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Set the extraction strategy to flatten.
            generationOptions.AnalysisOptions.DocumentationExtractionStrategy =
                StrategyNamesLookup.DocumentationExtractionStrategiesLookup[DocumentationExtractionStrategyKind.FromXmlFiles];

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
