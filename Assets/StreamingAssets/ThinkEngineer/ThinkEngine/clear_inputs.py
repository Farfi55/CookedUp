import glob
import os
import sys
from datetime import datetime
from tqdm import tqdm

path = os.environ['USERPROFILE'] + '/AppData/Local/Temp/ThinkEngineFacts/'


i = 0
while i < len(sys.argv):
    if sys.argv[i] in ['-p', '-path']:
        path = sys.argv[i + 1]
        i += 1
    if sys.argv[i] in ['-h', '-help']:
        print(f"Usage: python clear_inputs.py [-p path]")
        exit(0)
    i += 1

# remove all files in path
list_of_files = glob.glob(path + "*.txt") # * means all if need specific format then *.csv
for f in tqdm(list_of_files):
    os.remove(f)

print("cleared all files in path:", path)
