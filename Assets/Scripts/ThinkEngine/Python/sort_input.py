import glob
import os
import sys
from tqdm import tqdm

path = os.environ['USERPROFILE'] + '/AppData/Local/Temp/ThinkEngineFacts/'

files = []

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-p', '--path']:
        path = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-h', '--help']:
        print(f"Usage: python sort_input.py [files]")
        print(f"  if no files are given, all files in path will be sorted")
        print(f"Options:")
        print(f"  -h, --help: show this help")
        print(f"  -p, --path: input path")
        exit(0)
    else:
        files.append(sys.argv[i])
    i += 1


if len(files) == 0:
    list_of_files = glob.glob(path + "*.txt")
    files = list_of_files

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

