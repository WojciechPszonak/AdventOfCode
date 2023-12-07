const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data
    .replaceAll("move ", "")
    .replaceAll("from ", "")
    .replaceAll("to ", "")
    .split("\n")
    .slice(10, -1);

// Write code below this line
let crates = [
    ["Q", "S", "W", "C", "Z", "V", "F", "T"],
    ["Q", "R", "B"],
    ["B", "Z", "T", "Q", "P", "M", "S"],
    ["D", "V", "F", "R", "Q", "H"],
    ["J", "G", "L", "D", "B", "S", "T", "P"],
    ["W", "R", "T", "Z"],
    ["H", "Q", "M", "N", "S", "F", "R", "J"],
    ["R", "N", "F", "H", "W"],
    ["J", "Z", "T", "Q", "P", "R", "B"],
];

for (let line of lines) {
    const values = line.split(" ");
    
    const crateCount = +values[0];
    const src = +values[1] - 1;
    const dest = +values[2] - 1;

    for (let index = 0; index < crateCount; ++index) {
        const crate = crates[src].pop();
        crates[dest].push(crate);
    }
}

console.group("Part One");
console.log(crates.reduce((agg, curr) => agg += curr[curr.length - 1]));
console.groupEnd();

let crates2 = [
    ["Q", "S", "W", "C", "Z", "V", "F", "T"],
    ["Q", "R", "B"],
    ["B", "Z", "T", "Q", "P", "M", "S"],
    ["D", "V", "F", "R", "Q", "H"],
    ["J", "G", "L", "D", "B", "S", "T", "P"],
    ["W", "R", "T", "Z"],
    ["H", "Q", "M", "N", "S", "F", "R", "J"],
    ["R", "N", "F", "H", "W"],
    ["J", "Z", "T", "Q", "P", "R", "B"],
];

for (let line of lines) {
    const values = line.split(" ");
    
    const crateCount = +values[0];
    const src = +values[1] - 1;
    const dest = +values[2] - 1;

    const moved = crates2[src].splice(crates2[src].length - crateCount, crateCount);
    crates2[dest].push(...moved);
}

console.group("Part Two");
console.log(crates2.reduce((agg, curr) => agg += curr[curr.length - 1]));
console.groupEnd();