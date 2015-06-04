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

    game.client.removePlayer = function (player) {
        
        $('#connected').append('<li>' + player + ' has lefted' + '</li>');

        $('li').filter(function () {
            return $.text([this]) === player;
        }).remove();
    }

    $.connection.hub.start(function () {
        game.server.joinRoom(username, 1);
    });

});