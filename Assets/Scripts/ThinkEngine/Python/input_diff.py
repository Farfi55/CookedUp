import glob
import os
import sys
import subprocess
from datetime import datetime

path = os.environ['USERPROFILE'] + '/AppData/Local/Temp/ThinkEngineFacts/'
index_file_A = -1
index_file_B = -2

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-a', '--a-index']:
        index_file_A =  int(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-b', '--b-index']:
        index_file_B =  int(sys.argv[i + 1])
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


try:
    input_file_A = list_of_files[index_file_A]
    input_file_B = list_of_files[index_file_B]
except IndexError as e:
    print("Invalid index")
    print(e)
    exit(1)


os.system(f"python sort_input.py {input_file_A} {input_file_B}")


print("Difference between:")

print("A:", input_file_A)
print("created at:", datetime.fromtimestamp(os.path.getctime(input_file_A)).strftime('%Y-%m-%d %H:%M:%S'))

print("B:", input_file_B)
print("created at:", datetime.fromtimestamp(os.path.getctime(input_file_B)).strftime('%Y-%m-%d %H:%M:%S'))

run_command = ' '.join(['git diff --word-diff=color', input_file_A, input_file_B])
print(run_command)
print()

os.system(run_command)

