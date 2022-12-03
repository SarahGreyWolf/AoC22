const UPPER = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
const LOWER = ['a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'];

if (import.meta.main) {
    const filePath = Deno.args[0];
    const text = await Deno.readTextFile(filePath);
    const lines = text.split("\n");
    let sum: number = 0;
    lines.forEach(line => {
        const chars = [...line];
        const compartmentMap: Map<string, number>[] = [new Map(), new Map()];
        const compartment: string[][] = [chars.slice(0, chars.length/2), chars.slice(chars.length/2, chars.length)];
        for (let i=0; i<compartmentMap.length; i++) {
            compartment[i].forEach(char => {
                if (!compartmentMap[i].has(char)) {
                    compartmentMap[i].set(char, 1);
                } else {
                    const currentValue = compartmentMap[i].get(char);
                    if (currentValue)
                        compartmentMap[i].set(char, currentValue + 1);
                }
            });
        }
        compartmentMap[0].forEach((value, key) => {
            if (compartmentMap[1].get(key)) {
                if (UPPER.includes(key))
                    sum += UPPER.indexOf(key) + 27;
                if (LOWER.includes(key))
                    sum += LOWER.indexOf(key) + 1;
            }
        });

        
    });
    console.log(sum);
}
