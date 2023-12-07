const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line

class Point {
    x = 0;
    y = 0;

    constructor(x, y) {
        this.x = x;
        this.y = y;
    }
}

class Range {
    start = 0;
    end = 0;

    constructor(start, end) {
        this.start = start;
        this.end = end;
    }
}

const getDistance = (a, b) => {
    const horizontal = Math.abs(b.x - a.x);
    const vertical = Math.abs(b.y - a.y);

    return horizontal + vertical;
}

const getRange = (sensor, beacon, line) => {
    const distance = getDistance(sensor, beacon);
    const distanceY = Math.abs(sensor.y - line);

    if (distanceY > distance) {
        return null;
    }

    const distanceX = distance - distanceY;
    let startX = sensor.x - distanceX;
    let endX = sensor.x + distanceX;

    return new Range(startX, endX);
}

const part1 = () => {
    const row = 2000000;

    let ranges = [];
    let beacons = [];

    for (let line of lines) {
        const values = line.match(/[-\d]+/g, " ");

        const sensor = new Point(+values[0], +values[1]);
        const closestBeacon = new Point(+values[2], +values[3]);

        if (!beacons.some(item => item.x === closestBeacon.x && item.y === closestBeacon.y)) {
            beacons.push(closestBeacon);
        }

        const range = getRange(sensor, closestBeacon, row);

        if (range !== null) {
            ranges.push(range);
        }
    }

    const mergedRanges = mergeRanges(ranges);

    const positions = mergedRanges
        .map(x => x.end - x.start + 1)
        .reduce((agg, curr) => agg += curr, 0);
    const beaconsInRow = beacons
        .filter(beacon => beacon.y === row && mergedRanges.some(range => beacon.x >= range.start && beacon.x <= range.end)).length;

    return positions - beaconsInRow;
}

const part2 = () => {
    let reports = [];

    for (let line of lines) {
        const values = line.match(/[-\d]+/g, " ");

        const sensor = new Point(+values[0], +values[1]);
        const closestBeacon = new Point(+values[2], +values[3]);

        reports.push([sensor, closestBeacon]);
    }

    for (let row = 0; row <= 4000000; row++)
    {
        const ranges = reports
            .map(x => getRange(x[0], x[1], row))
            .filter(x => x !== null);

        const mergedRanges = mergeRanges(ranges);

        if (mergedRanges.length > 1) {
            return (mergedRanges[0].end + 1) * 4000000 + row;
        }
    }
}

const mergeRanges = (ranges) => {
    const count = ranges.length;

    if (count <= 1) {
        return [...ranges];
    }

    ranges.sort((a, b) => a.start - b.start);

    let result = [];
    let agg = ranges[0];

    for (let i = 1; i < count; i++) {
        let curr = ranges[i];

        if (agg.end >= curr.start - 1) {
            agg.end = Math.max(agg.end, curr.end);
        }
        else {
            result.push(agg);
            agg = curr;
        }
    }

    result.push(agg);

    return result;
}


console.group("Part One");
console.log(part1());
console.groupEnd();

console.group("Part Two");
console.log(part2());
console.groupEnd();