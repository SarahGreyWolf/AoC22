use std::env;
use std::fs::File;
use std::io;
use std::io::BufRead;
use std::io::BufReader;
use std::io::Read;

fn main() -> io::Result<()> {
    let mut args = env::args();
    args.next();
    let Some(f_path) = args.next() else {
        panic!("A file path must be provided as the first argument");
    };
    let Ok(file) = File::open(f_path) else {
        panic!("That file does not exist");
    };
    let reader = BufReader::new(file);

    let mut group_calories = get_calories(reader)?;

    let mut top_three_total: u32 = 0;

    for _ in 0..3 {
        let group_calories_clone = group_calories.clone();
        let calories_iter = group_calories_clone.iter().enumerate();
        let Some((index, most)) = calories_iter.max_by_key(|(_, c)| *c) else {
            panic!("Somehow no member was deemed to have the most calories");
        };
        println!("The elf with the most calories is {index} and has {most} calories");
        group_calories.remove(index);
        top_three_total += *most;
    }

    println!("The top 3 elves have {top_three_total} calories in total");

    Ok(())
}

fn get_calories(reader: BufReader<File>) -> io::Result<Vec<u32>> {
    let mut total_calories: Vec<u32> = vec![];
    let mut current_elf: u32 = 0;
    for line in reader.lines() {
        match line {
            Ok(l) => {
                if l.is_empty() {
                    total_calories.push(current_elf);
                    current_elf = 0;
                } else {
                    match l.parse::<u32>() {
                        Ok(res) => {
                            current_elf += res;
                        }
                        Err(e) => panic!("Error in file, could not parse number: {}", e),
                    }
                }
            }
            Err(e) => {
                return Err(e);
            }
        }
    }
    total_calories.push(current_elf);

    Ok(total_calories)
}
