using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Notification;

public static class Messages
{
    /// <summary>
    /// Event name when a message is received
    /// </summary>
    public const string RECEIVE = "ReceiveMessage";

    /// <summary>
    /// Name of the Hub method to register a new user
    /// </summary>
    public const string REGISTER = "Register";

    /// <summary>
    /// Name of the Hub method to send a message
    /// </summary>
    public const string SEND = "SendMessage";

}
public class SignalClient  : IAsyncDisposable
{
    public const string HUBURL = "/TufHub";
    private readonly string _hubUrl;
    private HubConnection _hubConnection;

    private readonly string _username;
    private bool _started = false;
    public SignalClient(string username, string siteUrl)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentNullException(nameof(username));
        if (string.IsNullOrWhiteSpace(siteUrl))
            throw new ArgumentNullException(nameof(siteUrl));
        _username = username;
        // set the hub URL
        _hubUrl = siteUrl.TrimEnd('/') + HUBURL;
    }
    public async Task StartAsync() {
        if (!_started)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();

            _hubConnection.On<string, string>(Messages.RECEIVE, (user, message) =>
            {
                HandleReceiveMessage(user, message);
            });
            // start the connection
            await _hubConnection.StartAsync();
            _started = true;
            // register user on hub to let other clients know they've joined
            await _hubConnection.SendAsync(Messages.REGISTER, _username);
        }

    }
    public event MessageReceivedEventHandler MessageReceived;
    private void HandleReceiveMessage(string username, string message)
    {
        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(username, message));
    }

    public async Task SendAsync(string message)
    {
        if (!_started)
            throw new InvalidOperationException("Client not started");
        await _hubConnection.SendAsync(Messages.SEND, _username, message);
    }

    public async Task StopAsync()
    {
        if (_started)
        {
            // disconnect the client
            await _hubConnection.StopAsync();
            // There is a bug in the mono/SignalR client that does not
            // close connections even after stop/dispose
            // see https://github.com/mono/mono/issues/18628
            // this means the demo won't show "xxx left the chat" since 
            // the connections are left open
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
            _started = false;
        }
    }
    public async ValueTask DisposeAsync()
    { 
        await StopAsync(); 
    }
}

public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
public class MessageReceivedEventArgs : EventArgs
{
    public MessageReceivedEventArgs(string username, string message)
    {
        Username = username;
        Message = message;
    }

    /// <summary>
    /// Name of the message/event
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Message data items
    /// </summary>
    public string Message { get; set; }

}
