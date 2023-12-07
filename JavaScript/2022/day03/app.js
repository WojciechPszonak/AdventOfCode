const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
getCommonChar = (str1, str2) => {
    const set1 = new Set(str1.split(""))
    const set2 = new Set(str2.split(""))

    for (let char of set1.values()) {
        if (set2.has(char)) {
            return char;
        }
    }

    return "";
}

let result = 0;

for (let line of lines) {
    const first = line.slice(0, line.length / 2);
    const second = line.slice(line.length / 2, line.length);

    const char = getCommonChar(first, second);
    const code = char.charCodeAt(0);

    if (code >= "a".charCodeAt(0)) {
        result += code - 96;
    }
    else {
        result += code - 38;
    }
}

console.group("Part One");
console.log(result);
console.groupEnd();

getCommonChar2 = (str1, str2, str3) => {
    const set1 = new Set(str1.split(""))
    const set2 = new Set(str2.split(""))
    const set3 = new Set(str3.split(""))

    for (let char of set1.values()) {
        if (set2.has(char) && set3.has(char)) {
            return char;
        }
    }

    return "";
}

let result2 = 0;
for (let lineIndex = 0; lineIndex < lines.length; lineIndex += 3) {
    const first = lines[lineIndex];
    const second = lines[lineIndex + 1];
    const third = lines[lineIndex + 2];

    const char = getCommonChar2(first, second, third);
    const code = char.charCodeAt(0);

    if (code >= "a".charCodeAt(0)) {
        result2 += code - 96;
    }
    else {
        result2 += code - 38;
    }
}

console.group("Part Two");
console.log(result2);
console.groupEnd();