console.log('Server started');

var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');
var players = [];



io.on('connection', function(socket)
{
    var thisPlayerId = shortid.generate();
    var player = {
        id: thisPlayerId,
        key:0,
        nr:0,
        state:0,
        x:0,
        y:0,
        z:0,
        r:0,
        g:0,
        b:0
    }

    players[thisPlayerId] = player;
    socket.emit('SendId',{id: thisPlayerId});
    socket.broadcast.emit('spawn', {id: thisPlayerId});
    socket.broadcast.emit('requestPosition');
	console.log('Client connected, broadcasting spawn, id:' , player);


    for(var playerId in players)
    {
        if(playerId == thisPlayerId)
        {
            continue;
        }
        socket.emit('spawn', players[playerId]);
        console.log('Sending spawn to new player for id: ', playerId);
    };
    
	socket.on('move', function(data) {
        console.log('Client moved', JSON.stringify(data));
        
        if (typeof  players[data.id] !== 'undefined') {
    // the variable is defined

        players[data.id].x = data.x;
        players[data.id].y = data.y;
        players[data.id].z = data.z;
        console.log(players);
        socket.broadcast.emit('move', players[data.id]);
        console.log('Client moved', JSON.stringify(data));
    }
	});

    socket.on('updatePosition', function(data){
        console.log("Update position: ", data);
        data.id = thisPlayerId;
        socket.broadcast.emit('updatePosition', data);
    });

    socket.on('disconnect', function() 
    {

        if (typeof  players[thisPlayerId] == 'undefined') 
        {
                socket.emit('disconnected', {id: thisPlayerId});
                console.log('Client disconnected');

        }else
        {
              if(players[thisPlayerId].x==0&&players[thisPlayerId].y==0&&players[thisPlayerId].z==0)
            {
                delete players[thisPlayerId];
                socket.emit('disconnected', {id: thisPlayerId});
            }
        }
    });

    socket.on('SetKey', function(data) {
         var count =0;
    for (var i in players) 
    {
      console.log(players[i].key);

      if(players[i].key==data)
      {
        count++;
      }
    }
    console.log('nr of connected');
    console.log(count);

    if(count>1 )
    {
        socket.broadcast.emit('disconnected', {id: thisPlayerId});
        delete players[thisPlayerId];
    }else
    {
        players[thisPlayerId].nr = count;
        players[thisPlayerId].key = data;
        console.log('new nr' , players[thisPlayerId].nr );
        console.log('new key' , players[thisPlayerId].key );
    }
    });
    socket.on('setNr', function(data) {
        // players[thisPlayerId].nr = data;
        // console.log('new nr' , players[thisPlayerId].nr )
    });
       socket.on('setColor', function(data) {
        // console.log('color', JSON.stringify(players[thisPlayerId]));
        if (typeof  players[thisPlayerId] != 'undefined') {
        players[data.id].r = data.r;
        players[data.id].g = data.g;
        players[data.id].b = data.b;
        console.log('color', JSON.stringify(data));
        }
    });

})