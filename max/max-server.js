const maxAPI = require("max-api");
const WebSocket = require("ws")
const MapperMan = require("./mapper-man")
const C = require("./constants")

let repeater
let isRunning = false

maxAPI.addHandler("server", (...args) => {
	maxAPI.outlet(args[0])
	if (args[0] === "autorun") {
		if (isRunning) {
			clearInterval(repeater)
			maxAPI.outlet([knobEvent, 0])
		} else {
			autoRun(knobEvent, 0, 2, 0.2)
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
			case C.primaryUpRight:
				maxAPI.outlet([noteOff])
				break
			case C.primaryDownRight:
				maxAPI.outlet([noteOn])
				break
			case C.secondaryUpRight:
				maxAPI.outlet([pitchEvent, 0])
				break
			case C.secondaryDownRight:
				let val = MapperMan.eightBit(decoded.xVal)
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
		// maxAPI.outlet(`before value: ${value}`)
		// mappedVal = MapperMan.eightBit(value)
		mappedVal = MapperMan.zeroToOne(value)
		// maxAPI.outlet(`after value: ${mappedVal}`)
		maxAPI.outlet([event, mappedVal])
	}, 75)
}
