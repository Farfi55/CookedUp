import glob
import os
import sys
from datetime import datetime

path = os.environ['USERPROFILE'] + '/AppData/Local/Temp/ThinkEngineFacts/'
input_index = -1

brain_file = './PlayerPlanner1.asp'

solver = '.\\lib\\dlv2.exe --output 1'
i = 0
while i < len(sys.argv):
    if sys.argv[i] in ['-i', '--index']:
        input_index =  sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-p', '-path']:
        path = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-b', '-brain']:
        brain_file = sys.argv[i + 1]
        i += 1
    elif sys.argv[i] in ['-s', '-solver']:
        solver = sys.argv[i + 1]
        i += 1

    i += 1


list_of_files = glob.glob(path + "*.txt") # * means all if need specific format then *.csv
list_of_files.sort(key=os.path.getmtime)

if len(list_of_files) == 0:
    print("No input file found")
    exit(1)


input_file = list_of_files[input_index]

print("executing with input file:", input_file)
print("created at:", datetime.fromtimestamp(os.path.getmtime(input_file)).strftime('%Y-%m-%d %H:%M:%S'))

run_command = ' '.join([solver, brain_file, input_file])
print(run_command)
os.system(run_command)
