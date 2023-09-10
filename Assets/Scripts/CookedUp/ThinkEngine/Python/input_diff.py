import glob
import os
import sys
import subprocess
import path_helper

input_path = path_helper.input_path + 'Player/'
index_file_A = -1
index_file_B = 0
input_file_pattern = '*.txt'
interactive = False

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-a', '--a-index']:
        index_file_A =  int(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-b', '--b-index']:
        index_file_B =  int(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-p', '--path']:
        input_path = path_helper.join_path(path_helper.input_path, sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-I', '--input-pattern']:
        input_file_pattern = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-i', '--interactive']:
        interactive = True
    elif sys.argv[i] in ['-h', '--help']:
        print(f"Usage: python run_with_input.py")
        print(f"Options:")
        print(f"  -h, --help: show this help")
        print(f"  -a, --a-index: input index A")
        print(f"  -b, --b-index: input index B")
        print(f"  -p, --path: input path")
        print(f"  -I, --input-pattern: input file pattern")
        print(f"  -i, --interactive: interactive mode")
        exit(0)
    i += 1


list_of_files = glob.glob(path_helper.join_path(input_path, input_file_pattern))
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
length = len(list_of_files)

if index_file_A < 0:
    index_file_A = length + index_file_A
if index_file_B < 0:
    index_file_B = length + index_file_B

print(f"A ({index_file_A}/{length-1}): {input_file_A}")
# print("created at:", datetime.fromtimestamp(os.path.getctime(input_file_A)).strftime('%Y-%m-%d %H:%M:%S'))

print(f"B ({index_file_B}/{length-1}): {input_file_B}")
# print("created at:", datetime.fromtimestamp(os.path.getctime(input_file_B)).strftime('%Y-%m-%d %H:%M:%S'))

run_command = ' '.join(['git diff --word-diff=color', input_file_A, input_file_B])

if interactive:
    os.system(run_command)

proc = subprocess.Popen(run_command, stdout=subprocess.PIPE, shell=True)
(out, err) = proc.communicate()
print(out.decode('utf-8'))
print()


