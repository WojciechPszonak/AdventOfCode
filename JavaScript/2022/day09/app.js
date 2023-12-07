const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
const printResult = async (array, hx, hy, tx, ty) => {
    await new Promise((resolve) => setTimeout(() => {
        console.clear();
        for (let j = 0; j < array[0].length; ++j) {
            let text = "";
            for (let i = 0; i < array.length; ++i) {
                if (i === hx && j === hy) {
                    text += "H ";
                }
                else if (i === tx && j === ty) {
                    text += "T ";
                }
                else if (array[i][j]) {
                    text += "# ";
                }
                else {
                    text += ". ";
                }
            }
            console.log(`${text}\n`);
        };
        resolve();
    }, 200));
}

const printResult2 = async (array, knots) => {
    await new Promise((resolve) => setTimeout(() => {
        console.clear();
        const displayArray = array.map((arr) => arr.slice().fill("."));

        for (let j = 0; j < array[0].length; ++j) {
            for (let i = 0; i < array.length; ++i) {
                for (let ki = 0; ki < knots.length; ++ki) {
                    if (!["H", ...Array(knots.length).keys()].includes(displayArray[i][j])) {
                        if (i === knots[ki][0] && j === knots[ki][1]) {
                            displayArray[i][j] = ki ? ki : "H";
                            break;
                        }
                        else if (array[i][j]) {
                            displayArray[i][j] = "#";
                        }
                    }
                }
            }
            const text = displayArray.map(x => x[j]).join(" ");
            console.log(`${text}\n`);
        };
        resolve();
    }, 200));
}

const printResult3 = (array, knots) => {
    const displayArray = array.map((arr) => arr.slice().fill("."));
    let contents = "";
    for (let j = 0; j < array[0].length; ++j) {
        for (let i = 0; i < array.length; ++i) {
            for (let ki = 0; ki < knots.length; ++ki) {
                if (!["H", ...Array(knots.length).keys()].includes(displayArray[i][j])) {
                    if (i === knots[ki][0] && j === knots[ki][1]) {
                        displayArray[i][j] = ki ? ki : "H";
                        break;
                    }
                    else if (array[i][j]) {
                        displayArray[i][j] = "#";
                    }
                }
            }
        }
        const text = displayArray.map(x => x[j]).join("");
        contents += `${text}\n`;
    }

    fs.writeFileSync(path.resolve(__dirname, "result.txt"), contents);
}

const updatePosition = (x, y, direction) => {
    switch (direction) {
        case "U":
            --y;
            break;
        case "D":
            ++y;
            break;
        case "L":
            --x;
            break;
        case "R":
            ++x;
            break;
    }

    return [x, y];
}

const partOne = async () => {
    const array = Array(6).fill().map(() => Array(5).fill(0));

    let hx = 0;
    let hy = 4;
    let tx = hx;
    let ty = hy;
    array[tx][ty] = 1;

    for (let line of lines) {
        const values = line.split(" ");
        const direction = values[0];
        const value = +values[1];

        for (let i = 0; i < value; ++i) {
            [old_hx, old_hy] = [hx, hy];
            [hx, hy] = updatePosition(hx, hy, direction);

            if (Math.abs(hx - tx) > 1 || Math.abs(hy - ty) > 1) {
                [tx, ty] = [old_hx, old_hy];
            }

            // Update T visit
            array[tx][ty] = 1;

            await printResult(array, hx, hy, tx, ty);
        }
    }

    return array;
}


console.group("Part One");
// partOne().then((array) => {
//     console.log(array.reduce((agg, curr) => agg += curr.filter(x => x == 1).length, 0))
// });
console.groupEnd();

const partTwo = async () => {
    const sx = 90;
    const sy = 200;

    const array = Array(400).fill().map(() => Array(300).fill(0));
    const knots = Array(10).fill().map(() => [sx, sy].slice())

    array[knots[9][0]][knots[9][1]] = 1;

    for (let line of lines) {
        const values = line.split(" ");
        const direction = values[0];
        const value = +values[1];

        for (let i = 0; i < value; ++i) {
            for (let ki = 0; ki < knots.length; ++ki) {
                if (ki === 0) {
                    knots[ki] = updatePosition(knots[ki][0], knots[ki][1], direction);
                }
                else {
                    const xdiff = knots[ki - 1][0] - knots[ki][0];
                    const ydiff = knots[ki - 1][1] - knots[ki][1];

                    if (Math.abs(xdiff) > 1 || Math.abs(ydiff) > 1) {
                        const xsign = Math.sign(xdiff);
                        const ysign = Math.sign(ydiff);
                        knots[ki] = [knots[ki][0] + xsign, knots[ki][1] + ysign];
                    }
                }
            }

            // Update T visit
            array[knots[9][0]][knots[9][1]] = 1;

            // await printResult2(array, knots);
        }
    }

    printResult3(array, knots);

    return array;
}

console.group("Part Two");
partTwo().then((array) => {
    console.log(array.reduce((agg, curr) => agg += curr.filter(x => x === 1).length, 0))
});
console.groupEnd();