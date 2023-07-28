const maxAPI = require("max-api");
let repeater
let isRunning = false
maxAPI.outlet("hey from server")
maxAPI.addHandler("text", (...args) => {
	maxAPI.outlet(args[0])
	if (args[0] === "autorun") {
		maxAPI.outlet("autorun toggled")
		if (isRunning) {
			clearInterval(repeater)
		} else {
			autoRun(-1, 5, 40, 2)
		}
		isRunning = !isRunning
	}
});

const WebSocket = require("ws")
const wss = new WebSocket.Server({ port: 9090 });
wss.on('connection', function connection(ws) {
	ws.on('message', function message(data) {
		maxAPI.outlet("message received by server")
		let decoded = JSON.parse(data)
		maxAPI.outlet([decoded.name, decoded.xVal])
	});
	let obj = {
		name: -1,
		xVal: 5
	}
	ws.send(JSON.stringify(obj));

	autoRun(-1, 5, 40, 2)

});
function autoRun(event, min, max, incrementValue) {
	let value = 0
	let shouldIncrease = true
	repeater = setInterval(() => {
		console.log('value: %s', value);
		if (shouldIncrease) {
			value += incrementValue
			if (value > max) {
				shouldIncrease = false
			}
		} else {
			value -= incrementValue
			if (value < min) {
				shouldIncrease = true
			}
		}
		let obj = {
			name: event,
			xVal: value
		}
		maxAPI.outlet([obj.name, obj.xVal])
	}, 75)
}