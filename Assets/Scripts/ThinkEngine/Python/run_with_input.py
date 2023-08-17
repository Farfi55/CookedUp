import glob
import os
import sys
from datetime import datetime

PROJECT_PATH = 'C:/Dev/CookedUp'

base_input_path = os.environ['USERPROFILE'] + '/AppData/Local/Temp/ThinkEngineFacts/'
input_path = ''
input_index = -1
input_file_pattern = ''

streaming_assets_path = PROJECT_PATH + '/Assets/StreamingAssets/ThinkEngineer/ThinkEngine/'

player_brain_files_patterns = [
    streaming_assets_path + '**/Player*.asp',
    streaming_assets_path + '**/Common*.asp',
    streaming_assets_path + '**/Map*.asp',
]

brain_files_patterns = []


brain_files = []

solver = streaming_assets_path + 'lib/dlv2.exe --no-facts --t -n 10'

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-i', '--index']:
        input_index =  int(sys.argv[i + 1])
        i += 1
    if sys.argv[i] in ['-I', '--input-pattern']:
        input_file_pattern = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-p', '--path']:
        input_path = base_input_path + sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-b', '--brain']:
        brain_files.append(streaming_assets_path + sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-B', '--brain-pattern']:
        brain_files_patterns.append(streaming_assets_path + sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-s', '--solver']:
        solver = streaming_assets_path + 'lib/' + sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-h', '--help']:
        print(f"Usage: python run_with_input.py")
        print(f"Options:")
        print(f"  -h, --help: show this help")
        print(f"  -i, --index: input index")
        print(f"  -I, --input-pattern: input file pattern")
        print(f"  -p, --path: input path")
        print(f"  -b, --brain: brain file")
        print(f"  -B, --brain-pattern: brain file pattern")
        print(f"  -s, --solver: solver path and options")
        exit(0)
    i += 1


if(input_path == ''):
    input_path = base_input_path + 'Player/'



list_of_files = glob.glob(input_path + "*.txt")
list_of_files.sort(key=os.path.getctime)

if len(list_of_files) == 0:
    print("No input file found")
    exit(1)


if input_file_pattern != '':
    import fnmatch
    list_of_files = fnmatch.filter(list_of_files, input_file_pattern)    

input_file = list_of_files[input_index]

print("executing with input file:", input_file)
print("created at:", datetime.fromtimestamp(os.path.getctime(input_file)).strftime('%Y-%m-%d %H:%M:%S'))

if(brain_files == []):
    if(brain_files_patterns == []):
        brain_files_patterns == player_brain_files_patterns

    for brain_files_pattern in brain_files_patterns:
        brain_files += glob.glob(brain_files_pattern, recursive=True)

print("using brain files:")
print("\n".join(brain_files))
print()

run_command = ' '.join([solver, *brain_files, input_file])
print(run_command)
print()

import subprocess

proc = subprocess.Popen(run_command, stdout=subprocess.PIPE, shell=True)
(out, err) = proc.communicate()
print("program output:")
print(out.decode('utf-8'))
print("END")

