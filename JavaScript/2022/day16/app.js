const read = (useTest) => {
    const fs = require("fs");
    const path = require("path");

    const fileName = useTest ? "input2.txt" : "input.txt";
    const data = fs.readFileSync(path.resolve(__dirname, fileName), "utf8");
    const lines = data.split("\n").slice(0, -1);

    return lines;
}

// Write code below this line
class Valve {
    open = false;

    constructor(name, flowRate, next) {
        this.name = name;
        this.flowRate = flowRate;
        this.next = next;
    }

    get nextValves() {
        return valves.filter(x => this.next.includes(x.name));
    }
}

// Parse input
const parse = (input) => {
    let valves = [];

    for (let line of input) {
        const values = line.split(" ");

        const name = values[1];
        const flowRate = +values[4].split("=")[1].slice(0, -1);
        const next = values.filter(x => x === x.toUpperCase()).slice(1).map(x => x.replace(",", ""));

        const valve = new Valve(name, flowRate, next);
        valves.push(valve);
    }

    return valves;
}

const input = read();
const valves = parse(input);

console.group("Part One");
console.log(valves);
console.groupEnd();

console.group("Part Two");
console.log();
console.groupEnd();