using ProtoGenerationLib.Attributes;

namespace SampleApp.Samples.MultipleFilesSamples.Dtos.GameDtos
{
    [ProtoService]
    public interface IGameService
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public Tile[,,] GetGameBoard(int gameId, string gameName);

        [ProtoRpc(ProtoRpcType.ClientStreaming)]
        public void UpdateGameLive(GameUpdate gameUpdate);

        [ProtoRpc(ProtoRpcType.ServerStreaming)]
        public GameUpdate GetGameLiveUpdates();

        [ProtoRpc(ProtoRpcType.Unary)]
        public Tile GetTile((int X, int Y, int Z) wantedTile);
    }
}
