const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
const width = 500;
const height = 200;
let array = Array(width).fill().map(_ => Array(height).fill(0));

const normalizeX = (value) => {
    return value - 200;
}

const addLine = (startX, startY, endX, endY) => {
    const isHorizontal = startY === endY;

    if (isHorizontal) {
        const start = Math.min(startX, endX);
        const end = Math.max(startX, endX);

        for (let i = start; i <= end; i++) {
            array[i][startY] = 1;
        }
    }
    else {
        const col = array[startX];

        const start = Math.min(startY, endY);
        const end = Math.max(startY, endY);

        for (let i = start; i <= end; i++) {
            col[i] = 1;
        }
    }
}

for (let line of lines) {
    var values = line.split(" -> ");

    for (let i = 0; i < values.length - 1; i++) {
        const start = values[i].split(",").map(x => +x);
        const end = values[i + 1].split(",").map(x => +x);

        addLine(normalizeX(start[0]), start[1], normalizeX(end[0]), end[1]);
    }
}

const maxY = array.reduce((agg, curr) => {
    const lastY = curr.lastIndexOf(1);

    if (lastY > agg) {
        agg = lastY;
    }

    return agg;
}, 0);

addLine(0, maxY + 2, width - 1, maxY + 2);

const addSand = () => {
    const source = [normalizeX(500), 0];

    let x = source[0];
    let y = source[1];

    // while (y < height - 1) {
    while (true) {
        let newX = x;
        let newY = y + 1;

        if (array[newX][newY] !== 0) {
            newX--;

            if (array[newX][newY] !== 0) {
                newX += 2;

                if (array[newX][newY] !== 0) {
                    array[x][y] = 2;

                    if (x === source[0] && y === source[1]) {
                        return false;
                    }
                    
                    return true;
                }
            }
        }

        x = newX;
        y = newY;
    }

    // return false;
}

const print = () => {
    let contents = "";

    for (let y = 0; y < height; y++) {
        let line = "";

        for (let x = 0; x < width; x++) {
            switch (array[x][y]) {
                case 0:
                    line += ".";
                    break;
                case 1:
                    line += "#";
                    break;
                case 2:
                    line += "o";
                    break;
            }
        }

        contents += `${line}\n`;
    }

    fs.writeFileSync(path.resolve(__dirname, "result.txt"), contents);
}

while (addSand()) {
}

print();

const result = array.reduce((agg, curr) => agg += curr.filter(x => x === 2).length, 0);

console.group("Part One/Two");
console.log(result);
console.groupEnd();