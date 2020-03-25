console.log('Server started');

var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');


var players = [];

io.on('connection', function(socket){
    var thisPlayerId = shortid.generate();
    var player = {
        id: thisPlayerId,
        key:"",
        nr:"",
        x:0,
        y:0,
        z:0,
        r:0,
        g:0,
        b:0
    }

    players[thisPlayerId] = player;

	console.log('Client connected, broadcasting spawn, id:' , thisPlayerId);
    socket.emit('SendId',{id: thisPlayerId});
	socket.broadcast.emit('spawn', {id: thisPlayerId});
    socket.broadcast.emit('requestPosition');

    for(var playerId in players){
        if(playerId == thisPlayerId){
            continue;
        }
        
        socket.emit('spawn', players[playerId]);
        console.log('Sending spawn to new player for id: ', playerId);
    };
    
	socket.on('move', function(data) {
        // data.id = thisPlayerId;
    	console.log('Client moved', JSON.stringify(data));
        if (typeof data.x !== 'undefined') {
    // the variable is defined

        players[data.id].x = data.x;
        players[data.id].y = data.y;
        players[data.id].z = data.z;
        // console.log(players);
        socket.broadcast.emit('move', players[data.id]);
    }
	});

    socket.on('updatePosition', function(data){
        console.log("Update position: ", data);
        data.id = thisPlayerId;
        socket.broadcast.emit('updatePosition', data);
    });

    socket.on('disconnect', function() {
        console.log('Client disconnected');
        if(players[thisPlayerId].x==0&&players[thisPlayerId].y==0&&players[thisPlayerId].z==0)
        {
        delete players[thisPlayerId];
        socket.broadcast.emit('disconnected', {id: thisPlayerId});
        }

    });

    socket.on('SetKey', function(data) {
        players[thisPlayerId].key = data;
        console.log('new key' , players[thisPlayerId].key );
    });
    socket.on('setNr', function(data) {
        players[thisPlayerId].nr = data;
        console.log('new nr' , players[thisPlayerId].nr );
    });
       socket.on('setColor', function(data) {
        data.id = thisPlayerId;
        player.r = data.r;
        player.g = data.g;
        player.b = data.b;
        console.log('color', JSON.stringify(data));

    });

})