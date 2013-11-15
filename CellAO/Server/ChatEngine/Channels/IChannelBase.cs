namespace ChatEngine.Channels
{
    using Cell.Core;

    using ChatEngine.CoreClient;

    public interface IChannelBase
    {
        /// <summary>
        /// </summary>
        uint ChannelId { get; }

        /// <summary>
        /// </summary>
        ChannelType channelType { get; }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        bool AddClient(IClient client);

        /// <summary>
        /// </summary>
        /// <param name="sourceClient">
        /// </param>
        /// <param name="text">
        /// </param>
        /// <param name="blob">
        /// </param>
        void ChannelMessage(Client sourceClient, string text, string blob = "");

        /// <summary>
        /// </summary>
        void CloseChannel();

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        bool RemoveClient(IClient client);
    }
}