$(function () {

    $.connection.hub.url = "http://localhost:6408/signalr";

    // Create the connection with the hub
    var game = $.connection.gameHub;

    var username = prompt("Enter your username: ", "");

    $.connection.hub.qs = { 'username': username };

    game.client.addChatMessage = function (message) {
        $('#connected').append('<li>' + message + '</li>');
    };

    game.client.addPlayer = function (player) {
        $('#players').append('<li>' + player + '</li>');
    }

    $.connection.hub.start(function () {
        game.server.joinRoom(username, 1);
    });

});