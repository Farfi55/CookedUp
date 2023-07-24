import glob
import os
import sys
from datetime import datetime

path = os.environ['USERPROFILE'] + '/AppData/Local/Temp/ThinkEngineFacts/'
input_index = -1

brain_file = './PlayerPlanner1.asp'

solver = '.\\lib\\dlv2.exe --output 1'

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-i', '--index']:
        input_index =  int(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-p', '--path']:
        path = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-b', '--brain']:
        brain_file = sys.argv[i + 1]
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


list_of_files = glob.glob(path + "*.txt")
list_of_files.sort(key=os.path.getctime)

if len(list_of_files) == 0:
    print("No input file found")
    exit(1)


input_file = list_of_files[input_index]

print("executing with input file:", input_file)
print("created at:", datetime.fromtimestamp(os.path.getctime(input_file)).strftime('%Y-%m-%d %H:%M:%S'))

run_command = ' '.join([solver, brain_file, input_file])
print(run_command)
os.system(run_command)