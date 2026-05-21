using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;
using ProtoGenerationLib;

namespace SampleApp.Samples.SingleTypeSamples
{
    public class CollectionsSample : ISampleRunner
    {
        public class Order
        {
            public List<string> Items { get; set; }
            public Dictionary<Item, int> ItemQuantities1 { get; set; }
            public Dictionary<string, Dictionary<string, int>> ItemQuantities2 { get; set; }
            public Dictionary<IEnumerable<Item>, Dictionary<string, int>> ItemQuantities3 { get; set; }
            public Dictionary<char, string> ItemQuantities4 { get; set; }
            public Dictionary<ulong, string> ItemQuantities5 { get; set; }
            public Dictionary<Letters, string> ItemQuantities6 { get; set; }
            public string[][][] StringJaggedArray {get; set;}
            public int[,,,,] IntMultidimensionalArray {get; set;}
            public byte[,][] ByteMultidimensionalArrayOfArrays {get; set;}
            public byte[][,] ByteArrayOfMultidimensionalArrays { get; set;}
        }

        public class Item
        {
            public int Id { get; set; }
        }

        public enum Letters
        {
            A,
            B
        }

        public void RunSample()
        {
            var sampleType = typeof(Order);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(SingleTypeSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
