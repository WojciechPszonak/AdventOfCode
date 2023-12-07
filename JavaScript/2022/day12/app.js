const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
let array = lines
    .map(x => x.split("")
        .map(x => {
            switch (x) {
                case "S":
                    return "a";
                case "E":
                    return "z";
                default:
                    return x;
            }
        })
        .map(x => x.charCodeAt(0) - 96));

const sx = 0;
const sy = 20;
const ex = 112;
const ey = 20;

// const sx = 0;
// const sy = 0;
// const ex = 5;
// const ey = 2;

const distances = Array(array.length).fill().map(x => Array(array[0].length).fill(null));
let queue = [[ey, ex]];

const findPathFor = (x, y) => {
    const current = array[y][x];
    const currentDistance = distances[y][x];

    const neighbors = [
        [y - 1, x],
        [y + 1, x],
        [y, x - 1],
        [y, x + 1]
    ];

    for (let i = 0; i < neighbors.length; i++) {
        const neighbor = neighbors[i];

        if (neighbor[0] < 0
            || neighbor[0] >= array.length
            || neighbor[1] < 0
            || neighbor[1] >= array[0].length
            || current - array[neighbor[0]][neighbor[1]] > 1) {
            neighbors.splice(i, 1);
            i--;
        }
        else if (distances[neighbor[0]][neighbor[1]] === null || distances[neighbor[0]][neighbor[1]] > 1 + currentDistance) {
            distances[neighbor[0]][neighbor[1]] = 1 + currentDistance;
            queue.push(neighbor);
        }
    }
}

const getBestStart = () => {
    let locations = [];

    for (let j = 0; j < array.length; j++) {
        for (let i = 0; i < array[j].length; i++) {
            if (array[j][i] === 1) {
                locations.push([j, i]);
            }
        }
    }

    let bestDistance = null;

    for (let location of locations) {
        if (bestDistance === null || (distances[location[0]][location[1]] != null && distances[location[0]][location[1]] < bestDistance)) {
            bestDistance = distances[location[0]][location[1]];
        }
    }

    return bestDistance;
}

distances[ey][ex] = 0;
count = 0;
while (queue.length) {
    count++;
    let item = queue.shift();
    findPathFor(item[1], item[0]);
}

const print = () => {
    let contents = "";

    for (let line of distances) {
        contents += line.reduce((agg, curr) => agg += curr != null ? "#" : ".", "") + "\n";
    }

    fs.writeFileSync(path.resolve(__dirname, "result.txt"), contents);
}

print();

console.group("Part One");
console.log(distances[sy][sx]);
console.groupEnd();

console.group("Part Two");
console.log(getBestStart());
console.groupEnd();