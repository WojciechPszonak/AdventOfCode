const read = (useTest) => {
    const fs = require("fs");
    const path = require("path");

    const fileName = useTest ? "input2.txt" : "input.txt";
    const data = fs.readFileSync(path.resolve(__dirname, fileName), "utf8");
    const lines = data.split("\n").slice(0, -1);

    return lines;
}

// Write code below this line
const maxRed = 12;
const maxGreen = 13;
const maxBlue = 14;

class Game {
    id;
    red = 0;
    green = 0;
    blue = 0;

    constructor(id) {
        this.id = id;
    }
}

const parse = (input) => {
    let id = 1;
    const games = [];

    for (let line of input) {
        const game = new Game(id++);

        line = line.replace(/Game (\d+): /, "");
        const sets = line.split(";");

        for (let set of sets) {
            const redMatch = set.match(/(\d+) red/d);
            const greenMatch = set.match(/(\d+) green/d);
            const blueMatch = set.match(/(\d+) blue/d);

            const red = redMatch && +redMatch[1] || 0;
            const green = greenMatch && +greenMatch[1] || 0;
            const blue = blueMatch && +blueMatch[1] || 0;

            if (red > game.red) game.red = red;
            if (green > game.green) game.green = green;
            if (blue > game.blue) game.blue = blue;
        }

        games.push(game);
    }

    return games;
}

const parse2 = (input) => {
    const games = [];

    for (let line of input) {
        const game = new Game(+line.match(/Game (\d+)/)[1]);

        const redMatches = [...line.matchAll(/(\d+) red/g)];
        const greenMatches = [...line.matchAll(/(\d+) green/g)];
        const blueMatches = [...line.matchAll(/(\d+) blue/g)];

        const mapping = match => +match[1];
        const red = redMatches && Math.max(...redMatches.map(mapping)) || 0;
        const green = greenMatches && Math.max(...greenMatches.map(mapping)) || 0;
        const blue = blueMatches && Math.max(...blueMatches.map(mapping)) || 0;

        if (red > game.red) game.red = red;
        if (green > game.green) game.green = green;
        if (blue > game.blue) game.blue = blue;

        games.push(game);
    }

    return games;
}

const part1 = (data) => {
    return data.filter(x => x.red <= maxRed && x.green <= maxGreen && x.blue <= maxBlue)
        .reduce((prev, curr) => prev += curr.id, 0);
}

const part2 = (data) => {
    return data.map(x => x.red * x.green * x.blue)
        .reduce((prev, curr) => prev += curr);
}

// Output:
const input = read();
const data = parse2(input);

console.group("Part One");
console.log(part1(data));
console.groupEnd();

console.group("Part Two");
console.log(part2(data));
console.groupEnd();