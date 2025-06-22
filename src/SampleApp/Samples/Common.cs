using ProtoGenerationLib;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;
using ProtoGenerationLib.Strategies.Internals.PackageNamingStrategies;

namespace SampleApp.Samples
{
    public static class Common
    {
        public const string PATH_TO_PROTO_ROOT = "../../../../SampleApp.GeneratedProtos";

        public const string BASE_PROTO_PATH = "Protos";

        public static void SetFileName(Type samplerRunnerType, IRegistry registry, ProtoGenerationOptions generationOptions)
        {
            var singleFilePath = $"{samplerRunnerType.Name}.proto";
            var singleFileStrategy = new SingleFileStrategy(singleFilePath);
            registry.RegisterFileNamingStrategy(singleFilePath, singleFileStrategy);
            generationOptions.ProtoNamingStrategiesOptions.FileNamingStrategy = singleFilePath;
        }

        public static void SetPackageName(Type samplerRunnerType, IRegistry registry, ProtoGenerationOptions generationOptions)
        {
            var packageName = $"{samplerRunnerType.Name}Pack";
            var singlePackageStrategy = new ConstNameAsPackageStrategy(packageName);
            registry.RegisterPackageNamingStrategy(packageName, singlePackageStrategy);
            generationOptions.ProtoNamingStrategiesOptions.PackageNamingStrategy = packageName;
        }
    }
}
