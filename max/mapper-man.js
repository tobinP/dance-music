class EightBit {
	// pitch knob
	constructor() {
		let min1 = 0
		let max1 = 2
		let min2 = 0
		let max2 = 127
		let diff1 = max1 - min1
		let diff2 = max2 - min2
		this.ratio = diff2 / diff1
	}

	 map = (val) => {
		val = val * this.ratio
		if (val > 126) val = 126
		if (val < 1) val = 1
		return val
	}
}

class ZeroToOne {
	// freq knob
	constructor() {
		let min1 = 0
		let max1 = 2
		let min2 = 0
		let max2 = 1
		let diff1 = max1 - min1
		let diff2 = max2 - min2
		this.ratio = diff2 / diff1
	}

	map = (val) => {
		val = val * this.ratio
		if (val > 1) val = 1
		if (val < 0.01) val = 0
		return val
	}
}

let eightBit = new EightBit()
let zeroToOne = new ZeroToOne()
module.exports = {
	eightBit: eightBit.map,
	zeroToOne: zeroToOne.map
}
