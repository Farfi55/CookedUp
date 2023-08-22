import glob
import os
import sys
import path_helper  
from tqdm import tqdm

input_path = path_helper.input_path

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-p', '-path']:
        input_path = path_helper.input_path + sys.argv[i + 1]
        i += 1
    if sys.argv[i] in ['-h', '-help']:
        print(f"Usage: python clear_inputs.py [-p path]")
        exit(0)
    i += 1

# remove all files in path
list_of_files = glob.glob(input_path + "*.txt")
list_of_files += glob.glob(input_path + "**/*.txt")

for f in tqdm(list_of_files):
    os.remove(f)

print("cleared all files in path:", input_path)
