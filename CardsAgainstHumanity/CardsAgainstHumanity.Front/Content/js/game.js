$(function () {

    $.connection.hub.url = "http://localhost:6408/signalr";

    // Create the connection with the hub
    var game = $.connection.gameHub;

    game.client.addChatMessage = function (message) {
        $('#notes').append('<div class="note">' + message + '</div>');
    };

    game.client.addPlayer = function (playerID, playerCzar, player) {
        if (playerCzar !== 1) {
            $('#players').append('<tr><td id="' + playerID + '" value="' + playerCzar + '" >' + player + '</td><td>0</td></tr>');
        }
     else
        {
            $('#players').append('<tr><td id="' + playerID + '" value="' + playerCzar + '" >' + player + ' <span class="czar">(Czar)</span></td></tr> ');
        }
    }

    game.client.removePlayer = function (player) {
        
        $('#notes').append('<div class="note">' + player + ' has left' + '</div>');

        $('li').filter(function () {
            return $.text([this]) === player;
        }).remove();
    }

    game.client.nextBlackCard = function (cardDescription, cardID, czarID) {

        $('#black-card').text(cardDescription);
        $('#nextRound').val(cardID);

        $(".czar").remove();
        $('#' + czarID + '').append('<span class="czar">(Czar)</span>');

        $('.white-card-played').remove();
        $('.playCard').show();

    }

    $('#nextRound').click(function () {

        var cardID = $(this).attr("value");

        game.server.nextRound(cardID, 1);
        
    });

    game.client.playWhiteCard = function (cardID, cardDescription) {
        $('.played-card-view').append('<div id="' + cardID + '" class="white-card-played"><span class="card-description-played">' + cardDescription + '</span>');
        $('.card-description-played').hide();
    }

    game.client.showWhiteCards = function () {
        $('.card-description-played').show();
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