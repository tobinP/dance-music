const maxAPI = require("max-api");
const WebSocket = require("ws")

let repeater
let isRunning = false
let noteOn = "noteOn"
let noteOff = "noteOff"
let primaryUp = "primaryUp"
let primaryDown = "primaryDown"
let secondaryUp = "secondaryUp"
let secondaryDown = "secondaryDown"
let knobEvent = "knob"
let pitchEvent = "pitch"
let noEventName = "noName"

maxAPI.addHandler("server", (...args) => {
	maxAPI.outlet(args[0])
	if (args[0] === "autorun") {
		if (isRunning) {
			clearInterval(repeater)
			maxAPI.outlet([pitchEvent, 0])
		} else {
			autoRun(pitchEvent, 5, 100, 2)
		}
		isRunning = !isRunning
	}
});

const wss = new WebSocket.Server({ port: 9090 });
wss.on('connection', function connection(ws) {
	ws.on('message', function message(data) {
		let decoded = JSON.parse(data)
		maxAPI.outlet(`name: ${decoded.name}, val: ${decoded.xVal}`)
		switch (decoded.name) {
			case primaryUp:
				maxAPI.outlet([noteOff])
				break
			case primaryDown:
				maxAPI.outlet([noteOn])
				break
			case secondaryUp:
				maxAPI.outlet([pitchEvent, 0])
				break
			case secondaryDown:
				let val = mapPitch(decoded.xVal)
				maxAPI.outlet(`in secondaryDown, val: ${val}`)
				maxAPI.outlet([pitchEvent, val])
				break
			case knobEvent: // change name to match vr controller action
				maxAPI.outlet([knobEvent, decoded.xVal])
				break
			default:
			break
		} 
	});
});

let min1 = 0
let max1 = 2
let min2 = 0
let max2 = 126

let diff1 = max1 - min1
let diff2 = max2 - min2
let ratio = diff2 / diff1

function mapPitch(val) {
	// if (val < 1) val = 1
	// if (val > 2) val = 2
	// return val * ratio

	val = val * ratio
	if (val > 126) val = 126
	if (val < 1) val = 1
	return val
}

// clearInterval(repeater)
// autoRun(noEventName, 5, 40, 2)
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