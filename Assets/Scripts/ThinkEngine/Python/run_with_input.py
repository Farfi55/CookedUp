import glob
import os
import sys
from datetime import datetime

input_path = os.environ['USERPROFILE'] + '/AppData/Local/Temp/ThinkEngineFacts/Player/'
input_index = -1

streaming_assets_path = '../../StreamingAssets/ThinkEngineer/ThinkEngine/'

brain_files_patterns = [
    streaming_assets_path + 'Player*.asp',
    streaming_assets_path + 'Common*.asp',
]
brain_files = []

solver = streaming_assets_path + 'lib/dlv2.exe --output 1'

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-i', '--index']:
        input_index =  int(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-p', '--path']:
        input_path = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-b', '--brain']:
        brain_files.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-s', '--solver']:
        solver = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-h', '--help']:
        print(f"Usage: python run_with_input.py")
        print(f"Options:")
        print(f"  -h, --help: show this help")
        print(f"  -i, --index: input index")
        print(f"  -p, --path: input path")
        print(f"  -b, --brain: brain file")
        print(f"  -s, --solver: solver path and options")
        exit(0)
    i += 1


list_of_files = glob.glob(input_path + "*.txt")
list_of_files.sort(key=os.path.getctime)

if len(list_of_files) == 0:
    print("No input file found")
    exit(1)


input_file = list_of_files[input_index]

print("executing with input file:", input_file)
print("created at:", datetime.fromtimestamp(os.path.getctime(input_file)).strftime('%Y-%m-%d %H:%M:%S'))

if(brain_files == []):
    for brain_files_pattern in brain_files_patterns:
        brain_files += glob.glob(brain_files_pattern)


run_command = ' '.join([solver, *brain_files, input_file])
print(run_command)
os.system(run_command)
