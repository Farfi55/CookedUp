import glob
import os
import sys
from tqdm import tqdm
import path_helper

path = path_helper.input_path + "Player/"
files_pattern = "*.txt"
files = []

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-p', '--path']:
        path = path_helper.join_path(path_helper.input_path, sys.argv[i + 1])
        i += 1
    if sys.argv[i] in ['-I', '--input-pattern']:
        files_pattern = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-h', '--help']:
        print(f"Usage: python sort_input.py [files]")
        print(f"  if no files are given, all files in path will be sorted")
        print(f"Options:")
        print(f"  -h, --help: show this help")
        print(f"  -p, --path: input path")
        print(f"  -I, --input-pattern: input file pattern")
        exit(0)
    else:
        files.append(sys.argv[i])
    i += 1


if len(files) == 0:
    list_of_files = glob.glob(path + files_pattern)
    files = list_of_files

if len(files) == 0:
    print("No input file found")
    exit(1)

i = 0
for file in tqdm(files):
    try:
        lines = []
        with open(file, 'r') as f:
            lines = f.readlines()
            lines.sort()
        with open(file, 'w') as f:
            f.writelines(lines)
    except:
        print("Error sorting file:", file)
        exit(1)
    i += 1

