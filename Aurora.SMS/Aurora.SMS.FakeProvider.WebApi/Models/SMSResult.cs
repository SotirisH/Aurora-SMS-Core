﻿using System;

/// <summary>
/// Te models are kept in a seperate project 
/// so they can been shared easily with the HttpClient projects
/// </summary>
namespace Aurora.SMS.FakeProvider.WebApi.Models
{
    public enum MessageStatus
    {
        OK,
        Pending,
        InvalidNumber,
        InvalidCredentials,
        NotEnoughCredits,
        MessageTooLong,
        Error
    }

    public class SMSResult
    {
        /// <summary>
        ///     Id generated by the server for this message
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     The message returned by the server.
        ///     If everything is ok then an empty string is retirned
        /// </summary>
        public string ReturnedMessage { get; set; }

        public DateTime TimeStamp { get; set; }
        public MessageStatus Status { get; set; }

        /// <summary>
        ///     The external ID that is passed by the client and returned back to it
        ///     in order to make easy to track the message and correlate it with the Guid ID
        /// </summary>
        public string ExternalId { get; set; }
    }
}
