const WebSocket = require('ws')

// create new websocket server
const wss = new WebSocket.Server({port: 8000})

// empty object to store all players
var players = {};
var messageS = {};

// on new client connect
wss.on('connection', function connection (client) {
  // on new message recieved
 client.send("Conceted");
 console.log('Client connected');
 
	client.on('message', function incoming (data) 
		{
	    // get data from string
		var _data = data.toString();
	    // chack for handshake
		if(_data.includes('color'))
	    {
			CreatePlayer (_data,client)
			broadcastTextMesege ();
	 		return;
	    }
		if(_data.includes('TextMessage'))
	    {
	   		TextMesg (data);
	    }
	    // Last option is UpdatePosition 
	    UpdatePosition (data);
	    
  });
	client.on('close',function incoming (data) {
    	delete players[client.udid];
 		broadcastClose(client.udid); 
	});

})
function TextMesg (data){
	var [udid, X, Y, Z,rX,rY,rZ,inTex] =  data.toString().split('\t');
		messageS[udid] = {
			position: {
	    		x: parseFloat(X),
	        	y: parseFloat(Y),
	        	z: parseFloat(Z)
	      	},	
	    	rotation: {
	        x: parseFloat(rX),
	        y: parseFloat(rY),
	        z: parseFloat(rZ)
	      	},
	      	text : inTex,
	    	id : udid
	  	}
	broadcastTextMesege ();
	// console.log('message,  data:' , messageS);
	}
function broadcastTextMesege () {
  // broadcast messages to all clients
  wss.clients.forEach(function each (client) {

    // var otherPlayers = Object.keys(messageS).filter(udid => udid !== client.udid)
    // create array from the rest
    var otherPlayersPositions =  Object.keys(messageS).map(udid => messageS[udid])
    // client.send(JSON.stringify({players: otherPlayersPositions}))
    
    client.send(JSON.stringify({messageS: otherPlayersPositions}))
    // client.send("messages")

	console.log('message,  data:' , otherPlayersPositions);


  })
}

function CreatePlayer (data,client){
	var [udid,r,g,b] = data.split('\t');
	players[udid] = {
	    position: {},
	    rotation: {},
	    color:{
	        r: parseFloat(r),
	        g: parseFloat(g),
	        b: parseFloat(b)
	      	},
	    timestamp: Date.now(),
	    id : udid,
	    moved : true
	 }
	 client.udid = udid
}

function UpdatePosition (data){
	var [udid, X, Y, Z,rX,rY,rZ] =  data.toString().split('\t');
	if (typeof  players[udid] != "undefined") {
		players[udid].position = {
	    	x: parseFloat(X),
	        y: parseFloat(Y),
	        z: parseFloat(Z)
	      }
	    players[udid].rotation = {
	        x: parseFloat(rX),
	        y: parseFloat(rY),
	        z: parseFloat(rZ)
	      }
	// console.log('Position update,  data:' , players[udid]);
	}
}


function broadcastUpdate () {
  // broadcast messages to all clients
  wss.clients.forEach(function each (client) {
    // filter disconnected clients
    if (client.readyState !== WebSocket.OPEN) 
    {
    	console.log('Client deleted');
    	return
    }
    // filter out current player by client.udid
    var otherPlayers = Object.keys(players).filter(udid => udid !== client.udid)
    // create array from the rest
    var otherPlayersPositions = otherPlayers.map(udid => players[udid])
    client.send(JSON.stringify({players: otherPlayersPositions}))
  })
}

function broadcastClose (id) {
  // broadcast messages to all clients
  wss.clients.forEach(function each (client) {
    var otherPlayers = Object.keys(players).filter(udid => udid !== client.udid)
    // create array from the rest
    var otherPlayersPositions = otherPlayers.map(udid => players[udid])
    client.send(`Deleted@${id}`);
    console.log(`Deleted ${id}`);
  })
}
// call broadcastUpdate every 0.1s
setInterval(broadcastUpdate, 200)
