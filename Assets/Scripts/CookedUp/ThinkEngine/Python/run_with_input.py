import glob
import os
import sys
from datetime import datetime
import path_helper
import platform

PROJECT_PATH = path_helper.project_path
base_input_path = path_helper.input_path 

input_path = ''
input_index = -1
input_file_pattern = ''

solver_pattern = ''
solver_path = ''
solver_options = ''

verbose_level = 1

streaming_assets_path = path_helper.think_engine_streaming_assets_path

player_brain_files_patterns = [
    streaming_assets_path + '**/Player*.asp',
    streaming_assets_path + '**/Common*.asp',
    streaming_assets_path + '**/Map*.asp',
]

brain_files_patterns = []


brain_files = []

solvers_path = path_helper.solvers_path


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
        solver_path = os.path.join(solvers_path, sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-S', '--solver-pattern']:
        solver_pattern = sys.argv[i + 1]
        i += 1    
    elif sys.argv[i] in ['-o', '--solver-options']:
        solver_options = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-v', '--verbose']:
        verbose_level = int(sys.argv[i + 1])
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
        print(f"  -s, --solver: solver path")
        print(f"  -S, --solver-pattern: solver path pattern, e.g. 'dlv2' or 'clingo'")
        print(f"  -o, --solver-options: solver options")
        print(f"  -v, --verbose: verbose level")
        exit(0)
    i += 1


if(input_path == ''):
    input_path = base_input_path + 'Player/'

if(solver_path == ''):
    solver_name = ''
    #iterate over all files in solvers_path
    solver_files_paths = glob.glob(os.path.join(solvers_path, '*'))
    os_name = platform.system()
    
    #find solver
    for solver_file_path in solver_files_paths:
        solver_file_name = os.path.basename(solver_file_path)
        if solver_file_name.endswith('.meta'):
            continue

        if solver_pattern != '' and solver_pattern.lower() not in solver_file_name.lower():
            continue

        if os_name == 'Windows' and ('win' in solver_file_name.lower() or solver_file_name.endswith('.exe')):
            solver_name = solver_file_name
            break
        if os_name == 'Linux' and 'linux' in solver_file_name.lower():
            solver_name = solver_file_name
            break
        if os_name == 'Darwin' and 'mac' in solver_file_name.lower():
            solver_name = solver_file_name
            break

    if solver_name == '':
        print("No solver found")
        exit(1)
    solver_path = os.path.join(solvers_path, solver_name)



list_of_files = glob.glob(input_path + "*.txt")
list_of_files.sort(key=os.path.getctime)

if len(list_of_files) == 0:
    print("No input file found")
    exit(1)


if input_file_pattern != '':
    import fnmatch
    list_of_files = fnmatch.filter(list_of_files, input_file_pattern)    

input_file = list_of_files[input_index]

print("START")
if(verbose_level > 0):
    print("\nexecuting with input file:")
    print("\t" + input_file.removeprefix(input_path))
    if(verbose_level > 1):
        print("\tcreated at:", datetime.fromtimestamp(os.path.getctime(input_file)).strftime('%Y-%m-%d %H:%M:%S'))

if(brain_files == []):
    if(brain_files_patterns == []):
        brain_files_patterns == player_brain_files_patterns

    for brain_files_pattern in brain_files_patterns:
        brain_files += glob.glob(brain_files_pattern, recursive=True)

if(verbose_level > 1):    
    print("\nusing brain files:")
    for brain_file in brain_files:
        print("\t" + brain_file.removeprefix(streaming_assets_path))
    print()

run_command = ' \\\n\t'.join([solver_path, solver_options, *brain_files, input_file])
if(verbose_level > 2):
    print("command:")
    print(run_command)
    print()

import subprocess

proc = subprocess.Popen(run_command, stdout=subprocess.PIPE, shell=True)
(out, err) = proc.communicate()

if(verbose_level > 0):
    print("program output:")
print(out.decode('utf-8'))
print("END")

