const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
let array = [];
let index = 0;

for (let line of lines) {
    if (line !== '') {
        if (array[index] === undefined) {
            array[index] = 0;
        }
        array[index] += +line;
    }
    else {
        index++;
    }
}

console.group("Part One");
console.log(Math.max(...array));
console.groupEnd();

console.group("Part Two");
array.sort((a, b) => b - a);
console.log(array[0] + array[1] + array[2]);
console.groupEnd();