const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
let visible = 0;

for (let i = 0; i < lines.length; ++i) {
    const line = lines[i];

    for (let j = 0; j < line.length; ++j) {
        const item = +line[j];

        const left = [-1, ...line.slice(0, j)].map(x => +x);
        const maxL = Math.max(...left);
        const right = [-1, ...line.slice(j + 1, line.length)].map(x => +x);
        const maxR = Math.max(...right);

        const column = lines.map(x => +x[j]);
        const up = [-1, ...column.slice(0, i)];
        const maxU = Math.max(...up);
        const down = [-1, ...column.slice(i + 1, lines.length)];
        const maxD = Math.max(...down);

        if (item > maxL || item > maxR || item > maxU || item > maxD) {
            ++visible;
        }
    }
}

console.group("Part One");
console.log(visible);
console.groupEnd();

let scenicScores = [];

for (let i = 0; i < lines.length; ++i) {
    const line = lines[i];

    for (let j = 0; j < line.length; ++j) {
        const item = +line[j];

        const row = [...line].map(x => +x);

        const left = row.slice(0, j).reverse();
        const blockL = left.findIndex(x => x >= item);
        const distanceL = left.length == 0 ? 0 : (blockL != -1 ? blockL + 1 : left.length);

        const right = row.slice(j + 1, line.length);
        const blockR = right.findIndex(x => x >= item);
        const distanceR = right.length == 0 ? 0 : (blockR != -1 ? blockR + 1 : right.length);

        const column = lines.map(x => +x[j]);

        const up = column.slice(0, i).reverse();
        const blockU = up.findIndex(x => x >= item);
        const distanceU = up.length == 0 ? 0 : (blockU != -1 ? blockU + 1 : up.length);

        const down = column.slice(i + 1, lines.length);
        const blockD = down.findIndex(x => x >= item);
        const distanceD = down.length == 0 ? 0 : (blockD != -1 ? blockD + 1 : down.length);

        const score = distanceL * distanceR * distanceU * distanceD;
        scenicScores.push(score);
    }
}

console.group("Part Two");
console.log(Math.max(...scenicScores));
console.groupEnd();