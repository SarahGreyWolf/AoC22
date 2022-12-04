const UPPER = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
const LOWER = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];

if (import.meta.main) {
    const filePath = Deno.args[0];
    const text = await Deno.readTextFile(filePath);
    const lines = text.split("\n");
    let sum = 0;
    const groups: string[][] = [];
    let group: string[] = [];
    lines.forEach(line => {
        if (group.length < 3) {
            group.push(line);
        } else {
            groups.push(group);
            group = [];
            group.push(line);
        }
        const chars = [...line];
        const compartmentMap: Map<string, number>[] = [new Map(), new Map()];
        const compartment: string[][] = [chars.slice(0, chars.length / 2), chars.slice(chars.length / 2, chars.length)];
        for (let i = 0; i < compartmentMap.length; i++) {
            compartmentMap[i] = mapChars(compartment[i]);
        }
        sum += getPrioritiesFromTwoMaps(compartmentMap[0], compartmentMap[1]);


    });
    console.log("Sum of priorities of duplicate items: "+sum);

    sum = 0;
    groups.forEach(group => {
        const items: Map<string, number> = new Map();
        group.forEach(member => {
            const chars = [...member];
            const person: Map<string, number> = mapChars(chars);
            person.forEach((_, key) => {
                if (!items.has(key)) {
                    items.set(key, 1);
                } else {
                    items.set(key, items.get(key) as number+1);
                }
            });
        });
        sum += getPriorities(items);
    });

    console.log("The sum of the badge priorities is: "+sum);
}

function getPriorities(map: Map<string, number>): number {
    let lastMostCommon = ['', 0];
    map.forEach((value, key) => {
        if (value > 1) {
            const [_, num] = lastMostCommon;
            if (value > num)
                lastMostCommon = [key, value];
        }
    });
    const [char, _] = lastMostCommon;
    if (UPPER.includes(char as string))
        return UPPER.indexOf(char as string) + 27;
    if (LOWER.includes(char as string))
        return LOWER.indexOf(char as string) + 1;
    return 0;
}

function getPrioritiesFromTwoMaps(mapA: Map<string, number>, mapB: Map<string, number>): number {
    let sum = 0;
    mapA.forEach((_, key) => {
        if (mapB.get(key)) {
            if (UPPER.includes(key))
                sum += UPPER.indexOf(key) + 27;
            if (LOWER.includes(key))
                sum += LOWER.indexOf(key) + 1;
        }
    });
    return sum;
}

function mapChars(chars: string[]): Map<string, number> {
    const result: Map<string, number> = new Map();
    chars.forEach(char => {
        if (!result.has(char)) {
            result.set(char, 1);
        } else {
            result.set(char, result.get(char) as number + 1);
        }
    });
    return result;
}
