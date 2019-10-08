﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using server.Model.DTO;
using server.Util;
using Sparrow.Collections;

namespace server.Services.SignalR
{
    public static class SignalRMemberService
    {
        private static ConcurrentDictionary<string, List<SignalRClient>> UserConnections { get; } =
            new ConcurrentDictionary<string, List<SignalRClient>>();

        private static ConcurrentSet<string> VisitorConnections { get; } = new ConcurrentSet<string>();

        public static void AddConnection(HubCallerContext context)
        {
            var connectionId = context.ConnectionId;
            var userIdentifier = context.UserIdentifier;
            var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (userIdentifier != null)
            {
                var username = context.User.Identity.Name;
                var websiteClient = new SignalRClient(username, emailClaim, connectionId);

                if (UserConnections.TryGetValue(userIdentifier, out var connectionList))
                {
                    connectionList.Add(websiteClient);
                }
                else
                {
                    UserConnections.TryAdd(userIdentifier, new List<SignalRClient>() {websiteClient});
                }
            }
            else
            {
                VisitorConnections.Add(connectionId);
            }
        }

        public static void RemoveConnection(HubCallerContext context)
        {
            var connectionId = context.ConnectionId;
            var userIdentifier = context.UserIdentifier;

            if (userIdentifier != null)
            {
                if (UserConnections.TryGetValue(userIdentifier, out var connectionList))
                {
                    connectionList.RemoveAll(x => x.ConnectionId == connectionId);
                    if (!connectionList.Any())
                        UserConnections.TryRemove(userIdentifier, out _);
                }
            }
            else
            {
                VisitorConnections.TryRemove(connectionId);
            }
        }

        public static WebsiteClient[] GetOnlineUsers()
        {
            return UserConnections.Select(x => new WebsiteClient(x.Key, x.Value)).ToArray();
        }

        public static int GetVisitorCount()
        {
            return VisitorConnections.Count;
        }

        public static string[] GetConnectionsByUser(string id)
        {
            return UserConnections.TryGetValue(id, out var connections) ? connections.Select(x => x.ConnectionId).ToArray() : null;
        }
    }

    [Serializable]
    public class WebsiteClient
    {
        public WebsiteClient()
        {
            
        }

        public WebsiteClient(string id, List<SignalRClient> clients)
        {
            Id = id;
            Clients = clients;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public List<SignalRClient> Clients { get; set; }
    }

    [Serializable]
    public class SignalRClient
    {
        public SignalRClient(string clientName, string clientEmail, string connectionId)
        {
            ClientName = clientName;
            ClientEmail = clientEmail;
            ConnectionId = connectionId;
        }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientEmail { get; set; }

        [Required]
        public string ConnectionId { get; set; }
    }
}