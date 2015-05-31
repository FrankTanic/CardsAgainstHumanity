$(function () {

    $.connection.hub.url = "http://localhost:6408/signalr";

    // Create the connection with the hub
    var game = $.connection.gameHub;

    var username = prompt("Enter your username: ", "");

    game.client.addChatMessage = function (message) {
        $('#connected').append('<li>' + message + '</li>');
    };

    $.connection.hub.start(function () {
        game.server.joinRoom(username, 1);
    });

    $.connection.hub.disconnected(function () {
        game.server.leaveRoom(username, 1);
    })

});