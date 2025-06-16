using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using System;

namespace ProtoGenerationLib.Mappers.Abstracts
{
    /// <summary>
    /// Mapper between csharp type to its <see cref="IProtoTypeBaseMetadata"/>.
    /// </summary>
    public interface ITypeMapper
    {
        /// <summary>
        /// Check whether or not the given <paramref name="type"/> can be handled
        /// by the this mapper.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// can be handled by this mapper otherwise <see langword="false"/>.
        /// </returns>
        bool CanHandle(Type type);

        /// <summary>
        /// Map the given <paramref name="type"/> to its <see cref="IProtoTypeBaseMetadata"/>.
        /// </summary>
        /// <param name="type">The type to map.</param>
        /// <returns>The <see cref="IProtoTypeBaseMetadata"/> that represents the given <paramref name="type"/>.</returns>
        /// <remarks>
        /// <b>Notes</b>:<br/>
        /// <list type="number">
        /// <item>
        /// Properties that you do not initialize will be initialized to <see langword="null"/>
        /// and afterward will get value base on the chosen strategies and will be styled.
        /// </item>
        /// <item>
        /// Properties that you chose to fill won't be styled meaning that you should
        /// styled them yourself however you want.
        /// </item>
        /// <item>
        /// If for some reason there is a new primitive type that is not yet supported
        /// by this proto generator, you can add custom mapper that will support the new primitive
        /// by filling the <see cref="IProtoTypeBaseMetadata.Package"/> and <see cref="IProtoTypeBaseMetadata.FilePath"/>
        /// with <see cref="string.Empty"/> values (<b>And not nulls!</b>).
        /// </item>
        /// </list>
        /// </remarks>
        IProtoTypeBaseMetadata MapTypeToProtoMetaData(Type type);
    }
}
