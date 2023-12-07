const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");

// Write code below this line
let sopMarkerIndex = 0;
for (let i = 4; i <= data.length; ++i) {
    const marker = data.substring(i - 4, i);
    const set = new Set(marker.split(""));

    if (set.size === marker.length) {
        sopMarkerIndex = i;
        break;
    }
}

console.group("Part One");
console.log(sopMarkerIndex);
console.groupEnd();

let somMarkerIndex = 0;
for (let i = 14; i <= data.length; ++i) {
    const marker = data.substring(i - 14, i);
    const set = new Set(marker.split(""));

    if (set.size === marker.length) {
        somMarkerIndex = i;
        break;
    }
}

console.group("Part Two");
console.log(somMarkerIndex);
console.groupEnd();