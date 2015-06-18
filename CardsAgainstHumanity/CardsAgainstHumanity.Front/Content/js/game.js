$(function () {

    $.connection.hub.url = "http://localhost:6408/signalr";

    // Create the connection with the hub
    var game = $.connection.gameHub;

    game.client.addChatMessage = function (message) {
        $('#connected').append('<li>' + message + '</li>');
    };

    game.client.addPlayer = function (player) {
        $('#players').append('<li>' + player + '</li>');
    }

    game.client.removePlayer = function (player) {
        
        $('#connected').append('<li>' + player + ' has left' + '</li>');

        $('li').filter(function () {
            return $.text([this]) === player;
        }).remove();
    }

    game.client.nextBlackCard = function (cardDescription, cardID) {

        $('#black-card').text(cardDescription);
        $('#nextRound').val(cardID);
        $('.white-card-played').remove();
        $('.playCard').show();
    }

    $('#nextRound').click(function () {

        var cardID = $(this).attr("value");

        game.server.nextRound(cardID, 1);
    });

    game.client.playWhiteCard = function (cardID) {
        $('.played-card-view').append('<div id="' + cardID + '" class="white-card-played">');
    }

    $('.playCard').click(function () {
        var cardID = $(this).attr("value");

        $('.playCard').hide();
        $('.card-' + cardID).remove();

        game.server.playCard(cardID, 1);
    })

    $.connection.hub.start(function () {
        game.server.joinRoom(1);
    });

});