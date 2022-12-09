use std::env;
use std::error::Error;
use std::fs::File;
use std::io::BufRead;
use std::io::BufReader;
use std::ops::Range;

fn main() -> Result<(), Box<dyn Error>> {
    let mut args = env::args();
    args.next();
    let Some(f_path) = args.next() else {
        panic!("A file path must be provided as the first argument");
    };
    let Ok(file) = File::open(f_path) else {
        panic!("That file does not exist");
    };
    let reader = BufReader::new(file);

    let mut pairs: Vec<(Range<u32>, Range<u32>)> = vec![];
    get_pairs(reader, &mut pairs)?;

    let mut contains_others_totally = 0;
    for (a, b) in &pairs {
        if ((a.start <= b.start && b.start <= a.end) && (a.end >= b.end && b.end >= a.start))
            || ((b.start <= a.start && a.start <= b.end) && (b.end >= a.end && a.end >= b.start))
        {
            contains_others_totally += 1;
            continue;
        }
    }
    println!("There are {contains_others_totally} pairs that have total overlapping ranges");

    let mut contains_others = 0;
    for (a, b) in &pairs {
        if (a.start <= b.start && b.start <= a.end) || (b.start <= a.start && a.start <= b.end) {
            contains_others += 1;
            continue;
        }
        if (a.end >= b.end && b.end >= a.start) || (b.end >= a.end && a.end >= b.start) {
            contains_others += 1;
            continue;
        }
    }
    println!("There are {contains_others} pairs that have overlapping ranges");

    Ok(())
}

fn get_pairs(
    reader: BufReader<File>,
    pairs: &mut Vec<(Range<u32>, Range<u32>)>,
) -> Result<(), Box<dyn Error>> {
    for line in reader.lines() {
        match line {
            Ok(l) => {
                let mut pair_result: (Range<u32>, Range<u32>) = (0..0, 0..0);
                let mut number: u8 = 0;
                let pair = l.split(',');
                for range in pair {
                    let mut values = range.split('-');
                    let Some(start) = values.next() else {
                        panic!("Could not get start value from range text");
                    };
                    let Some(end) = values.next() else {
                        panic!("Could not get end value from range text");
                    };
                    let r: Range<u32> = Range {
                        start: start.parse()?,
                        end: end.parse()?,
                    };
                    if number == 0 {
                        pair_result.0 = r;
                        number += 1;
                    } else {
                        pair_result.1 = r;
                    }
                }
                pairs.push(pair_result);
            }
            Err(e) => return Err(Box::new(e)),
        }
    }
    Ok(())
}
