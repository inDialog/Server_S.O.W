const WebSocket = require('ws')

// create new websocket server
const wss = new WebSocket.Server({port: 8000})

// empty object to store all players
var players = {}

// on new client connect
wss.on('connection', function connection (client) {
  // on new message recieved
 	console.log('Client connected');
  client.send("Conceted");
  client.on('message', function incoming (data) {
    // get data from string
    var _data = data.toString();
    // chack for disconect 
    if(_data.includes('Disconected')){
    	var [udid,msg] = data.toString().split('\t')
    	// client.disconnected;
    	// delete players[udid];
    	console.log('Client deisconected');
		return;
    }
    // UpdatePosition 
    var [udid, x, y, z] = _data.split('\t')
    // store data to players object
    players[udid] = {
      position: {
        x: parseFloat(x),
        y: parseFloat(y),
        z: parseFloat(z)
      },
      moved : true,
      timestamp: Date.now(),
      id : udid
    }
    // save player udid to the client
    client.udid = udid

    // console.log('Position update,  data:' , data.toString());
  });


})

function broadcastUpdate () {
  // broadcast messages to all clients
  wss.clients.forEach(function each (client) {
    // filter disconnected clients
    if (client.readyState !== WebSocket.OPEN) 
    {
     //  delete players[client.udid];
    	// wss.clients.delete(client);
    	console.log('Client deleted');
    	return
    }
    // filter out current player by client.udid
    var otherPlayers = Object.keys(players).filter(udid => udid !== client.udid)
    // create array from the rest
    var otherPlayersPositions = otherPlayers.map(udid => players[udid])
    /// remove unmoved position
    // otherPlayersPositions.forEach(function each (player)
    // {
    //   if(!player.moved)
    //   {
    //     delete otherPlayersPositions[player.udid]; ;
    //     console.log('NotMoved');
    //   }
    // })
    for (var key of Object.keys(otherPlayersPositions)) {
      
      otherPlayersPositions[key].moved  = false;
      console.log(key + " -> " + otherPlayersPositions[key].moved)
    }
    // send it
    client.send(JSON.stringify({players: otherPlayersPositions}))

  })
}

// call broadcastUpdate every 0.1s
setInterval(broadcastUpdate, 100)
