const maxAPI = require("max-api");
const WebSocket = require("ws")

let repeater
let isRunning = false

let primaryUpLeft = "LeftHanded-primaryUp"
let primaryDownLeft = "LeftHanded-primaryDown"
let secondaryUpLeft = "LeftHanded-secondaryUp"
let secondaryDownLeft = "LeftHanded-secondaryDown"

let primaryUpRight = "RightHanded-primaryUp"
let primaryDownRight = "RightHanded-primaryDown"
let secondaryUpRight = "RightHanded-secondaryUp"
let secondaryDownRight = "RightHanded-secondaryDown"

let noteOn = "noteOn"
let noteOff = "noteOff"
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
			case primaryUpRight:
				maxAPI.outlet([noteOff])
				break
			case primaryDownRight:
				maxAPI.outlet([noteOn])
				break
			case secondaryUpRight:
				maxAPI.outlet([pitchEvent, 0])
				break
			case secondaryDownRight:
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
	val = val * ratio
	if (val > 126) val = 126
	if (val < 1) val = 1
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