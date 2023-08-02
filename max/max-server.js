const maxAPI = require("max-api");
let repeater
let isRunning = false
let noteOn = 100
let noteOff = 0
let noteNoChange = -1
maxAPI.outlet("hey from server")
maxAPI.addHandler("server", (...args) => {
	maxAPI.outlet(args[0])
	if (args[0] === "autorun") {
		maxAPI.outlet("autorun toggled")
		if (isRunning) {
			// clearInterval(repeater)
			maxAPI.outlet([noteOff])
		} else {
			// autoRun(noteNoChange, 5, 40, 2)
			maxAPI.outlet([noteOn])
		}
		isRunning = !isRunning
	}
});



const WebSocket = require("ws")
const wss = new WebSocket.Server({ port: 9090 });
wss.on('connection', function connection(ws) {
	ws.on('message', function message(data) {
		// maxAPI.outlet("message received by server")
		let decoded = JSON.parse(data)
		let val = map(decoded.xVal)
		maxAPI.outlet([decoded.name, val])
	});
	let obj = {
		name: -1,
		xVal: 5
	}
	ws.send(JSON.stringify(obj));

	// autoRun(noteNoChange, 5, 40, 2)

});

let min1 = 1
let max1 = 2
let min2 = 25
let max2 = 100

let diff1 = max1 - min1
let diff2 = max2 - min2
let ratio = diff2 / diff1

function map(val) {
	// if (val < 1) val = 1
	// if (val > 2) val = 2
	// return val * ratio

	val = val * 100
	if (val > 126) val - 126
	return val
}

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
		maxAPI.outlet([event, value])
	}, 75)
}