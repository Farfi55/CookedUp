import sys

allow_list = []
allow_list_prefixes = []
deny_list = []
deny_list_prefixes = []

solver = 'dlv'

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-a', '--allow']:
        allow_list.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-A', '--allow-prefix']:
        allow_list_prefixes.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-d', '--deny']:
        deny_list.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-D', '--deny-prefix']:
        deny_list_prefixes.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-S', '--solver']:
        solver = sys.argv[i + 1].lower()
        i += 1
    elif sys.argv[i] in ['-h', '--help']:
        print(f"Usage: python output_parser.py")
        print(f"Options:")
        print(f"  -h, --help: show this help")
        print(f"  -a, --allow: allow predicate")
        print(f"  -A, --allow-prefix: allow predicate prefix")
        print(f"  -d, --deny: deny predicate")
        print(f"  -D, --deny-prefix: deny predicate prefix")
        print(f"  -S, --solver: solver (dlv, clingo)")
        exit(0)
    i += 1

if solver not in ['dlv', 'clingo']:
    print(f"Unknown solver: {solver}")
    exit(1)

uses_allow_list = len(allow_list) > 0 or len(allow_list_prefixes) > 0
uses_deny_list = len(deny_list) > 0 or len(deny_list_prefixes) > 0

if not uses_allow_list and not uses_deny_list:
    deny_list_prefixes.append('s_')
    deny_list_prefixes.append('c_')
    deny_list_prefixes.append('tmp_')
    uses_deny_list = True





output = ''
input_line = ''
prev_line = ''


while(True):
    prev_line = input_line
    input_line = input()

    if input_line == 'END':
        break

    if solver == 'dlv':
        is_predicate_line = input_line.startswith('{') and input_line.endswith('}')
    elif solver == 'clingo':
        is_predicate_line = prev_line.startswith('Answer')

    if not is_predicate_line:
        output += input_line + '\n'
        continue

    if solver == 'dlv':
        input_line = input_line[1:-1].replace('), ', ').\n')
    elif solver == 'clingo':
        input_line = input_line.replace(') ', ').\n')

    lines = input_line.splitlines()
    predicates = []

    for predicate in lines:

        
        predicate_name = predicate.split('(')[0]
        
        if predicate_name in deny_list:
            can_add_predicate = False
        elif predicate_name in allow_list:
            can_add_predicate = True
        else:
            can_add_predicate = True
            for deny_prefix in deny_list_prefixes:
                if predicate_name.startswith(deny_prefix):
                    can_add_predicate = False
                    break
            for allow_prefix in allow_list_prefixes:
                if predicate_name.startswith(allow_prefix):
                    can_add_predicate = True
                    break            

        if can_add_predicate:
            predicates.append(predicate)
         
    predicates.sort()
    output += '{\n' + '\n'.join(predicates) + '\n}\n'


print(output)
