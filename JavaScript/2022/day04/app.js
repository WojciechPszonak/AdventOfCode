const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
const getSections = (range) => range.split("-").map(x => +x);
let fullyContained = 0;

for (let line of lines) {
    const pair = line.split(',');
    const first = pair[0];
    const second = pair[1];

    const firstSections = getSections(first);
    const secondSections = getSections(second);

    if ((firstSections[0] >= secondSections[0] && firstSections[1] <= secondSections[1])
     || (firstSections[0] <= secondSections[0] && firstSections[1] >= secondSections[1])) {
        ++fullyContained;
     }
}

console.group("Part One");
console.log(fullyContained);
console.groupEnd();

let overlaps = 0;

for (let line of lines) {
    const pair = line.split(',');
    const first = pair[0];
    const second = pair[1];

    const firstSections = getSections(first);
    const secondSections = getSections(second);

    if ((firstSections[0] <= secondSections[1] && firstSections[1] >= secondSections[0])
     || (firstSections[1] >= secondSections[0] && firstSections[0] <= secondSections[1])) {
        ++overlaps;
     }
}

console.group("Part Two");
console.log(overlaps);
console.groupEnd();