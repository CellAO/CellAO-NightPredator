#region License

// Copyright (c) 2005-2014, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

namespace CellAO.Core.Components
{
    #region Usings ...

    using System;

    using CellAO.Core.Entities;
    using CellAO.Core.Network;

    using SmokeLounge.AOtomation.Messaging.Messages;

    #endregion

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="TU">
    /// </typeparam>
    public class BaseMessageHandler<T, TU> : AbstractMessageHandler<T>
        where T : MessageBody, new() where TU : new()
    {
        /// <summary>
        /// </summary>
        public enum MessageHandlerDirection
        {
            /// <summary>
            /// </summary>
            InboundOnly, 

            /// <summary>
            /// </summary>
            OutboundOnly, 

            /// <summary>
            /// </summary>
            All
        }

        #region Singleton

        /// <summary>
        /// </summary>
        protected static TU _default;

        /// <summary>
        /// </summary>
        public MessageHandlerDirection Direction { get; protected set; }

        /// <summary>
        /// </summary>
        public static TU Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new TU();
                }

                return _default;
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        public BaseMessageHandler()
        {
        }

        #region Inbound

        protected bool UpdateCharacterStatsOnReceive = false;

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="message">
        /// </param>
        /// <param name="updateCharacterStats">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        //public override void Receive(IZoneClient client, Message message)
        public override void Receive(MessageWrapper<T> messageWrapper)
        {
            IZoneClient client = messageWrapper.Client;
            MessageBody messageBody = (messageWrapper.Message != null) ? messageWrapper.Message.Body : messageWrapper.MessageBody;

            if ((this.Direction == MessageHandlerDirection.All)
                || (this.Direction == MessageHandlerDirection.InboundOnly))
            {
                T body = messageBody as T;
                if (body != null)
                {
                    this.Read(body, client);

                    if (this.UpdateCharacterStatsOnReceive)
                    {
                        if (client != null)
                        {
                            if (client.Character != null)
                            {
                                client.Character.SendChangedStats();
                            }
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException(
                        "Don't throw other messagetypes on me (" + messageBody.GetType() + " instead of " + typeof(T)
                        + ")");
                }
            }
            else
            {
                throw new NotImplementedException("This message handler cannot receive inbound messages");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void Read(T message, IZoneClient client)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Outbound

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="messageDataFiller">
        /// </param>
        /// <param name="announceToPlayfield">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void Send(
            ICharacter character, 
            MessageDataFiller messageDataFiller, 
            bool announceToPlayfield = false)
        {
            if ((this.Direction == MessageHandlerDirection.All)
                || (this.Direction == MessageHandlerDirection.OutboundOnly))
            {
                T mb = this.Create(character, messageDataFiller);
                character.Send(mb, announceToPlayfield);
            }
            else
            {
                throw new NotImplementedException("This message handler cannot send outboud messages");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="messageDataFiller">
        /// </param>
        /// <param name="announceToPlayfield">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void SendToPlayfield(
            ICharacter character, 
            MessageDataFiller messageDataFiller)
        {
            this.Send(character, messageDataFiller, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="messageDataFiller">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override T Create(ICharacter character, MessageDataFiller messageDataFiller)
        {
            T temp = new T();
            messageDataFiller(temp);
            return temp;
        }

        #endregion
    }
}